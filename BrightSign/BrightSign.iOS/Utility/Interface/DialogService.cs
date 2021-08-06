using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using BrightSign.Core.Utility.Interface;
using UIKit;

namespace BrightSign.iOS.Utility.Interface
{
    public class DialogService : IDialogService
    {
        UIActivityIndicatorView activityIndicatorView;
        public void HideLoading()
        {
            UserDialogs.Instance.HideLoading();
            //activityIndicatorView.RemoveFromSuperview();
        }

        public void ShowActionSheetAlert()
        {


        }

        public void ShowActionSheetAlert(string title, string cancelstr, string item1, string item2, Action item1Clicked = null, Action item2Clikced = null)
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
                                             .SetTitle(title)
                                             .SetCancel(cancelstr, () => { })
                                             .Add(item1, () =>
                                             {
                                                 item1Clicked();
                                             }).Add(item2, () =>
                                             {
                                                 item2Clikced();
                                             })
                                             );
        }

        public Task ShowAlertAsync(string message, string title, string buttonText)
        {
            return Task.Run(() =>
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    new UIAlertView(title, message, null, buttonText).Show();
                }));
        }

        public void ShowAlertWithTwoButtons(string message, string title = null, string okButtonText = "OK", string cancelButtonText = "Cancel", Action okClicked = null, Action cancelClikced = null)
        {
            UIAlertView alert = new UIAlertView(title, message, null, cancelButtonText, null);
            alert.AddButton(okButtonText);
            alert.Clicked += (object sender, UIButtonEventArgs e) =>
            {
                switch (e.ButtonIndex)
                {
                    case 0:
                        if (cancelClikced != null)
                        {
                            cancelClikced();
                        }
                        break;
                    case 1:
                        if (okClicked != null)
                        {
                            okClicked();
                        }
                        break;
                    default:
                        break;
                }
            };
            alert.Show();

        }

        public void ShowLoading(string loadingText = null)
        {
            UserDialogs.Instance.ShowLoading();

            //activityIndicatorView = new UIActivityIndicatorView();
            //activityIndicatorView.BackgroundColor = UIColor.Black.ColorWithAlpha(0.6f);
            //activityIndicatorView.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.WhiteLarge;
            //activityIndicatorView.Layer.CornerRadius = 10;
            //activityIndicatorView.Opaque = false;
            //activityIndicatorView.Frame = new CoreGraphics.CGRect(0, 0, 100, 100);
            //activityIndicatorView.Center = UIApplication.SharedApplication.Windows[0].Center;
            //activityIndicatorView.StartAnimating();
            //UIApplication.SharedApplication.Windows[0].AddSubview(activityIndicatorView);
        }
    }
}
