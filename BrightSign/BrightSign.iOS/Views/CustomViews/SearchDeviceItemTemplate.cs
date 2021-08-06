using System;
using BrightSign.Core.Models;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using ObjCRuntime;
using UIKit;

namespace BrightSign.iOS.Views.CustomViews
{
    [Register("SearchDeviceItemTemplate")]
    public partial class SearchDeviceItemTemplate : MvxView
    {
        /// <summary>
        /// MvxImageViewLoader for UIImageView
        /// </summary>
        private MvxImageViewLoader loader;

        public bool isViewSet = false;
        public SearchDeviceItemTemplate()
        {

        }

        public SearchDeviceItemTemplate(IntPtr h) : base(h)
        {

        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (isViewSet)
            {
                isViewSet = false;
                loader = new MvxImageViewLoader(() => unitImage);

                var views = NSBundle.MainBundle.LoadNib("SearchDeviceTemplate", this, null);
                var view = Runtime.GetNSObject(views.ValueAt(0)) as UIView;
                view.Frame = new CoreGraphics.CGRect(0, 0, Frame.Width, Frame.Height);
                AddSubview(view);

                shadowView.SetSelectedBorder();

                this.CreateBinding(loader).To((BSDeviceTemp device) => device.Image).WithConversion("ImageName", 1).Apply();
                this.CreateBinding(unitName).To((BSDeviceTemp item) => item.Name).Apply();
                this.CreateBinding(ipAddress).To((BSDeviceTemp item) => item.IpAddress).Apply();

            }
        }

        internal void SetContext(BSDeviceTemp device, bool isSet = true)
        {
            this.DataContext = device;
            this.isViewSet = isSet;
        }

        public void SetContext(IMvxBindingContext context, bool isSet = true)
        {
            this.BindingContext = context;
            this.isViewSet = isSet;
        }
    }
}
