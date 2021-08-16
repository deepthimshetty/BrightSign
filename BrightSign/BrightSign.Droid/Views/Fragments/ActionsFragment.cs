
using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using BrightSign.Core.Utility;
using BrightSign.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace BrightSign.Droid.Views.Fragments
{
	public class ActionsFragment : MvxFragment<ActionsViewModel>
	{
		private View view, topView;
		Button defaultBtn, userDefinedBtn, btnEdit;
		View defaultView, userDefinedView;
		ImageButton btnHome;
		bool isFirstLoad = false;

		public override void OnCreate(Bundle savedInstanceState)
		{
			isFirstLoad = true;
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			((MainActivity)this.Activity).HideKeybord();
			view = this.BindingInflate(Resource.Layout.ActionsView, null);
			topView = view.FindViewById(Resource.Id.actions_top_view);

			var toolbar = ((MainActivity)this.Activity).FindViewById(Resource.Id.toolbar);
			btnEdit = toolbar.FindViewById<Button>(Resource.Id.btnSave);
			btnEdit.Click += BtnEdit_Click;
			TextView deviceName = topView.FindViewById<TextView>(Resource.Id.top_device_name);
			deviceName.Text = ViewModel.CurrentDevice.Name;
			TextView deviceIP = topView.FindViewById<TextView>(Resource.Id.top_device_ip);
			deviceIP.Text = "IP Address:" + ViewModel.CurrentDevice.IpAddress;
			ImageView deviceImg = topView.FindViewById<ImageView>(Resource.Id.top_deviceImg);
			btnHome = topView.FindViewById<ImageButton>(Resource.Id.btnHome);

			defaultBtn = view.FindViewById<Button>(Resource.Id.defaultBtn);
			defaultBtn.Click += BtnDefault_Click;
			userDefinedBtn = view.FindViewById<Button>(Resource.Id.userDefinedBtn);
			userDefinedBtn.Click += BtnUserDefined_Click;

			defaultView = view.FindViewById(Resource.Id.defaultView);
			userDefinedView = view.FindViewById(Resource.Id.userDefinedView);
			RegisterEvents();

			         
			return view;
		}

		/// <summary>
		/// Navigate to ManageActions Page
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void BtnEdit_Click(object sender, EventArgs e)
		{
			ViewModel.EditActionsCommand.Execute();
		}

		/// <summary>
		/// Navigate to Device Search Page
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void BtnHome_Click(object sender, EventArgs e)
		{
			ViewModel.ChangeDeviceCommand.Execute();
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.Finish();
		}
		private void SelectDefaultTab()
		{
			if (ViewModel.SelectedTabIndex == 0)
			{
				defaultView.SetBackgroundColor(Android.Graphics.Color.DarkOrange);
				userDefinedView.SetBackgroundColor(Android.Graphics.Color.White);
			}
			else
			{
				userDefinedView.SetBackgroundColor(Android.Graphics.Color.DarkOrange);
				defaultView.SetBackgroundColor(Android.Graphics.Color.White);
			}
		}
		void BtnDefault_Click(object sender, EventArgs e)
		{
			defaultView.SetBackgroundColor(Android.Graphics.Color.DarkOrange);
			userDefinedView.SetBackgroundColor(Android.Graphics.Color.White);
			ViewModel.SelectedTabIndex = 0;
		}
		void BtnUserDefined_Click(object sender, EventArgs e)
		{
			userDefinedView.SetBackgroundColor(Android.Graphics.Color.DarkOrange);
			defaultView.SetBackgroundColor(Android.Graphics.Color.White);
			ViewModel.SelectedTabIndex = 1;
		}

		int numColumns = 0;
		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			//Get Button type defined in settings
			var itemId = ViewModel.SelectedButtonType;
			var gridView = view.FindViewById<GridView>(Resource.Id.gvActions);

			if (itemId == 0)
				numColumns = 3;
			else if (itemId == 1)
			{
				numColumns = 1;
				var parametss = (LinearLayout.LayoutParams)gridView.LayoutParameters;
				parametss.SetMargins(150, 0, 150, 0);
				gridView.LayoutParameters = parametss;
			}
			else
			{
				numColumns = 1;
			}
			gridView.SetNumColumns(numColumns);

			base.OnViewCreated(view, savedInstanceState);
		}

		public void RegisterEvents()
		{
			try
			{
				var toolbar = ((MainActivity)this.Activity).FindViewById(Resource.Id.toolbar);
				
				var btnCancel = ((MainActivity)this.Activity).FindViewById<Button>(Resource.Id.btnCancel);
				btnCancel.Visibility = ViewStates.Gone;
				SelectDefaultTab();

				if (btnHome == null)
					btnHome = topView.FindViewById<ImageButton>(Resource.Id.btnHome);
				btnHome.Click += BtnHome_Click;
                
			}
			catch (Exception ex)
			{

			}
		}

		public override void OnResume()
		{
			base.OnResume();
			if (!isFirstLoad && Constants.CurrentTab == TitleType.Actions)
				((MainActivity)this.Activity).ShowToolbarActions(1);
			isFirstLoad = false;
		}

		public override void OnDestroyView()
		{
			base.OnDestroyView();
			UnRegisterEvents();
		}

		/// <summary>
		/// UnRegister all event handlers while leaving from the Fragment/Page
		/// </summary>
		public void UnRegisterEvents()
		{
			try
			{
				btnEdit.Click -= BtnEdit_Click;
				btnHome.Click -= BtnHome_Click;
			}
			catch (Exception ex)
			{

			}
		}


	}
}
