using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.App;
using BrightSign.Core.Utility.Interface;
using MvvmCross;
//using MvvmCross.Platform;
//using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platforms.Android;

namespace BrightSign.Droid.Utility.Interface
{
    public class DialogService : IDialogService
    {
        protected Android.App.ProgressDialog progressDialog;

        protected Activity CurrentActivity =>
            Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public void HideLoading()
        {
            //if (progressDialog != null)
            //{
            //    if (progressDialog.IsShowing)
            //    {
            //        CurrentActivity.RunOnUiThread(() =>
            //        {
            //            progressDialog.Dismiss();
            //        });

            //    }

            //}

            UserDialogs.Instance.HideLoading();
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

        public Task ShowAlertAsync(string message,
            string title, string buttonText)
        {
            return Task.Run(() =>
            {
                UserDialogs.Instance.Alert(message, title, buttonText);
                //Alert(message, title, buttonText);
            });
        }

        public void ShowAlertWithTwoButtons(string message, string title = null, string okButtonText = "OK", string cancelButtonText = "Cancel", Action okClicked = null, Action cancelClikced = null)
        {
            AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(CurrentActivity)
                       .SetMessage(message)
                       .SetTitle(title)
                       .SetCancelable(false)
                       .SetPositiveButton(okButtonText, delegate
                       {
                           if (okClicked != null)
                           {
                               okClicked();
                           }
                       })
                       .SetNegativeButton(cancelButtonText, delegate
                       {
                           if (cancelClikced != null)
                           {
                               cancelClikced();
                           }
                       });
            try
            {
                Application.SynchronizationContext.Post(ignored =>
                {
                    if (CurrentActivity == null || CurrentActivity.IsFinishing)
                    {
                        return;
                    }
                    CurrentActivity.RunOnUiThread(() =>
                    {
                        alertDialogBuilder.Show();
                    });

                }, null);
            }

            catch (Exception ex)
            {
                Console.WriteLine(" Alert exception while showing" + ex.Message);
            }

        }

        public void ShowLoading(string loadingText = null)
        {
            //       if (progressDialog == null)
            //       {
            //           progressDialog = new Android.App.ProgressDialog(CurrentActivity);
            //           progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            //           if (!string.IsNullOrEmpty(loadingText))
            //           {
            //               progressDialog.SetMessage(loadingText);
            //           }
            //           else
            //           {
            //               progressDialog.SetMessage(Strings.please_wait);
            //           }
            //           progressDialog.SetCancelable(false);
            //           CurrentActivity.RunOnUiThread(() =>
            //           {
            //               progressDialog.Show();
            //           });
            //       }
            //       else
            //       {
            //           if (!progressDialog.IsShowing)
            //           {
            //CurrentActivity.RunOnUiThread(() =>
            //{
            //  progressDialog.Show();
            //});
            //    }
            //}

            try
            {
                UserDialogs.Instance.ShowLoading();
            }
            catch (Exception ex)
            {

            }

        }

        private void Alert(string message, string title, string okButton)
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                var builder = new AlertDialog.Builder(CurrentActivity);
                builder.SetIconAttribute
                    (Android.Resource.Attribute.AlertDialogIcon);
                builder.SetTitle(title);
                builder.SetMessage(message);
                builder.SetPositiveButton(okButton, delegate { });
                builder.Create().Show();
            }, null);
        }
    }
}
