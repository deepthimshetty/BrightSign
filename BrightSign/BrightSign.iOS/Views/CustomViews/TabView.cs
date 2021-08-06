using System;
using BrightSign.Core.Models;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using ObjCRuntime;
using UIKit;

namespace BrightSign.iOS.Views.Home.CustomViews
{
    [Register("TabView")]
    public partial class TabView : MvxView
    {
        public bool isViewSet = false;
        public TabView()
        {

        }

        public TabView(IntPtr h) : base(h)
        {

        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (isViewSet)
            {
                isViewSet = false;
                var views = NSBundle.MainBundle.LoadNib("TabView", this, null);
                var view = Runtime.GetNSObject(views.ValueAt(0)) as UIView;
                view.Frame = new CoreGraphics.CGRect(0, 0, Frame.Width, Frame.Height);
                AddSubview(view);

                this.CreateBinding(tabTitle).To((TabItem item) => item.Name).Apply();
                this.CreateBinding(tabLine).For(o => o.Hidden).To((TabItem item) => item.IsSelected).WithConversion("Inverse").Apply();
            }
        }

        internal void SetContext(TabItem tabitem, bool isSet = true)
        {
            this.DataContext = tabitem;
            this.isViewSet = isSet;
        }

        public void SetContext(IMvxBindingContext context, bool isSet = true)
        {
            this.BindingContext = context;
            this.isViewSet = isSet;
        }
    }
}
