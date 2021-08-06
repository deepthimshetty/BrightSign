using System;
using BrightSign.Core.Models;
using MvvmCross.Plugins.Messenger;

namespace BrightSign.Core.Utility.Messages
{
    public class AddDeviceRefreshMessage : MvxMessage
    {
        public BSDevice device;
        public AddDeviceRefreshMessage(object sender, BSDevice obj) : base(sender)
        {
            device = obj;
        }
    }
}
