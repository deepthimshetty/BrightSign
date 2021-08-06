using System;
using BrightSign.Core.Models;
using MvvmCross.Plugins.Messenger;

namespace BrightSign.Core.Utility.Messages
{
    public class AddActionMessage : MvxMessage
    {
        public BSUdpAction actionObj;
        public AddActionMessage(object sender, BSUdpAction obj) : base(sender)
        {
            actionObj = obj;
        }
    }
}
