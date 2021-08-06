using System;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using BrightSign.Core.Utility.Interface;
using Plugin.CurrentActivity;

namespace BrightSign.Droid.Utility.Interface
{
    public class CustomAlert : ICustomAlert
    {
        private AlertDialog dialog;
        private AlertDialog customDialog;

        public CustomAlert()
        {
        }

        public void ShowCustomAlert(bool isSuccess, string title, string message)
        {

            LayoutInflater inflater = (LayoutInflater)CrossCurrentActivity.Current.Activity.GetSystemService(Context.LayoutInflaterService);

            View view = inflater.Inflate(Resource.Layout.popup, null);

            //Set Title,message, ok button

            Button okButton = (Button)view.FindViewById(Resource.Id.okBtn);
            view.FindViewById<TextView>(Resource.Id.title).Text = title;
            view.FindViewById<TextView>(Resource.Id.message).Text = message;
            //plotViewType = (PlotView)view.FindViewById(Resource.Id.plot);
            //plotViewType.Model = plotView;

            try
            {
                Application.SynchronizationContext.Post(ignored =>
                {
                    if (CrossCurrentActivity.Current.Activity == null || CrossCurrentActivity.Current.Activity.IsFinishing)
                    {
                        return;
                    }

                    CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
                    {
                        customDialog = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity)
                                                .SetView(view)
                                                .Show();
                        //Check
                        WindowManagerLayoutParams lp = new WindowManagerLayoutParams();
                        Window window = customDialog.Window;
                        lp.CopyFrom(window.Attributes);
                        //This makes the dialog take up the full width
                        lp.Width = WindowManagerLayoutParams.MatchParent;
                        lp.Height = WindowManagerLayoutParams.WrapContent;
                        window.Attributes = lp;
                        //stop checking
                    });

                }, null);

                okButton.Click += OnBackPressed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Alert exception while showing" + ex.Message);
            }


        }

        void OnBackPressed(object sender, EventArgs e)
        {
            try
            {
                if (customDialog != null && customDialog.IsShowing)
                {
                    customDialog.Dismiss();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Alert exception while close" + ex.Message);
            }
        }

        public void RemoveCustomAlert()
        {
            if (dialog != null && dialog.IsShowing)
            {
                dialog.Dismiss();
            }
        }
    }
}
