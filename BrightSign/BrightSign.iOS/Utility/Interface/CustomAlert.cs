using System;
using BrightSign.Core.Utility.Interface;
using BrightSign.iOS.Views.CustomViews;
using ObjCRuntime;
using UIKit;

namespace BrightSign.iOS.Utility.Interface
{
    public class CustomAlert : ICustomAlert
    {
        CustomAlertView alertView;

        public void ShowCustomAlert(bool isSuccess, string title, string message)
        {
            var views = Foundation.NSBundle.MainBundle.LoadNib("CustomAlertView", null, null);
            alertView = Runtime.GetNSObject(views.ValueAt(0)) as CustomAlertView;
            alertView.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
            alertView.SetValues(isSuccess, title, message);
            UIApplication.SharedApplication.Windows[0].AddSubview(alertView);
        }
    }
}
