using System;
using BrightSign.iOS.Utility;
using Foundation;
using UIKit;

namespace BrightSign.iOS.Views.CustomViews
{
    [Register("BorderButton")]
    public class BorderButton : UIButton
    {
        public BorderButton(IntPtr h) : base(h)
        {
            Layer.CornerRadius = 5;
            Layer.BorderColor = UIColorUtility.FromHex("F66A3B").CGColor;
            Layer.BorderWidth = 1;
        }
    }
}
