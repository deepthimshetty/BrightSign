using System;
using MvvmCross.Plugins.Messenger;

namespace BrightSign.Core.Utility.Messages
{
    public class LoadButtonsMessage : MvxMessage
    {
        public DeviceStatus deviceStatus;
        public bool RefreshData = false;
        public LoadButtonsMessage(object sender, DeviceStatus devicestatus, bool refreshdata = false) : base(sender)
        {
            deviceStatus = devicestatus;
            RefreshData = refreshdata;
        }
    }
}
