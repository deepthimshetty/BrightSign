using System;
using MvvmCross.Plugins.Messenger;

namespace BrightSign.Core.Utility.Messages
{
    public class ActiveDeviceMessage : MvxMessage
    {
        public DeviceStatus deviceStatus;
        public bool RefreshData = false;
        public ActiveDeviceMessage(object sender, DeviceStatus devicestatus, bool refreshdata = false) : base(sender)
        {
            deviceStatus = devicestatus;
            RefreshData = refreshdata;
        }
    }
}
