using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Database;
using BrightSign.Core.Utility.Interface;
using BrightSign.Core.Utility.Messages;
using BrightSign.Core.Utility.Web;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Plugin.DeviceInfo;
using Sockets.Plugin;
using Zeroconf;

namespace BrightSign.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        public VariablesViewModel variablesViewModel;
        public ActionsViewModel actionsViewModel;
        public DiagnosticsViewModel diagnosticViewModel;
        public SnapshotsViewModel snapshotsViewModel;
        public SettingsViewModel settingsViewModel;


        MvxSubscriptionToken DashboardRefreshToken;


        private readonly IMvxNavigationService _navigationService;

        public IMvxAsyncCommand ShowVariablesViewModelCommand { get; private set; }
        public IMvxAsyncCommand ShowActionsViewModelCommand { get; private set; }
        public IMvxAsyncCommand ShowDiagnosticsViewModelCommand { get; private set; }
        public IMvxAsyncCommand ShowGalleryViewModelCommand { get; private set; }
        public IMvxAsyncCommand ShowSettingsViewModelCommand { get; private set; }

        public MvxCommand SettingsCommand
        {
            get
            {
                return new MvxCommand(() => SetSettingsView());
            }
        }


        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        bool _AcquireLock = false;
        public bool AcquireLock
        {
            get
            {
                return _AcquireLock;
            }
            set
            {
                _AcquireLock = value;
                RaisePropertyChanged("AcquireLock");
            }
        }

        public MainViewModel(IMvxMessenger messenger, IUserPreferences userPreferences) : base(messenger)
        {
            try
            {
                Constants.UserDefinedActionsList = DBHandler.Instance.GetActionsfromDBforPresentation(Constants.ActivePresentation);

                if (CrossDeviceInfo.Current.Platform == Plugin.DeviceInfo.Abstractions.Platform.Android)
                {
                    variablesViewModel = new VariablesViewModel(messenger);
                    diagnosticViewModel = new DiagnosticsViewModel(messenger);
                    actionsViewModel = new ActionsViewModel(messenger, userPreferences);
                    settingsViewModel = new SettingsViewModel(messenger, userPreferences);
					snapshotsViewModel = new SnapshotsViewModel(messenger);
                }

                _navigationService = Mvx.Resolve<IMvxNavigationService>();

                ShowVariablesViewModelCommand = new MvxAsyncCommand(async () => await NavigateToVariables());
                ShowActionsViewModelCommand = new MvxAsyncCommand(async () => await NavigateToActions());
                ShowDiagnosticsViewModelCommand = new MvxAsyncCommand(async () => await NavigateToDiagnostics());
                ShowGalleryViewModelCommand = new MvxAsyncCommand(async () => await NavigateToGallery());
                ShowSettingsViewModelCommand = new MvxAsyncCommand(async () => await NavigateToSettings());

                if (DashboardRefreshToken == null)
                {
                    DashboardRefreshToken = Messenger.Subscribe<DashboardRefreshMessage>(OnDashboardRefreshResponse);
                }


               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private async void OnDashboardRefreshResponse(DashboardRefreshMessage obj)
        {
            if (obj != null)
            {
                if (obj.device != null)
                {
                    if (obj.device.IpAddress != Constants.ActiveDevice.IpAddress)
                    {
                        Constants.ScannedDevices.ForEach(o => o.IsDefault = false);
                        Constants.FullDevices.ForEach(o => o.IsDefault = false);
                        int index = Constants.FullDevices.IndexOf(obj.device);
                        Constants.FullDevices[index].IsDefault = true;

                        Constants.ActiveDevice = obj.device;

                        if (Constants.ActiveDevice.IsOnline)
                        {
                            await LoadDeviceData();
                        }
                        else
                        {
                            Messenger.Publish(new ActiveDeviceMessage(this, DeviceStatus.Disconnected));
                            Messenger.Publish(new LoadButtonsMessage(this, DeviceStatus.Disconnected));
                            Messenger.Publish(new SettingsMessage(this));
                        }

                    }
                }
            }
        }

        public async Task NavigateToVariables()
        {
            ViewTitle = TitleType.Variables.ToString();
            await _navigationService.Navigate<VariablesViewModel>();
        }

        public async Task NavigateToActions()
        {
            ViewTitle = TitleType.Actions.ToString();
            await _navigationService.Navigate<ActionsViewModel>();
        }

        public async Task NavigateToDiagnostics()
        {
            ViewTitle = TitleType.Diagnostics.ToString();
            await _navigationService.Navigate<DiagnosticsViewModel>();
        }

        public async Task NavigateToGallery()
        {
            ViewTitle = TitleType.Actions.ToString();
            await _navigationService.Navigate<SnapshotsViewModel>();
        }

        public async Task NavigateToSettings()
        {
            ViewTitle = TitleType.Diagnostics.ToString();
            await _navigationService.Navigate<SettingsViewModel>();
        }


        private async Task LoadDeviceData()
        {
            try
            {
                //IsBusy = true;
                //await GetDeviceDetails(Constants.ActiveDevice.IpAddress);
                bool IsSuccess = await BSUtility.Instance.GetDeviceRemoteData();

                Messenger.Publish(new ActiveDeviceMessage(this, IsSuccess ? DeviceStatus.Connected : DeviceStatus.Disconnected));
                Messenger.Publish(new LoadButtonsMessage(this, IsSuccess ? DeviceStatus.Connected : DeviceStatus.Disconnected));
                Messenger.Publish(new SettingsMessage(this));
                if (IsSuccess)
                {
                    await BSUtility.Instance.InitializeSocketListening();
                }

                if (settingsViewModel != null)
                {
                    settingsViewModel.SetData();
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                //IsBusy = false;
            }
        }


        private void SetSettingsView()
        {
            ViewTitle = "Settings";
            ShowViewModel<SettingsViewModel>();
        }

    }


}

