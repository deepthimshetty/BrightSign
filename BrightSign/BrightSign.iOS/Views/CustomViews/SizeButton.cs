using System;
using BrightSign.Core.Models;
using BrightSign.iOS.Utility;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using ObjCRuntime;
using UIKit;

namespace BrightSign.iOS.Views.CustomViews
{
    [Register("SizeButton")]
    public partial class SizeButton : MvxView
    {
        public bool IsViewSet = false;
        private MvxImageViewLoader loader;

        public SizeButton(IntPtr h) : base(h)
        {
            Layer.ShadowRadius = 1.0f;
            Layer.ShadowColor = UIColorUtility.FromHex("#E9EEF3").CGColor;
            Layer.ShadowOffset = new CoreGraphics.CGSize(0.5f, 0.5f);
            Layer.ShadowOpacity = 0.9f;
            Layer.MasksToBounds = false;
            Layer.CornerRadius = 5;
            Layer.BorderColor = UIColorUtility.FromHex("#F76334").CGColor;
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (IsViewSet)
            {
                IsViewSet = false;
                loader = new MvxImageViewLoader(() => buttonImage);

                var views = NSBundle.MainBundle.LoadNib("SizeButtonView", this, null);
                var view = Runtime.GetNSObject(views.ValueAt(0)) as UIView;
                view.Frame = new CoreGraphics.CGRect(0, 0, Frame.Width, Frame.Height);
                AddSubview(view);


                this.CreateBinding(loader).To((ButtonSizeItem device) => device.Image).WithConversion("ImageName", 1).Apply();
                this.CreateBinding(buttonText).To((ButtonSizeItem item) => item.ButtonText).Apply();
                //this.CreateBinding(Layer).For(o => o.BorderWidth).To((ButtonSizeItem item) => item.IsSelected).WithConversion("Border").Apply();


            }
        }

        internal void SetContext(ButtonSizeItem device, bool isSet = true)
        {
            this.DataContext = device;
            if (device.IsSelected)
            {
                Layer.BorderWidth = 2;
            }
            (this.DataContext as ButtonSizeItem).PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "IsSelected")
                {
                    if ((this.DataContext as ButtonSizeItem).IsSelected)
                    {
                        Layer.BorderWidth = 2;
                    }
                    else
                    {
                        Layer.BorderWidth = 0;
                    }
                }
            };
            this.IsViewSet = isSet;
        }


    }

}
