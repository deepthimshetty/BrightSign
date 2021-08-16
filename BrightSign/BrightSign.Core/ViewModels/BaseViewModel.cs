using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Interface;
using BrightSign.Core.ViewModels.SearchUnits;
using BrightSign.Localization;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MvvmCross.Localization;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using Plugin.Connectivity;
using MvvmCross.Navigation;

namespace BrightSign.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel, IDisposable
    {
        protected IMvxMessenger Messenger;
        protected IDialogService DialogService;
        protected IMvxNavigationService _navigationService;
        public BaseViewModel()
        {
            CurrentDevice = Constants.ActiveDevice;
        }
        

        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mobilogix.Core.Code.ViewModels.BaseViewModel"/> class.
        /// </summary>
        /// <param name="messenger">Messenger.</param>
        /// <param name="_dialogService">Dialog service.</param>
        public BaseViewModel(IMvxMessenger messenger, IDialogService _dialogService = null, IMvxNavigationService navigationService = null)
        {
            Messenger = messenger;
            if (_dialogService != null)
            {
                DialogService = _dialogService;
            }
            CurrentDevice = Constants.ActiveDevice;
            if(_navigationService != null)
            {
                _navigationService = navigationService;
            }
        }

        /// <summary>
        /// The view title.
        /// </summary>
        private string _viewTitle;
        public string ViewTitle
        {
            get { return _viewTitle; }
            set
            {
                _viewTitle = value;
                RaisePropertyChanged(() => ViewTitle);
            }
        }

        public IMvxLanguageBinder TextSource =>
            new MvxLanguageBinder("", GetType().Name);

        protected async Task ReloadDataAsync()
        {
            try
            {
                await InitializeAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        protected virtual Task InitializeAsync()
        {
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            Messenger = null;
        }

        /// <summary>
        /// Checks the network connectivity.
        /// </summary>
        /// <returns><c>true</c>, if network connectivity was checked, <c>false</c> otherwise.</returns>
        /// <param name="showError">If set to <c>true</c> show error.</param>
        public bool CheckNetworkConnectivity(bool showError = true)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }
            else
            {
                if (showError)
                {
                    DialogService.ShowAlertAsync(Strings.check_your_network, Strings.error, Strings.ok);
                }
                return false;
            }
        }

        /// <summary>
        /// is busy.
        /// </summary>
        bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }

            set
            {
                if (_IsBusy != value)
                {
                    _IsBusy = value;
                    RaisePropertyChanged("IsBusy");
                    if (value)
                    {
                        if (DialogService != null)
                        {
                            InvokeOnMainThread(() =>
                            {
                                DialogService.ShowLoading();
                            });

                        }
                        else
                        {
                            InvokeOnMainThread(() =>
                            {
                                DialogService = Mvx.Resolve<IDialogService>();
                                DialogService.ShowLoading();
                            });
                        }
                        //UserDialogs.Instance.ShowLoading();
                    }
                    else
                    {
                        if (DialogService != null)
                        {
                            InvokeOnMainThread(() =>
                            {
                                DialogService.HideLoading();
                            });
                        }
                        else
                        {
                            InvokeOnMainThread(() =>
                            {
                                DialogService = Mvx.Resolve<IDialogService>();
                                DialogService.HideLoading();
                            });
                        }

                        //UserDialogs.Instance.HideLoading();
                    }
                }

            }
        }

        public IMvxCommand ChangeDeviceCommand
        {
            get
            {
                return new MvxCommand<int>(ChangeDeviceClick);
            }
        }

        private async void ChangeDeviceClick(int obj)
        {
            BSUtility.Instance.DisconnectSocket();
            //ShowViewModel<SearchUnitsViewModel>();
            await _navigationService.Navigate<SearchUnitsViewModel>();
        }

        private BSDevice _CurrentDevice;
        public BSDevice CurrentDevice
        {
            get
            {
                return _CurrentDevice;
            }
            set
            {
                _CurrentDevice = value;
                RaisePropertyChanged(() => CurrentDevice);
            }
        }

        public bool IsValidIPAddress(string iPAddress)
        {
            if (string.IsNullOrWhiteSpace(iPAddress))
            {
                return false;
            }
            return true;
        }

        public void Refresh()
        {
            CurrentDevice = Constants.ActiveDevice;
        }
    }
}
