using System;
using BrightSign.iOS.Utility;
using Foundation;
using UIKit;

namespace BrightSign.iOS.Views.CustomViews
{
   
    [Register("BorderShadowView")]
    public partial class BorderShadowView : UIView
    {
        public BorderShadowView(IntPtr h) : base(h)
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
            //UIEdgeInsets shadowInsets = new UIEdgeInsets(0, 0, -1.5f, 0);
            //UIBezierPath shadowPath = UIBezierPath.FromRect(TextFieldsView.Bounds);
            //TextFieldsView.Layer.ShadowPath = shadowPath.CGPath;
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

    }
}
