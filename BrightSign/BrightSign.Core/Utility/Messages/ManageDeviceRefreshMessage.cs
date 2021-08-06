using System;
using BrightSign.Core.Models;
using MvvmCross.Plugins.Messenger;

namespace BrightSign.Core.Utility.Messages
{

    public class ManageDeviceRefreshMessage : MvxMessage
    {
        public bool IsRefresh;
        public BSDevice device;
        public ManageDeviceRefreshMessage(object sender, BSDevice obj, bool isrefreshlist = false) : base(sender)
        {
            IsRefresh = isrefreshlist;
            device = obj;
        }
    }
}
