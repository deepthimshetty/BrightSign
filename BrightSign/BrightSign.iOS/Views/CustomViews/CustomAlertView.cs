using System;
using Foundation;
using MvvmCross.Binding.iOS.Views;

namespace BrightSign.iOS.Views.CustomViews
{
    [Register("CustomAlertView")]
    public partial class CustomAlertView : MvxView
    {
        public CustomAlertView(IntPtr h) : base(h)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            actionButton.TouchUpInside += delegate
            {
                this.RemoveFromSuperview();
            };
        }

        internal void SetValues(bool isSuccess, string Title, string Message)
        {
            title.Text = Title;
            message.Text = Message;
            shadowView.Layer.CornerRadius = 10;
            title.Font = UIKit.UIFont.BoldSystemFontOfSize(18);
            actionButton.Font = UIKit.UIFont.BoldSystemFontOfSize(18);
        }
    }
}
