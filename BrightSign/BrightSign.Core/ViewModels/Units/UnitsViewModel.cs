using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Database;
using BrightSign.Core.Utility.Interface;
using BrightSign.Core.Utility.Messages;
using BrightSign.Core.ViewModels.AddDevice;
using BrightSign.Localization;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Email;
using MvvmCross.Plugins.Messenger;
using Sockets.Plugin;

namespace BrightSign.Core.ViewModels.Units
{
    public class UnitsViewModel : BaseViewModel, IRemove
    {
        void HandleAction()
        {
        }


        public OfflineViewModel offlineViewModel;
        public ActiveViewModel activeViewModel;
        MvxSubscriptionToken AddDeviceToken;
        IDialogService dialogService;
        int SelectedTabIndex = 0;

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

        bool _AddButtonVisible = true;
        public bool AddButtonVisible
        {
            get
            {
                return _AddButtonVisible;
            }
            set
            {
                _AddButtonVisible = value;
                RaisePropertyChanged("AddButtonVisible");
            }
        }

        TabItem _firsttabItem;
        public TabItem firsttabItem
        {
            get
            {
                return _firsttabItem;
            }
            set
            {
                _firsttabItem = value;
                RaisePropertyChanged(() => firsttabItem);
            }
        }

        TabItem _secondtabItem;
        public TabItem secondtabItem
        {
            get
            {
                return _secondtabItem;
            }
            set
            {
                _secondtabItem = value;
                RaisePropertyChanged(() => secondtabItem);
            }
        }

        ObservableCollection<BSDevice> _deviceList;
        public ObservableCollection<BSDevice> deviceList
        {
            get
            {
                return _deviceList;
            }
            set
            {
                _deviceList = value;
                RaisePropertyChanged(() => deviceList);
            }
        }

        ObservableCollection<BSDevice> _ActiveDevices;
        public ObservableCollection<BSDevice> ActiveDevices
        {
            get
            {
                return _ActiveDevices;
            }
            set
            {
                _ActiveDevices = value;
                RaisePropertyChanged(() => ActiveDevices);
            }
        }

        ObservableCollection<BSDevice> _OfflineDevices;
        public ObservableCollection<BSDevice> OfflineDevices
        {
            get
            {
                return _OfflineDevices;
            }
            set
            {
                _OfflineDevices = value;
                RaisePropertyChanged(() => OfflineDevices);
            }
        }

        MvxCommand<BSDevice> _mItemClickCommand;
        /// <summary>
        /// Gets the item click command.
        /// </summary>
        /// <value>The item click command.</value>
        public ICommand ItemClickCommand
        {
            get
            {
                return this._mItemClickCommand ?? (this._mItemClickCommand = new MvxCommand<BSDevice>(async (BSDevice obj) =>
                {
                    await ItemClick(obj);

                }));
            }
        }

        public IMvxCommand RefreshCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await RefreshClick();
                });
            }
        }

        private async Task RefreshClick()
        {
            try
            {
                IsBusy = true;

                AcquireLock = true;

                await BSUtility.Instance.EnumerateAllServicesFromAllHosts();

                AcquireLock = false;


                GetData();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }

        }

        private async Task ItemClick(BSDevice obj)
        {
            if (SelectedTabIndex == 0)
            {
                foreach (var item in deviceList)
                {
                    item.IsSelected = false;
                    obj.IsDefault = false;
                }
                obj.IsSelected = true;
                obj.IsDefault = true;

                Constants.ActiveDevice = obj;

                IsBusy = true;

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
                            ShowViewModel<MainViewModel>();
                            break;
                        case Status.Failure:
                            IsBusy = false;
                            await dialogService.ShowAlertAsync(Strings.devicedetailserror, Strings.error, Strings.ok);
                            break;
                        case Status.NoPresentation:
                            IsBusy = false;
                            await dialogService.ShowAlertAsync(Strings.nopresentationerror, Strings.error, Strings.ok);
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
                        BSUdpAction action = DBHandler.Instance.GetActionInstanceByNameLabel(item.DataUDP);

                        if (action == null)
                        {
                            item.Sno = 1000;
                            item.PresentationLabel = Constants.ActivePresentation;
                            DBHandler.Instance.InsertAction(item);
                        }
                    }

                    string[] commands = new string[Constants.BSActionList.Count];


                    for (int i = 0; i < Constants.BSActionList.Count; i++)
                    {
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

        public ICommand RemoveCommand
        {
            get
            {
                return new MvxCommand<int>(RemoveClick);
            }
        }

        private void RemoveClick(int removeIndex)
        {
            var item = OfflineDevices[removeIndex];
            deviceList.RemoveAt(removeIndex);
            Constants.FullDevices.Remove(item);
            DBHandler.Instance.RemoveDevice(item);

        }

        public int Selectedtab
        {
            get
            {
                return SelectedTabIndex;
            }
        }

        private void AddDeviceClick()
        {
            //Mvx.Resolve<ICustomAlert>().ShowCustomAlert(true, "BX", "bx");

            ShowViewModel<AddDeviceViewModel>();
            if (AddDeviceToken == null)
            {
                AddDeviceToken = Messenger.Subscribe<AddDeviceRefreshMessage>(OnAddDeviceResponse);
            }
        }

        private void OnAddDeviceResponse(AddDeviceRefreshMessage obj)
        {
            if (obj.device != null)
            {
                Constants.FullDevices.Add(obj.device);
                DBHandler.Instance.InsertorReplaceDevice(obj.device);

                ActiveDevices.Add(obj.device);
            }

            //Messenger.Unsubscribe<AddDeviceRefreshMessage>(AddDeviceToken);
            //AddDeviceToken = null;

        }

        public UnitsViewModel(IMvxMessenger messenger, IDialogService _dialogservice) : base(messenger)
        {
            Constants.BSSnapshotList = new System.Collections.Generic.List<BSSnapshot>();
            dialogService = _dialogservice;
            if (Constants.UdpReceiver != null)
            {
                Constants.UdpReceiver.DisconnectAsync();
                Constants.UdpReceiver.Dispose();
                Constants.UdpReceiver = null;
            }
            activeViewModel = new ActiveViewModel(messenger);
            offlineViewModel = new OfflineViewModel(messenger);

            ViewTitle = Strings.selectbrightsign;
            firsttabItem = new TabItem()
            {
                ID = 0,
                IsSelected = true,
                Name = "Active"
            };
            secondtabItem = new TabItem()
            {
                ID = 0,
                IsSelected = false,
                Name = "Offline"
            };

            GetData();
            if (AddDeviceToken == null)
            {
                AddDeviceToken = Messenger.Subscribe<AddDeviceRefreshMessage>(OnAddDeviceResponse);
            }

            Constants.IsCredentialsRequiredforSnapshots = false;
        }

        private async Task GetData()
        {
            List<BSDevice> previousOnlineDevicesList = null;
            if (Constants.FullDevices != null && Constants.FullDevices.Count > 0)
            {
                previousOnlineDevicesList = new List<BSDevice>(Constants.FullDevices.Where(o => o.IsOnline));
            }

            //Combine the Scanned list and the full list
            try
            {
                Constants.FullDevices = DBHandler.Instance.GetDevicefromDB();
            }
            catch (Exception ex)
            {
                Constants.FullDevices = new List<BSDevice>();
            }


            if (previousOnlineDevicesList == null)
            {
                previousOnlineDevicesList = new List<BSDevice>(Constants.FullDevices.Where(o => o.IsOnline));
            }
            foreach (var item in Constants.FullDevices)
            {
                item.IsOnline = false;
            }
            List<Task<Tuple<bool, BSDevice>>> retryApiCalls = null;


            if (previousOnlineDevicesList != null)
            {
                retryApiCalls = new List<Task<Tuple<bool, BSDevice>>>();

                foreach (var item in previousOnlineDevicesList)
                {
                    var device = Constants.ScannedDevices.FirstOrDefault(o => o.IpAddress == item.IpAddress);
                    if (device == null)
                    {
                        // Do an API Call
                        retryApiCalls.Add(BSUtility.Instance.GetDeviceStatus(item.IpAddress));

                    }
                }
            }

            if (retryApiCalls != null)
            {

                try
                {
                    IsBusy = true;
                    Task.WaitAll(retryApiCalls.ToArray());
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    IsBusy = false;
                }

            }

            foreach (var item in retryApiCalls)
            {
                if (item.Result.Item1)
                {
                    Constants.ScannedDevices.Add(item.Result.Item2);
                }
            }

            if (Constants.ScannedDevices != null)
            {
                foreach (var item in Constants.ScannedDevices)
                {
                    var device = Constants.FullDevices.FirstOrDefault(o => o.IpAddress == item.IpAddress);
                    if (device != null)
                    {
                        var item_index = Constants.FullDevices.IndexOf(device);
                        Constants.FullDevices[item_index] = item;
                    }
                    else
                    {
                        Constants.FullDevices.Add(item);
                    }
                }
            }




            DBHandler.Instance.InsertorReplaceData(Constants.FullDevices);


            ActiveDevices = new ObservableCollection<BSDevice>(Constants.FullDevices.Where(o => o.IsOnline));
            OfflineDevices = new ObservableCollection<BSDevice>(Constants.FullDevices.Where(o => !o.IsOnline));

            TabChange();
        }

        public void TabChange()
        {
            if (firsttabItem.IsSelected)
            {
                SelectedTabIndex = 0;
                deviceList = ActiveDevices;
                AddButtonVisible = true;
            }
            else
            {
                SelectedTabIndex = 1;
                deviceList = OfflineDevices;
                AddButtonVisible = true;
            }
        }
    }
}
