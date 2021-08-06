using System;

using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace BrightSign.iOS.Views.Home.CustomViews
{
    public partial class BSSwitchTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("BSSwitchTableViewCell");
        public static readonly UINib Nib;
        public UISwitch RefreshSwitch;
        static BSSwitchTableViewCell()
        {
            Nib = UINib.FromName("BSSwitchTableViewCell", NSBundle.MainBundle);
        }

        protected BSSwitchTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.


        }

        [Export("awakeFromNib")]
        public void AwakeFromNib()
        {
            RefreshSwitch = refreshSwitch;
        }
    }
}
