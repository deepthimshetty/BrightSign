
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
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;

namespace BrightSign.Droid.Views.Fragments.Snapshot
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    public class RemoteSnapshotFragment : MvxFragment<RemoteSnapshotViewModel>
    {
        Button btnSave;
        Button btnCancel;
        TextView snapshotFrequency, numberOfSnapshots, jpegQuality;
        SeekBar frequencySeekBar, numberOfSnapshotSeekbar, jpegSeekbar;
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

            ((MainActivity)this.Activity).HideAllIcons();

            var view = this.BindingInflate(Resource.Layout.RemoteSnapshotView, null);

            var topView = view.FindViewById(Resource.Id.remote_top_view);

            TextView deviceName = topView.FindViewById<TextView>(Resource.Id.top_device_name);
            deviceName.Text = ViewModel.CurrentDevice.Name;
            TextView deviceIP = topView.FindViewById<TextView>(Resource.Id.top_device_ip);
            deviceIP.Text = "IP Address:" + ViewModel.CurrentDevice.IpAddress;
            ImageView deviceImg = topView.FindViewById<ImageView>(Resource.Id.top_deviceImg);

            jpegQuality = view.FindViewById<TextView>(Resource.Id.maxJpeglevel);
            numberOfSnapshots = view.FindViewById<TextView>(Resource.Id.maxSnapshot);
            snapshotFrequency = view.FindViewById<TextView>(Resource.Id.maxFrequency);

            frequencySeekBar = view.FindViewById<SeekBar>(Resource.Id.sbarFrequency);
            numberOfSnapshotSeekbar = view.FindViewById<SeekBar>(Resource.Id.sbarNoOfSnaps);
            jpegSeekbar = view.FindViewById<SeekBar>(Resource.Id.sbarJpg);
            //  frequencySeekBar.ProgressChanged+=FrequencySeekBar_ProgressChanged;
            //  numberOfSnapshotSeekbar.ProgressChanged += SnapshotSeekBar_ProgressChanged;
            //   jpegSeekbar.ProgressChanged += JpegSeekBar_ProgressChanged;
            RegisterEvents();

            return view;
        }

        //void BtnArrow_Click(object sender, EventArgs e)
        //{
        //   // ViewModel.ChangeDeviceCommand.Execute();
        //}
        public void RegisterEvents()
        {
            try
            {
                var toolbar = ((MainActivity)this.Activity).FindViewById(Resource.Id.toolbar);
                var title = toolbar.FindViewById<TextView>(Resource.Id.TitleText);
                title.Text = ViewModel.ViewTitle;
                var btnHome = toolbar.FindViewById<ImageButton>(Resource.Id.btnHome);
                btnHome.Visibility = ViewStates.Invisible;

                btnCancel = toolbar.FindViewById<Button>(Resource.Id.btnBack);
                btnCancel.Visibility = ViewStates.Visible;

                if (btnSave == null)
                    btnSave = ((MainActivity)this.Activity).FindViewById<Button>(Resource.Id.btnSave);
                btnSave.Visibility = ViewStates.Visible;

                btnSave = toolbar.FindViewById<Button>(Resource.Id.btnSave);
                btnSave.SetTextColor(Android.Graphics.Color.White);
                btnSave.Click += BtnSave_Click;
                btnCancel.Click += BtnCancel_Click;

            }
            catch (Exception ex)
            {

            }
        }

        void FrequencySeekBar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            snapshotFrequency.Text = e.Progress.ToString();
        }

        void SnapshotSeekBar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            numberOfSnapshots.Text = e.Progress.ToString();
        }

        void JpegSeekBar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            jpegQuality.Text = e.Progress.ToString();
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
                btnSave.Click -= BtnSave_Click;
                btnCancel.Click -= BtnCancel_Click;
            }
            catch (Exception ex)
            {

            }
        }
        void BtnSave_Click(object sender, EventArgs e)
        {
            ViewModel.SaveRSCommand.Execute();
        }

        void BtnCancel_Click(object sender, EventArgs e)
        {
            ViewModel.CancelRSCommand.Execute();
        }

    }
}
