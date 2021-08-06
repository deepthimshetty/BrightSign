using System;
using MvvmCross.Core.ViewModels;

namespace BrightSign.Core.Models
{
    public class ActiveListViewItem: MvxViewModel
    {

        private string _deviceName;
        public string DeviceName
        {
            get
            { return _deviceName; }
            set
            {
                _deviceName = value;
                RaisePropertyChanged(() => DeviceName);
            }
        }

        private string _ipAddress;
        public string IPAddress
        {
            get
            { return _ipAddress; }
            set
            {
                _ipAddress = value;
                RaisePropertyChanged(() => IPAddress);
            }
        }
        public ActiveListViewItem()
        {
        }
    }
}
