using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Database;
using BrightSign.Core.Utility.Messages;
using BrightSign.Core.ViewModels.AddDevice;
using MvvmCross.ViewModels;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross;

namespace BrightSign.Core.ViewModels.Settings
{
    public class ManageBSUnitsViewModel : BaseViewModel
    {
        MvxSubscriptionToken AddDeviceToken;
        BSDevice bsdeviceAdd;

        public ManageBSUnitsViewModel(IMvxMessenger messenger) : base(messenger)
        {

            ViewTitle = "Manage Active BrightSigns";
            BSUnitsItemSource = new ObservableCollection<BSDevice>(Constants.FullDevices);
            _navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
            //BSUnitsItemSource = new ObservableCollection<BSDevice>();
            //GetDevices();
        }

        private ObservableCollection<BSDevice> _bsUnitsItemSource;
        public ObservableCollection<BSDevice> BSUnitsItemSource
        {
            get { return _bsUnitsItemSource; }
            set
            {
                _bsUnitsItemSource = value;
                RaisePropertyChanged(() => BSUnitsItemSource);
            }

        }

        private void GetDevices()
        {
            _bsUnitsItemSource.Add(new BSDevice
            {
                Name = "AMZ_TV",
                IpAddress = "192.168.1.11",
                IsDefault = true
            });
            _bsUnitsItemSource.Add(new BSDevice
            {
                Name = "AMZ_TV",
                IpAddress = "192.168.1.11"
            });
            _bsUnitsItemSource.Add(new BSDevice
            {
                Name = "AMZ_TV_K",
                IpAddress = "193.168.2.12"
            });
            _bsUnitsItemSource.Add(new BSDevice
            {
                Name = "AMZ_TV-Innominds",
                IpAddress = "194.168.3.13"
            });
        }

        /// <summary>
        /// Gets the list selector command.
        /// </summary>
        /// <value>The list selector command.</value>
        public IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand<BSDevice>(ExecuteListSelectorCommand); }
        }

        public MvxCommand CancelCommand
        {
            get { return new MvxCommand(() => ExecuteCancelCommand()); }
        }

        public MvxCommand SaveCommand
        {
            get { return new MvxCommand(() => ExecuteSaveCommand()); }
        }

        private void ExecuteCancelCommand()
        {
            //Close(this);
            _navigationService.Close(this);
        }

        private void ExecuteSaveCommand()
        {
            //Close(this);
            _navigationService.Close(this);
            if (bsdeviceAdd != null && !string.IsNullOrEmpty(bsdeviceAdd.Name))
            {
                Constants.FullDevices.Add(bsdeviceAdd);
                try
                {
                    DBHandler.Instance.InsertorReplaceDevice(bsdeviceAdd);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                Messenger.Publish(new ManageDeviceRefreshMessage(this, bsdeviceAdd));
            }

        }

        private void ExecuteListSelectorCommand(BSDevice item)
        {

        }

        public IMvxCommand DeleteUnitCommand
        {
            get { return new MvxCommand<int>(ExecuteDeleteUnitCommand); }

        }

        private void ExecuteDeleteUnitCommand(int unitPosition)
        {
            bsdeviceAdd = null;
            var item = BSUnitsItemSource[unitPosition];
            if (item.IpAddress == Constants.ActiveDevice.IpAddress)
            {
                Constants.ScannedDevices.Remove(Constants.ScannedDevices.FirstOrDefault(o => o.IpAddress == item.IpAddress));
                Constants.FullDevices.Remove(Constants.ScannedDevices.FirstOrDefault(o => o.IpAddress == item.IpAddress));
                if (Constants.ScannedDevices.Count > 0)
                {
                    Constants.ScannedDevices[0].IsOnline = true;
                    Constants.ScannedDevices[0].IsDefault = true;
                    Constants.ActiveDevice = Constants.ScannedDevices[0];
                }

            }
            else
            {
                BSUnitsItemSource.RemoveAt(unitPosition);
            }


            DBHandler.Instance.RemoveDevice(item);
            Messenger.Publish(new ManageDeviceRefreshMessage(this, bsdeviceAdd, true));

            //RaisePropertyChanged(() => BSUnitsItemSource);
        }

        public IMvxCommand AddUnitCommand
        {
            get { return new MvxCommand(() => ExecuteAddUnitCommand()); }
        }

        private async void ExecuteAddUnitCommand()
        {
            //ShowViewModel<AddDeviceViewModel>();
            await _navigationService.Navigate<AddDeviceViewModel>();
            if (AddDeviceToken == null)
            {
                AddDeviceToken = Messenger.Subscribe<AddDeviceRefreshMessage>(OnAddDeviceResponse);
            }
        }

        private void OnAddDeviceResponse(AddDeviceRefreshMessage obj)
        {
            if (obj.device != null)
            {
                BSUnitsItemSource.Add(obj.device);
                bsdeviceAdd = obj.device;
            }
        }
    }
}
