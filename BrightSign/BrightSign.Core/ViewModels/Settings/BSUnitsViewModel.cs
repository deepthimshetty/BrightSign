using System;
using System.Collections.ObjectModel;
using System.Linq;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Messages;
using BrightSign.Core.ViewModels.Settings;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace BrightSign.Core.ViewModels
{
    public class BSUnitsViewModel : BaseViewModel
    {
        MvxSubscriptionToken ManageDeviceToken;
        public BSUnitsViewModel(IMvxMessenger messenger) : base(messenger)
        {
            ViewTitle = "Select BrightSign";

            BSUnitsItemSource = new ObservableCollection<BSDevice>(Constants.FullDevices);

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
            BSUnitsItemSource.Add(new BSDevice
            {
                Name = "AMZ_TV",
                IpAddress = "192.168.1.11",
                IsDefault = true,
                BsnActive = true
            });
            BSUnitsItemSource.Add(new BSDevice
            {
                Name = "AMZ_TV",
                IpAddress = "192.168.1.11",
                BsnActive = true
            });
            BSUnitsItemSource.Add(new BSDevice
            {
                Name = "AMZ_TV_K",
                IpAddress = "193.168.2.12",
                BsnActive = true
            });
            BSUnitsItemSource.Add(new BSDevice
            {
                Name = "AMZ_TV-Innominds",
                IpAddress = "194.168.3.13",
                BsnActive = false
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

        public MvxCommand ManageBSUnitsCommand
        {
            get { return new MvxCommand(() => ExecuteManageBSUnitsCommand()); }
        }

        public MvxCommand CancelCommand
        {
            get { return new MvxCommand(() => ExecuteCancelCommand()); }
        }

        public MvxCommand SaveCommand
        {
            get { return new MvxCommand(() => ExecuteSaveCommand()); }
        }

        private void ExecuteManageBSUnitsCommand()
        {
            ShowViewModel<ManageBSUnitsViewModel>();
            if (ManageDeviceToken == null)
            {
                ManageDeviceToken = Messenger.Subscribe<ManageDeviceRefreshMessage>(OnManageDeviceResponse);
            }
        }

        private void OnManageDeviceResponse(ManageDeviceRefreshMessage obj)
        {
            if (obj.IsRefresh && obj == null)
            {
                BSUnitsItemSource = new ObservableCollection<BSDevice>(Constants.FullDevices);
            }
            else if (obj.device != null && !string.IsNullOrEmpty(obj.device.IpAddress))
            {
                BSUnitsItemSource.Add(obj.device);
            }
        }

        private void ExecuteCancelCommand()
        {
            Close(this);
        }

        private void ExecuteSaveCommand()
        {
            var defaultUnit = BSUnitsItemSource.Where(x => x.IsDefault).FirstOrDefault();
            Close(this);
            Messenger.Publish(new SettingsRefreshMessage(this, defaultUnit, true));
        }

        private void ExecuteListSelectorCommand(BSDevice item)
        {
            var defaultUnit = BSUnitsItemSource.Where(x => x.IsDefault).FirstOrDefault();
            defaultUnit.IsDefault = false;
            item.IsDefault = true;
        }
    }
}
