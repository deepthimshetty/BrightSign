
using System;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using BrightSign.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace BrightSign.Droid.Views.Fragments
{
    [MvvmCross.Droid.Views.Attributes.MvxTabLayoutPresentation("Active", Resource.Id.viewPagerDevice, Resource.Id.tabsDevice, typeof(MainViewModel))]
    public class ActiveView : MvxFragment<ActiveViewModel>
    {
        ImageButton refreshImg;
        FloatingActionButton floatingBtn;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = this.BindingInflate(Resource.Layout.active_view, null);

            floatingBtn = view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            floatingBtn.Click += Btnfloating_Click;

            return view;

        }

        /// <summary>
        /// Navigate to Add New Device Page
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Btnfloating_Click(object sender, EventArgs e)
        {
            try
            {
                ViewModel.AddDeviceCommand.Execute();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
