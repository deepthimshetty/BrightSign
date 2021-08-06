
using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using BrightSign.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;

namespace BrightSign.Droid.Views.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    public class AddActionFragment : MvxFragment<AddActionViewModel>
    {
        Button btnSave;
        Button btnCancel;
        private MainActivity activity;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.AddActionView, null);
            RegisterEvents();
            ShowToolbarActions(true);
            activity = (MainActivity)Activity;

            return view;
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
                btnSave.Click -= BtnSave_Click;
                btnCancel.Click -= BtnCancel_Click;
            }
            catch (Exception ex)
            {

            }
        }

        public void RegisterEvents()
        {
            try
            {
                var toolbar = ((MainActivity)this.Activity).FindViewById(Resource.Id.toolbar);
                btnCancel = toolbar.FindViewById<Button>(Resource.Id.btnCancel);

                btnSave = toolbar.FindViewById<Button>(Resource.Id.btnSave);
                btnSave.Click += BtnSave_Click;
                btnCancel.Click += BtnCancel_Click;

            }
            catch (Exception ex)
            {

            }
        }

        public void ShowToolbarActions(bool isVisible)
        {
            if (btnCancel == null)
                btnCancel = ((MainActivity)this.Activity).FindViewById<Button>(Resource.Id.btnCancel);
            btnCancel.Visibility = isVisible ? ViewStates.Visible : ViewStates.Invisible;

            if (btnSave == null)
                btnSave = ((MainActivity)this.Activity).FindViewById<Button>(Resource.Id.btnSave);
            btnSave.Visibility = isVisible ? ViewStates.Visible : ViewStates.Invisible;
        }

        /// <summary>
        /// Navigate to Back
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void BtnCancel_Click(object sender, EventArgs e)
        {
            activity.HideKeybord();
            ViewModel.CancelCommand.Execute();
        }

        /// <summary>
        /// Save an Action
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                activity.HideKeybord();
                ViewModel.SaveCommand.Execute();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
