using System;
using BrightSign.Core.Models;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using ObjCRuntime;
using UIKit;

namespace BrightSign.iOS.Views.CustomViews
{
    [Register("DeviceInfo")]
    public partial class DeviceInfo : MvxView
    {
        /// <summary>
        /// MvxImageViewLoader for UIImageView
        /// </summary>
        private MvxImageViewLoader loader;

        public bool ShowButton;

        public bool isViewSet = false;

        public Func<object, object, object> SelectDeviceClicked { get; internal set; }


        public DeviceInfo()
        {

        }

        public DeviceInfo(IntPtr h) : base(h)
        {

        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (isViewSet)
            {
                isViewSet = false;
                loader = new MvxImageViewLoader(() => unitImage);

                var views = NSBundle.MainBundle.LoadNib("DeviceInfoView", this, null);
                var view = Runtime.GetNSObject(views.ValueAt(0)) as UIView;
                view.Frame = new CoreGraphics.CGRect(0, 0, Frame.Width, Frame.Height);
                AddSubview(view);

                this.CreateBinding(loader).To((BSDevice device) => device.Image).WithConversion("ImageName", 1).Apply();
                this.CreateBinding(unitName).To((BSDevice item) => item.Name).Apply();
                this.CreateBinding(ipAddress).To((BSDevice item) => item.IpAddress).Apply();
                //this.CreateBinding(rightButton).For(o => o.Hidden).To((BSDevice item) => item.IsRightArrowVisible).WithConversion("Inverse").Apply();
                rightButton.TouchUpInside += (sender, e) =>
                {
                    if (SelectDeviceClicked != null)
                    {
                        SelectDeviceClicked(sender, e);
                    }
                };
                if (!ShowButton)
                {
                    rightButton.Hidden = true;

                }
                //this.CreateBinding(tabTitle).To((TabItem item) => item.Name).Apply();
                //this.CreateBinding(tabLine).For(o => o.Hidden).To((TabItem item) => item.IsSelected).WithConversion("Inverse").Apply();
            }
        }

        internal void SetContext(BSDevice device, bool isSet = true, bool showButton = true)
        {
            if (device != null)
            { // Null check
                this.DataContext = device;
                this.isViewSet = isSet;
            }

            ShowButton = showButton;
        }

        public void SetContext(IMvxBindingContext context, bool isSet = true)
        {
            this.BindingContext = context;
            this.isViewSet = isSet;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            if (gradientView != null)
            {
                gradientView.SetNeedsDisplay();
            }
        }

    }
}
