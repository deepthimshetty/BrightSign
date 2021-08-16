using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Database;
using BrightSign.Core.Utility.Interface;
using BrightSign.Core.Utility.Messages;
using BrightSign.Core.ViewModels.AddDevice;
using BrightSign.Localization;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace BrightSign.Core.ViewModels
{
    public class ActiveViewModel : BaseViewModel
    {
        MvxSubscriptionToken AddDeviceToken;
        private readonly IMvxNavigationService _navigationServic;
        public ActiveViewModel(IMvxMessenger messenger, IMvxNavigationService navigationService) : base(messenger)
        {
            _navigationServic = navigationService;
            ActiveItemSource = new ObservableCollection<BSDevice>();
        }

        private ObservableCollection<BSDevice> _activeItemSource;
        public ObservableCollection<BSDevice> ActiveItemSource
        {
            get { return _activeItemSource; }
            set
            {
                _activeItemSource = value;
                RaisePropertyChanged(() => ActiveItemSource);
            }

        }


        public IMvxCommand AddDeviceCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    AddDeviceClick();
                });
            }
        }

        private void AddDeviceClick()
        {
            //Mvx.Resolve<ICustomAlert>().ShowCustomAlert(true, "BX", "bx");

            //ShowViewModel<AddDeviceViewModel>();
            _ = NavigateToViewmodelMethod();
            if (AddDeviceToken == null)
            {
                AddDeviceToken = Messenger.Subscribe<AddDeviceRefreshMessage>(OnAddDeviceResponse);
            }
        }

        public async Task NavigateToViewmodelMethod()
        {
            await _navigationServic.Navigate<AddDeviceViewModel>();
        }

        private void OnAddDeviceResponse(AddDeviceRefreshMessage obj)
        {
            if (obj.device != null)
            {
                var item = Constants.FullDevices.FirstOrDefault(o => o.IpAddress == obj.device.IpAddress);
                {
                    if (item ==null)
                    {
                        Constants.FullDevices.Add(obj.device);
                        ActiveItemSource.Add(obj.device);
                    }
                }

                DBHandler.Instance.InsertorReplaceDevice(obj.device);

                
            }

            Messenger.Unsubscribe<AddDeviceRefreshMessage>(AddDeviceToken);
            AddDeviceToken = null;

        }
        public IMvxCommand ListSelectorCommand
        {
            get
            {
                return new MvxCommand<BSDevice>(async (BSDevice obj) =>
                {
                    await ItemClick(obj);
                });
            }
        }


        private async Task ItemClick(BSDevice obj)
        {
            IsBusy = true;

            obj.IsSelected = true;
            obj.IsDefault = true;
            Constants.ActiveDevice = obj;

            try
            {
                var status = await LoadDeviceData();
                switch (status)
                {
                    case Status.Success:
                        Constants.UserDefinedActionsList = new ObservableCollection<BSUdpAction>();
                        IsBusy = false;
                        Constants.IsCredentialsRequiredforSnapshots = false;
                        Constants.LoginUser = string.Empty;
                        Constants.LoginPwd = string.Empty;

                        //ShowViewModel<MainViewModel>();
                        await _navigationServic.Navigate<MainViewModel>();
                        break;
                    case Status.Failure:
                        IsBusy = false;
                        await Mvx.Resolve<IDialogService>().ShowAlertAsync(Strings.devicedetailserror, Strings.error, Strings.ok);
                        break;
                    case Status.NoPresentation:
                        IsBusy = false;
                        await Mvx.Resolve<IDialogService>().ShowAlertAsync(Strings.nopresentationerror, Strings.error, Strings.ok);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private async Task<Status> LoadDeviceData()
        {
            bool IsSuccess = false;
            try
            {
                IsSuccess = await BSUtility.Instance.GetDeviceRemoteData();

                if (IsSuccess)
                {
                    if (String.IsNullOrEmpty(Constants.ActivePresentation))
                    {
                        return Status.NoPresentation;
                    }
                }
                // Device Actions in Constants.BSUdpAction is filled
                // Remove Default actions from Db
                // Add the new device default action into the db
                //DBHandler.Instance.RemoveDefaultActionsFromDB();

                if (Constants.BSActionList != null)
                {
                    foreach (var item in Constants.BSActionList)
                    {
                        BSUdpAction action = DBHandler.Instance.GetActionInstanceByNameLabel( item.DataUDP);

                        if(action == null){
                            item.Sno = 1000;
                            item.PresentationLabel = Constants.ActivePresentation;
                            DBHandler.Instance.InsertAction(item);
                        }
                    }

                    string[] commands = new string[Constants.BSActionList.Count];


                    for (int i = 0; i < Constants.BSActionList.Count; i++){
                        commands[i] = Constants.BSActionList[i].DataUDP;
                    }

                    DBHandler.Instance.RemoveDefaultActionsFromDB(commands);
                    Constants.BSActionList = DBHandler.Instance.GetActionsfromDBforPresentation(Constants.ActivePresentation, false);

                }

                if (IsSuccess)
                {
                    await BSUtility.Instance.InitializeSocketListening();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            if (IsSuccess)
            {
                return Status.Success;
            }
            else
            {
                return Status.Failure;
            }
        }
    }
}
