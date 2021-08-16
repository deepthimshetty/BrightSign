using System;
using BrightSign.Core.Models;
using MvvmCross.Plugin.Messenger;

namespace BrightSign.Core.Utility.Messages
{
    public class DashboardRefreshMessage : MvxMessage
    {
        public bool IsRefresh;
        public BSDevice device;
        public DashboardRefreshMessage(object sender, BSDevice obj, bool isrefreshlist = false) : base(sender)
        {
            IsRefresh = isrefreshlist;
            device = obj;
        }

    }
}
