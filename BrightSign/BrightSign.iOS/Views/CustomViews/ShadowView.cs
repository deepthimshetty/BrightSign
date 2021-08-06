using System;
using BrightSign.iOS.Utility;
using Foundation;
using UIKit;

namespace BrightSign.iOS.Views.CustomViews
{
    [Register("ShadowView")]
    public partial class ShadowView : UIView
    {
        public ShadowView(IntPtr h) : base(h)
        {
            CreateShadow();
        }

        private void CreateShadow()
        {
            Layer.ShadowRadius = 1.0f;
            Layer.ShadowColor = UIColor.LightGray.CGColor;
            Layer.ShadowOffset = new CoreGraphics.CGSize(0.5f, 0.5f);
            Layer.ShadowOpacity = 0.9f;
            Layer.MasksToBounds = false;
            Layer.CornerRadius = 5;
            //UIEdgeInsets shadowInsets = new UIEdgeInsets(0, 0, -1.5f, 0);
            //UIBezierPath shadowPath = UIBezierPath.FromRect(TextFieldsView.Bounds);
            //TextFieldsView.Layer.ShadowPath = shadowPath.CGPath;
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }


        internal void SetSelectedBorder()
        {
            Layer.BorderColor = UIColorUtility.FromHex("#65C941").CGColor;
            Layer.BorderWidth = 2;
        }
    }
}
