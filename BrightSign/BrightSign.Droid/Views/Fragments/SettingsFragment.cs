
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat.Widget;
using MvvmCross.Droid.Views.Attributes;
using static BrightSign.Core.Utility.BSUtility;

namespace BrightSign.Droid.Views.Fragments
{
    //[MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    public class SettingsFragment : MvxFragment<SettingsViewModel>
    {
        private View view;
        Button btnSave;
        Button btnCancel;
        ImageButton btnHome;
        ImageButton btnSmall, btnMedium, btnLarge;
        LinearLayout smallBtnLayout, mediumBtnLayout, largeBtnLayout;
        LinearLayout imgrotateLayout;
        MvxListView mListView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            base.OnCreateView(inflater, container, savedInstanceState);
            view = this.BindingInflate(Resource.Layout.SettingsView, null);

            var topView = view.FindViewById(Resource.Id.top_view);

            TextView deviceName = topView.FindViewById<TextView>(Resource.Id.top_device_name);
            deviceName.Text = ViewModel.CurrentDevice.Name;
            TextView deviceIP = topView.FindViewById<TextView>(Resource.Id.top_device_ip);
            deviceIP.Text = "IP Address:" + ViewModel.CurrentDevice.IpAddress;
            ImageView deviceImg = topView.FindViewById<ImageView>(Resource.Id.top_deviceImg);

            btnSmall = view.FindViewById<ImageButton>(Resource.Id.smallBtn);
            btnMedium = view.FindViewById<ImageButton>(Resource.Id.mediumBtn);
            btnLarge = view.FindViewById<ImageButton>(Resource.Id.largeBtn);

            smallBtnLayout = view.FindViewById<LinearLayout>(Resource.Id.smallBtnLayout);
            mediumBtnLayout = view.FindViewById<LinearLayout>(Resource.Id.mediumBtnLayout);
            largeBtnLayout = view.FindViewById<LinearLayout>(Resource.Id.largeBtnLayout);

            if (ViewModel.SelectedButtonType == 0)
            {
                btnSmall.SetImageResource(Resource.Drawable.action_selected);
                smallBtnLayout.SetBackgroundResource(Resource.Drawable.shape);
            }
            else if (ViewModel.SelectedButtonType == 1)
            {
                btnMedium.SetImageResource(Resource.Drawable.action_selected);
                mediumBtnLayout.SetBackgroundResource(Resource.Drawable.shape);
            }
            else if (ViewModel.SelectedButtonType == 2)
            {
                btnLarge.SetImageResource(Resource.Drawable.action_selected);
                largeBtnLayout.SetBackgroundResource(Resource.Drawable.shape);
            }

            RegisterEvents();


            ShowToolbarActions(ViewModel.IsDataModified);

            // var resourceId = (int)typeof(Resource.Drawable).GetField(ViewModel.CurrentDevice.Image).GetValue(null);
            // deviceImg.SetImageResource(resourceId);

            return view;
        }
        public override void OnResume()
        {
            base.OnResume();
            if (Constants.CurrentTab == TitleType.Settings)
            {
                ((MainActivity)this.Activity).ShowToolbarActions(4);
                ShowToolbarActions(ViewModel.IsDataModified);
            }
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsDataModified")
            {
                ShowToolbarActions(ViewModel.IsDataModified);
            }
            else if (e.PropertyName == "SelectedButtonType")
            {
                if (ViewModel.SelectedButtonType == 0)
                {
                    btnSmall.SetImageResource(Resource.Drawable.action_selected);
                    smallBtnLayout.SetBackgroundResource(Resource.Drawable.shape);

                    btnMedium.SetImageResource(Resource.Drawable.action);
                    mediumBtnLayout.SetBackgroundResource(Resource.Drawable.round_corner_view);
                    btnLarge.SetImageResource(Resource.Drawable.action);
                    largeBtnLayout.SetBackgroundResource(Resource.Drawable.round_corner_view);
                }
                else if (ViewModel.SelectedButtonType == 1)
                {
                    btnSmall.SetImageResource(Resource.Drawable.action);
                    smallBtnLayout.SetBackgroundResource(Resource.Drawable.round_corner_view);

                    btnMedium.SetImageResource(Resource.Drawable.action_selected);
                    mediumBtnLayout.SetBackgroundResource(Resource.Drawable.shape);

                    btnLarge.SetImageResource(Resource.Drawable.action);
                    largeBtnLayout.SetBackgroundResource(Resource.Drawable.round_corner_view);
                }
                else if (ViewModel.SelectedButtonType == 2)
                {
                    btnSmall.SetImageResource(Resource.Drawable.action);
                    smallBtnLayout.SetBackgroundResource(Resource.Drawable.round_corner_view);
                    btnMedium.SetImageResource(Resource.Drawable.action);
                    mediumBtnLayout.SetBackgroundResource(Resource.Drawable.round_corner_view);
                    btnLarge.SetImageResource(Resource.Drawable.action_selected);
                    largeBtnLayout.SetBackgroundResource(Resource.Drawable.shape);
                }
            }
        }

        void BtnHome_Click(object sender, EventArgs e)
        {
            ViewModel.ChangeDeviceCommand.Execute();
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.Finish();
        }

        void BtnSmall_Click(object sender, EventArgs e)
        {

            ViewModel.SelectedButtonType = 0;
        }
        void BtnMedium_Click(object sender, EventArgs e)
        {

            ViewModel.SelectedButtonType = 1;
        }
        void BtnLarge_Click(object sender, EventArgs e)
        {

            ViewModel.SelectedButtonType = 2;
        }
        public void RegisterEvents()
        {
            try
            {
                var toolbar = ((MainActivity)this.Activity).FindViewById(Resource.Id.toolbar);
                btnCancel = toolbar.FindViewById<Button>(Resource.Id.btnCancel);
                btnHome = toolbar.FindViewById<ImageButton>(Resource.Id.btnHome);
                imgrotateLayout = toolbar.FindViewById<LinearLayout>(Resource.Id.rotateLayout);
                btnHome.Click += BtnHome_Click;
                btnCancel.Text = "Reset";
                btnSave = toolbar.FindViewById<Button>(Resource.Id.btnSave);
                btnSave.Text = "Save";
                btnSave.Click += BtnDone_Click;
                btnCancel.Click += BtnReset_Click;

                smallBtnLayout.Click += BtnSmall_Click;
                mediumBtnLayout.Click += BtnMedium_Click;
                largeBtnLayout.Click += BtnLarge_Click;
                btnSmall.Click += BtnSmall_Click;
                btnMedium.Click += BtnMedium_Click;
                btnLarge.Click += BtnLarge_Click;
                ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            }
            catch (Exception ex)
            {

            }
        }
        public override void OnDestroyView()
        {
            base.OnDestroyView();
            UnRegisterEvents();
        }

        public void UnRegisterEvents()
        {
            try
            {
                btnSave.Click -= BtnDone_Click;
                btnCancel.Click -= BtnReset_Click;

                btnHome.Click -= BtnHome_Click;
                smallBtnLayout.Click -= BtnSmall_Click;
                mediumBtnLayout.Click -= BtnMedium_Click;
                largeBtnLayout.Click -= BtnLarge_Click;
                btnSmall.Click -= BtnSmall_Click;
                btnMedium.Click -= BtnMedium_Click;
                btnLarge.Click -= BtnLarge_Click;

                ViewModel.PropertyChanged -= ViewModel_PropertyChanged;

            }
            catch (Exception ex)
            {

            }
        }
        void BtnDone_Click(object sender, EventArgs e)
        {
            btnHome.Visibility = ViewStates.Visible;
            ViewModel.SaveCommand.Execute();
        }

        void BtnReset_Click(object sender, EventArgs e)
        {
            btnHome.Visibility = ViewStates.Visible;
            ViewModel.CancelCommand.Execute();
        }

        public void ShowToolbarActions(bool isVisible)
        {
            if (imgrotateLayout != null)
            {
                imgrotateLayout.Visibility = ViewStates.Gone;
            }

            btnHome.Visibility = ViewModel.IsDataModified ? ViewStates.Gone : ViewStates.Visible;
            if (btnCancel == null)
                btnCancel = ((MainActivity)this.Activity).FindViewById<Button>(Resource.Id.btnCancel);
            btnCancel.Visibility = isVisible ? ViewStates.Visible : ViewStates.Invisible;

            if (btnSave == null)
                btnSave = ((MainActivity)this.Activity).FindViewById<Button>(Resource.Id.btnSave);
            btnSave.Visibility = isVisible ? ViewStates.Visible : ViewStates.Invisible;

            var refreshImg = ((MainActivity)this.Activity).FindViewById<ImageButton>(Resource.Id.refreshImg);
            if (refreshImg != null)
            {
                refreshImg.Visibility = ViewStates.Invisible;
            }

        }

        public void ShowToolbarItems()
        {
            bool isVisible = ViewModel.IsDataModified;
            btnHome.Visibility = ViewModel.IsDataModified ? ViewStates.Gone : ViewStates.Visible;
            if (btnCancel == null)
                btnCancel = ((MainActivity)this.Activity).FindViewById<Button>(Resource.Id.btnCancel);
            btnCancel.Visibility = isVisible ? ViewStates.Visible : ViewStates.Invisible;

            if (btnSave == null)
                btnSave = ((MainActivity)this.Activity).FindViewById<Button>(Resource.Id.btnSave);
            btnSave.Visibility = isVisible ? ViewStates.Visible : ViewStates.Invisible;

            var refreshImg = ((MainActivity)this.Activity).FindViewById<ImageButton>(Resource.Id.refreshImg);
            if (refreshImg != null)
            {
                refreshImg.Visibility = ViewStates.Invisible;
            }

        }
    }
}
