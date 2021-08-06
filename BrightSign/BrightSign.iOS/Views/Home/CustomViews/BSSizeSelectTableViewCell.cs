using System;

using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace BrightSign.iOS.Views.Home.CustomViews
{
    public partial class BSSizeSelectTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("BSSizeSelectTableViewCell");
        public static readonly UINib Nib;

        static BSSizeSelectTableViewCell()
        {
            Nib = UINib.FromName("BSSizeSelectTableViewCell", NSBundle.MainBundle);
        }

        protected BSSizeSelectTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
