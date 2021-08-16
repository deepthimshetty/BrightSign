
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Messages;
using BrightSign.Core.ViewModels;
using BrightSign.Droid.Utility.Interface;
using BrightSign.Droid.Views.Fragments;
using BrightSign.Localization;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Plugin.CurrentActivity;
using Android.Content.PM;
using Android.Util;
using Plugin.Permissions;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;

namespace BrightSign.Droid.Views
{

    [Activity(Theme = "@style/MyTheme",
              MainLauncher = false, Icon = "@mipmap/ic_launcher", LaunchMode = Android.Content.PM.LaunchMode.SingleTop, ParentActivity = typeof(MainActivity))]
    public class MainActivity : MvxAppCompatActivity<MainViewModel>
    {
        Button btnCancel, btnSave, btnBack;
        LinearLayout imgrotateLayout;
        ImageButton btnRefresh, btnClose, btnHome;
        TextView mTitleTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {


            base.OnCreate(savedInstanceState);
            AppCenter.Start("c68ff2cb-82b8-4e31-b687-69f2c7650263",
                   typeof(Analytics), typeof(Crashes));
            if (!isTablet())
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            }

            try
            {
                SetContentView(Resource.Layout.MainView);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            
            UserDialogs.Init(this);

            
            
            // Create your application here

            CrossCurrentActivity.Current.Activity = this;

            try
            {
                ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            if (savedInstanceState == null)
            {

                var _connectToDeviceViewModel = ViewModel as MainViewModel;
                var transaction = SupportFragmentManager.BeginTransaction();
                MvxFragment frag = new HomeFragment();
                frag.ViewModel = _connectToDeviceViewModel;
                transaction.Replace(Resource.Id.content_frame, frag, frag.GetType().Name);
                //transaction.AddToBackStack(frag.GetType().Name);
                transaction.CommitAllowingStateLoss();
            }

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                View customView = LayoutInflater.Inflate(Resource.Layout.CustomActionBar, null);
                toolbar.AddView(customView);
            }

            mTitleTextView = FindViewById<TextView>(Resource.Id.TitleText);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            btnSave = FindViewById<Button>(Resource.Id.btnSave);

            btnCancel.Visibility = ViewStates.Invisible;
            btnSave.Visibility = ViewStates.Invisible;

            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

        }
        public override void OnBackPressed()
        {
            var currentFrag = SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as MvxFragment;
            if (currentFrag != null && currentFrag.Tag.ToString().Contains("HomeFragment"))
            {
                new DialogService().ShowAlertWithTwoButtons(Strings.exit_the_application, "", Strings.yes, Strings.no, () => OnCloseClick(), null);
                return;
            }

            base.OnBackPressed();
        }

        bool OnCloseClick()
        {
            FinishAffinity();
            Process.KillProcess(Process.MyPid());
            System.Environment.Exit(1);
            return true;
        }


        //Removing Event Handlers
        protected override void OnDestroy()
        {
            base.OnDestroy();
            try
            {
                ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public override View OnCreateView(View parent, string name, Context context, Android.Util.IAttributeSet attrs)
        {
            return base.OnCreateView(parent, name, context, attrs);
        }

        void BtnSettings_Click(object sender, EventArgs e)
        {
            ViewModel.SettingsCommand.Execute();
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ViewTitle"))
            {
                if (mTitleTextView == null)
                    mTitleTextView = FindViewById<TextView>(Resource.Id.TitleText);
                mTitleTextView.Text = ViewModel.ViewTitle;
            }

        }

        public void HideAllIcons()
        {
            if (btnBack == null)
                btnBack = FindViewById<Button>(Resource.Id.btnBack);
            if (btnCancel == null)
                btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            if (btnSave == null)
                btnSave = FindViewById<Button>(Resource.Id.btnSave);
            if (btnRefresh == null)
                btnRefresh = FindViewById<ImageButton>(Resource.Id.refreshImg);
            if (btnClose == null)
                btnClose = FindViewById<ImageButton>(Resource.Id.closeBtn);
            if (btnHome == null)
                btnHome = FindViewById<ImageButton>(Resource.Id.btnHome);


            var btnShare = FindViewById<ImageButton>(Resource.Id.btnShare);
            btnShare.Visibility = ViewStates.Gone;

            imgrotateLayout = FindViewById<LinearLayout>(Resource.Id.rotateLayout);
            imgrotateLayout.Visibility = ViewStates.Gone;



            btnBack.Visibility = btnCancel.Visibility = btnRefresh.Visibility =
                btnClose.Visibility = btnHome.Visibility = ViewStates.Invisible;
            btnSave.Visibility = ViewStates.Invisible;

        }

        public void ShowActionsMenu()
        {
            btnHome.Visibility = ViewStates.Visible;
            if (btnSave == null)
                btnSave = FindViewById<Button>(Resource.Id.btnSave);
            btnSave.Text = "Edit";
            btnSave.Visibility = ViewStates.Visible;
            var refreshImg = FindViewById<ImageButton>(Resource.Id.refreshImg);
            if (refreshImg != null)
            {
                refreshImg.Visibility = ViewStates.Invisible;
            }

        }

        public bool isTablet()
        {

            DisplayMetrics displayMetrics = new DisplayMetrics();
            Activity activity = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
            activity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);

            float yInches = displayMetrics.HeightPixels / displayMetrics.Ydpi;
            float xInches = displayMetrics.WidthPixels / displayMetrics.Xdpi;

            double diagnolInches = Math.Sqrt(xInches * xInches + yInches * yInches);
            if (diagnolInches >= 6.5)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void ShowToolbarActions(int position)
        {
            //Hide all icons first and show as per tab selection
            HideAllIcons();

            switch (position)
            {
                case 0:
                    {
                        Constants.CurrentTab = TitleType.Variables;
                        btnHome.Visibility = ViewStates.Visible;
                        btnRefresh.Visibility = ViewStates.Visible;
                        ViewModel.ViewTitle = TitleType.Variables.ToString();
                    }
                    break;
                case 1:
                    {
                        Constants.CurrentTab = TitleType.Actions;
                        ShowActionsMenu();
                        ViewModel.ViewTitle = TitleType.Actions.ToString();

                    }
                    break;
                case 2:
                    {
                        Constants.CurrentTab = TitleType.Diagnostics;
                        btnHome.Visibility = ViewStates.Visible;
                        btnRefresh.Visibility = ViewStates.Visible;
                        Constants.OnDiagnosticsTabSelected();
                        ViewModel.ViewTitle = TitleType.Diagnostics.ToString();
                    }
                    break;
                case 3:
                    {
                        Constants.CurrentTab = TitleType.Gallery;
                        btnHome.Visibility = ViewStates.Visible;
                        imgrotateLayout.Visibility = ViewStates.Visible;
                        ViewModel.ViewTitle = TitleType.Gallery.ToString();
                    }
                    break;
                case 4:
                    {
                        Constants.CurrentTab = TitleType.Settings;
                        btnHome.Visibility = ViewStates.Visible;
                        imgrotateLayout.Visibility = ViewStates.Gone;
                        ViewModel.ViewTitle = TitleType.Settings.ToString();
                    }
                    break;
                default:
                    {
                        Constants.CurrentTab = TitleType.Variables;
                        HideAllIcons();
                        btnHome.Visibility = ViewStates.Visible;
                        btnRefresh.Visibility = ViewStates.Visible;
                    }
                    break;
            }

        }


        public void ShowToolbarActionsForSelectBS(int position)
        {
            switch (position)
            {
                case 0:
                    {
                        ViewModel.ViewTitle = "Select BrightSign";
                    }
                    break;
                case 1:
                    {
                        ViewModel.ViewTitle = "Select BrightSign";

                    }
                    break;

                default:
                    {

                    }
                    break;
            }

        }
        public void HideKeybord()
        {
            View view = CurrentFocus;
            if (view != null)
            {
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.None);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}
