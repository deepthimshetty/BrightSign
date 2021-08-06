using System;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.iOS.Utility;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace BrightSign.iOS
{
    public partial class ActionViewCell : MvxCollectionViewCell
    {
        public static readonly NSString Key = new NSString("ActionViewCell");
        public static readonly UINib Nib;

        static ActionViewCell()
        {
            Nib = UINib.FromName("ActionViewCell", NSBundle.MainBundle);
        }

        protected ActionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ActionViewCell, BSUdpAction>();
                set.Bind(titleLbl).To(item => item.Label);
                set.Apply();
                //actionImage.Image = actionImage.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                //actionImage.TintColor = UIColorUtility.FromHex(ColorConstants.OrangeColor);

            });
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            UIView.Animate(0.5, () =>
            {
                shadowView.BackgroundColor = UIColor.LightGray;
            });
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            UIView.Animate(0.5, () =>
            {
                shadowView.BackgroundColor = UIColor.White;
            });
        }
    }
}
