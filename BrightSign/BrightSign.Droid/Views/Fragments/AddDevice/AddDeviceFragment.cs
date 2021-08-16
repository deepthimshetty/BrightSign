
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using BrightSign.Core.ViewModels;
using BrightSign.Core.ViewModels.AddDevice;
using BrightSign.Core.ViewModels.SearchUnits;
using BrightSign.Droid.Views.Fragments.SearchUnits;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using Android.Views.InputMethods;

namespace BrightSign.Droid.Views.Fragments.AddDevice
{
    [MvxFragmentPresentation(typeof(SearchUnitsViewModel), Resource.Id.content_frame, true)]
    public class AddDeviceFragment : MvxFragment<AddDeviceViewModel>
    {
        Button btnSave;
        Button btnCancel;
        ImageButton closeBtn;
        ImageButton btnRefresh;
        View view;
        ImageView deviceImg;
        TextView deviceName, ipAddress;
        View deviceView;
        EditText editText;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            base.OnCreateView(inflater, container, savedInstanceState);

            view = this.BindingInflate(Resource.Layout.AddDeviceView, null);
            editText = view.FindViewById<EditText>(Resource.Id.ipEditText);
            deviceView = view.FindViewById(Resource.Id.deviceLayout);
            deviceView.Click += Layout_Click;
            deviceView.Visibility = ViewStates.Invisible;
            deviceName = deviceView.FindViewById<TextView>(Resource.Id.device_name);
            ipAddress = deviceView.FindViewById<TextView>(Resource.Id.device_ip);


            editText.KeyPress += (object sender, View.KeyEventArgs e) =>
            {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    ((SearchUnitsActivity)this.Activity).HideSoftKeyboard();
                    //add your logic here
                    e.Handled = true;
                }
            };
            RegisterEvents();
            ShowToolbarActions(true);

            var toolbar = ((SearchUnitsActivity)this.Activity).FindViewById(Resource.Id.toolbar);
            var txtTitle = ((SearchUnitsActivity)this.Activity).FindViewById<TextView>(Resource.Id.TitleText);
            txtTitle.Text = ViewModel.ViewTitle;
            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;
            return view;
        }
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AddButtonVisibility"))
            {

                if (ViewModel.bsdeviceAdd.Name != null)
                {
                    deviceView.Visibility = ViewStates.Visible;
                    deviceName.Text = ViewModel.bsdeviceAdd.Name;
                }
                if (ViewModel.bsdeviceAdd.IpAddress != null)
                {
                    ipAddress.Text = ViewModel.bsdeviceAdd.IpAddress;
                }
            }
        }

        public void RegisterEvents()
        {
            try
            {
                var toolbar = ((SearchUnitsActivity)this.Activity).FindViewById(Resource.Id.toolbar);
                closeBtn = toolbar.FindViewById<ImageButton>(Resource.Id.closeBtn);

                btnSave = toolbar.FindViewById<Button>(Resource.Id.btnSave);
                closeBtn.Click += BtnCancel_Click;

            }
            catch (Exception ex)
            {

            }
        }
        public override void OnDestroyView()
        {
            base.OnDestroyView();
            UnRegisterEvents();
            ShowToolbarActions(false);
        }

        public void UnRegisterEvents()
        {
            try
            {
                closeBtn.Click -= BtnCancel_Click;
            }
            catch (Exception ex)
            {

            }
        }

        void Layout_Click(object sender, EventArgs e)
        {
            ViewModel.AddCommand.Execute();
            //ViewModel.CancelCommand.Execute();
        }


        void BtnCancel_Click(object sender, EventArgs e)
        {
            ViewModel.CancelCommand.Execute();
        }


        public void ShowToolbarActions(bool isVisible)
        {
            if (btnCancel == null)
                btnCancel = ((SearchUnitsActivity)this.Activity).FindViewById<Button>(Resource.Id.btnCancel);
            btnCancel.Visibility = ViewStates.Invisible;

            if (btnSave == null)
                btnSave = ((SearchUnitsActivity)this.Activity).FindViewById<Button>(Resource.Id.btnSave);
            btnSave.Visibility = ViewStates.Invisible;
            if (btnRefresh == null)
                btnRefresh = ((SearchUnitsActivity)this.Activity).FindViewById<ImageButton>(Resource.Id.refreshImg);
            btnRefresh.Visibility = ViewStates.Invisible;

            if (closeBtn == null)
                closeBtn = ((SearchUnitsActivity)this.Activity).FindViewById<ImageButton>(Resource.Id.closeBtn);
            closeBtn.Visibility = ViewStates.Visible;

            // ((SearchUnitsActivity)this.Activity).ShowHideHomeMenu(!isVisible);

        }
    }
}
