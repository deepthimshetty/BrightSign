using System;
using BrightSign.Core.Models;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace BrightSign.iOS.Views.CustomViews
{
    public partial class ManageActionTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ManageActionTableViewCell");
        public static readonly UINib Nib;

        static ManageActionTableViewCell()
        {
            Nib = UINib.FromName("ManageActionTableViewCell", NSBundle.MainBundle);
        }

        protected ManageActionTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ManageActionTableViewCell, BSUdpAction>();
                set.Bind(label).To(item => item.Label);
                set.Bind(udpLabel).To(item => item.DataUDP);
                set.Apply();
            });

            this.SelectionStyle = UITableViewCellSelectionStyle.None;
        }
    }
}
