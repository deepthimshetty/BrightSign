using System;
using BrightSign.Core.Utility;
using MvvmCross.ViewModels;
//using MvvmCross.Platform;
using System.Reflection;

namespace BrightSign.Core.Models
{
    public class BSDeviceTemp : MvxViewModel
    {
        private string _IpAddress;
        public string IpAddress
        {
            get
            {
                return _IpAddress;
            }
            set
            {
                _IpAddress = value;
                RaisePropertyChanged(() => IpAddress);
            }
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public string UniqueId { get; set; }
        public string UdpMulticastAddress { get; set; }
        string _SerialNumber;
        public string SerialNumber
        {
            get
            {
                return _SerialNumber;
            }
            set
            {
                _SerialNumber = value;
                Image = BSUtility.Instance.GetImageString(value);
            }
        }
        public string Description { get; set; }
        public string NamingMethod { get; set; }
        public string Functionality { get; set; }

        public string AutorunVersion { get; set; }
        public string FirmwareVersion { get; set; }
        public bool BsnActive { get; set; }
        public bool SnapshotsAvailable { get; set; }

        private string _Image;
        public string Image
        {
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;
                RaisePropertyChanged(() => Image);
            }
        }

        public bool IsOnline { get; set; }

        private bool _isDefault;

        public bool IsDefault
        {
            get { return _isDefault; }
            set
            {
                _isDefault = value;
                BsnActive = value;
                //RaisePropertyChanged(() => IsDefault);
            }
        }

        private bool _IsRightArrowVisible;
        public bool IsRightArrowVisible
        {
            get { return _IsRightArrowVisible; }
            set
            {
                _IsRightArrowVisible = value;
                //RaisePropertyChanged(() => IsRightArrowVisible);
            }
        }


        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                //RaisePropertyChanged(() => IsSelected);
            }
        }

        internal BSDevice GetBSdeviceObj()
        {
            object instance = Activator.CreateInstance(typeof(BSDevice));
            foreach (var sourceProperty in this.GetType()
                                            .GetProperties(BindingFlags.Instance |
                                                           BindingFlags.Public))
            {
                var targetProperty = typeof(BSDevice).GetProperty(sourceProperty.Name);
                // TODO: Check that the property is writable, non-static etc
                if (targetProperty != null)
                {
                    object value = sourceProperty.GetValue(this);
                    targetProperty.SetValue(instance, value);
                }
            }

            return instance as BSDevice;
        }
    }
}
