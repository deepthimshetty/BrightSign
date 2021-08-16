using System;
using MvvmCross.ViewModels;
using SQLite;

namespace BrightSign.Core.Models
{
    public class BSDevice //: MvxViewModel
    {

        [PrimaryKey]
        public string IpAddress { get; set; }

        public string Name { get; set; }
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
            }
        }



        public string Description { get; set; }
        public string NamingMethod { get; set; }
        public string Functionality { get; set; }

        public string AutorunVersion { get; set; }
        public string FirmwareVersion { get; set; }
        public bool BsnActive { get; set; }
        public bool SnapshotsAvailable { get; set; }

        public string Image { get; set; }

        public bool IsOnline { get; set; }

        private bool _isDefault;

        [Ignore]
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
        [Ignore]
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
        [Ignore]
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                //RaisePropertyChanged(() => IsSelected);
            }
        }


    }
}
