using System;
using BrightSign.Core.Utility;
using BrightSign.iOS.Utility;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace BrightSign.iOS.Views.CustomViews
{
    [Register("GradientView")]
    public class GradientView : UIView
    {
        CAGradientLayer gradientlayer = null;
        public GradientView(IntPtr h) : base(h)
        {
            try
            {
                if (gradientlayer != null)
                {
                    gradientlayer.RemoveFromSuperLayer();
                }

                gradientlayer = new CAGradientLayer();
                //gradientlayer.Frame = Bounds;
                gradientlayer.Frame = new CGRect(this.Bounds.X, this.Bounds.Y, UIScreen.MainScreen.Bounds.Width, this.Bounds.Height);
                gradientlayer.Colors = new CGColor[] { UIColorUtility.FromHex(ColorConstants.StartColor).CGColor, UIColorUtility.FromHex(ColorConstants.EndColor).CGColor };
                gradientlayer.StartPoint = new CGPoint(0.0, 0.5);
                gradientlayer.EndPoint = new CGPoint(1.0, 0.5);
                gradientlayer.Name = "GradientLayer";

                Layer.InsertSublayer(gradientlayer, 0);


            }
            catch (Exception ex)
            {

            }

        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            try
            {
                if (gradientlayer != null)
                {
                    gradientlayer.RemoveFromSuperLayer();
                }
                gradientlayer = new CAGradientLayer();
                //gradientlayer.Frame = Bounds;
                gradientlayer.Frame = new CGRect(this.Bounds.X, this.Bounds.Y, UIScreen.MainScreen.Bounds.Width, this.Bounds.Height);
                gradientlayer.Colors = new CGColor[] { UIColorUtility.FromHex(ColorConstants.StartColor).CGColor, UIColorUtility.FromHex(ColorConstants.EndColor).CGColor };
                gradientlayer.StartPoint = new CGPoint(0.0, 0.5);
                gradientlayer.EndPoint = new CGPoint(1.0, 0.5);
                gradientlayer.Name = "GradientLayer";
                Layer.InsertSublayer(gradientlayer, 0);

            }
            catch (Exception ex)
            {

            }
        }


    }
}
