using System;
using BrightSign.Core.Models;
using MvvmCross.Plugins.Messenger;

namespace BrightSign.Core.Utility.Messages
{
    public class ListItemCreatedMessage : MvxMessage
    {
        public ListViewItem listItem;
        public ListItemCreatedMessage(object sender, ListViewItem item)
            : base(sender)
        {
            listItem = item;
        }
    }
}
