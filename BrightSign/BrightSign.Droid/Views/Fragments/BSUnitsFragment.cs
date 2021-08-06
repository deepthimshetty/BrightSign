
using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using BrightSign.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;

namespace BrightSign.Droid.Views.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    public class BSUnitsFragment : MvxFragment<BSUnitsViewModel>
    {
        private View view;
        Button btnSave, btnCancel;
        MvxListView mListView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            view =this.BindingInflate(Resource.Layout.BSUnitsView, null);
            ShowToolbarActions(true);
            RegisterEvents();

            var toolbar = ((MainActivity)this.Activity).FindViewById(Resource.Id.toolbar);
            var txtTitle = ((MainActivity)this.Activity).FindViewById<TextView>(Resource.Id.TitleText);
            txtTitle.Text = ViewModel.ViewTitle;
            return view;
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
            ViewModel.SaveCommand.Execute();
        }

        void BtnCancel_Click(object sender, EventArgs e)
        {
            ViewModel.CancelCommand.Execute();
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
    }
}
