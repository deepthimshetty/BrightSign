using System;
using Foundation;
using UIKit;

namespace BrightSign.iOS.Views.CustomViews
{
    [Register("RoundedButton")]
    public class RoundedButton : UIButton
    {
        public RoundedButton(IntPtr h) : base(h)
        {
            ClipsToBounds = true;
            Layer.CornerRadius = Bounds.Size.Width / 2;
            Layer.ShadowOffset = new CoreGraphics.CGSize(5f, 5f);
            Layer.ShadowColor = UIColor.Black.CGColor;
            //Layer.Opacity = 0.8F;
            Layer.ShadowRadius = 4.0F;
            Layer.MasksToBounds = false;
        }
    }
}
