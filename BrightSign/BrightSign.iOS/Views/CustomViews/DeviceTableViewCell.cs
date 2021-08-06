using System;
using BrightSign.Core.Models;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace BrightSign.iOS.Views.CustomViews
{
    public partial class DeviceTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("DeviceTableViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// MvxImageViewLoader for UIImageView
        /// </summary>
        private MvxImageViewLoader loader;

        static DeviceTableViewCell()
        {
            Nib = UINib.FromName("DeviceTableViewCell", NSBundle.MainBundle);
        }

        protected DeviceTableViewCell(IntPtr handle) : base(handle)
        {
            loader = new MvxImageViewLoader(() => unitImage);

            this.SelectionStyle = UITableViewCellSelectionStyle.None;
            // Note: this .ctor should not contain any initialization logic.
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<DeviceTableViewCell, BSDevice>();
                set.Bind(loader).To(item => item.Image).WithConversion("ImageName", 1);
                set.Bind(unitName).To(item => item.Name);
                set.Bind(ipAddress).To(item => item.IpAddress);
                set.Bind(checkImage).For(o => o.Hidden).To(item => item.IsSelected).WithConversion("Inverse");
                set.Apply();
            });
        }
    }
}
