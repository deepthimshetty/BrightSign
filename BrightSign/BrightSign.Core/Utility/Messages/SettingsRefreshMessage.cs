using System;
using BrightSign.Core.Models;
using MvvmCross.Plugin.Messenger;

namespace BrightSign.Core.Utility.Messages
{
    public class SettingsRefreshMessage : MvxMessage
    {
        public bool IsRefresh;
        public BSDevice device;
        public SettingsRefreshMessage(object sender, BSDevice obj, bool isrefreshlist = false) : base(sender)
        {
            IsRefresh = isrefreshlist;
            device = obj;
        }
    }
}
