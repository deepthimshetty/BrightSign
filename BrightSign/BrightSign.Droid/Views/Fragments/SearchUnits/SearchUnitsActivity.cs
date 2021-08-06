
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
using BrightSign.Core.ViewModels.SearchUnits;
using BrightSign.Droid.Utility.Interface;
using BrightSign.Localization;
using MvvmCross.Droid.Support.V7.AppCompat;
using Plugin.CurrentActivity;

namespace BrightSign.Droid.Views.Fragments.SearchUnits
{
    //,Theme = "@style/MyTheme"
    [Activity(Label = "SearchUnitsActivity")]
	public class SearchUnitsActivity : MvxAppCompatActivity<SearchUnitsViewModel>
	{
		TextView mTitleTextView;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.SearchUnitsLayout);
			UserDialogs.Init(this);
			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			if (toolbar != null)
			{
				SetSupportActionBar(toolbar);
				View customView = LayoutInflater.Inflate(Resource.Layout.CustomActionBar, null);
				toolbar.AddView(customView);
                toolbar.Visibility = ViewStates.Gone;
			}
			mTitleTextView = FindViewById<TextView>(Resource.Id.TitleText);
			mTitleTextView.Text = "Select BrightSign";
			CrossCurrentActivity.Current.Activity = this;

			TextView versionText = FindViewById<TextView>(Resource.Id.versionText);
			var name = ApplicationContext.PackageManager.GetPackageInfo(ApplicationContext.PackageName, 0).VersionName;
			var code = ApplicationContext.PackageManager.GetPackageInfo(ApplicationContext.PackageName, 0).VersionCode;
			versionText.Text = "version " + name + string.Format(" ({0})", code);

		}
		public void ShowToolbarActionsForSelectBS(int position)
		{
			ViewModel.ViewTitle = "Select BrightSign";//BaseViewModel.TitleType.Active.ToString();

		}
		public void HideSoftKeyboard()
		{
			InputMethodManager manager = (InputMethodManager)GetSystemService(Context.InputMethodService);
			manager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
		}

		public override void OnBackPressed()
		{
			new DialogService().ShowAlertWithTwoButtons(Strings.exit_the_application, "", Strings.yes, Strings.no, () => OnCloseClick(), null);
			return;

		}

		bool OnCloseClick()
		{
			FinishAffinity();
			Process.KillProcess(Process.MyPid());
			System.Environment.Exit(1);
			return true;
		}


		void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("AcquireLock"))
			{
				var wifi = (WifiManager)ApplicationContext.GetSystemService(Context.WifiService);
				var mlock = wifi.CreateMulticastLock("Zeroconf lock");

				if (ViewModel.AcquireLock)
				{
					try
					{
						mlock.Acquire();
						Console.WriteLine("WifiMulticast Locked");
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
				}
				else
				{
					try
					{
						if (mlock.IsHeld)
						{
							mlock.Release();
						}
						Console.WriteLine("WifiMulticast Released");
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
				}
			}

		}

	}
}
