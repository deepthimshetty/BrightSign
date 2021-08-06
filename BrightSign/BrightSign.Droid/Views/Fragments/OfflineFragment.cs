
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using BrightSign.Core.Models;
using BrightSign.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;

namespace BrightSign.Droid.Views.Fragments
{
    [MvvmCross.Droid.Views.Attributes.MvxTabLayoutPresentation("Offline", Resource.Id.viewPagerDevice, Resource.Id.tabsDevice, typeof(MainViewModel))]
    public class OfflineView : MvxFragment<OfflineViewModel>
    {
       
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
            View view = this.BindingInflate(Resource.Layout.offline_view, null);
            var offlineListView = view.FindViewById<MvxListView>(Resource.Id.active_list);
            var floatingBtn = view.FindViewById<FloatingActionButton>(Resource.Id.fabBtn);
            floatingBtn.Click += Btnfloating_Click;
            RegisterForContextMenu(offlineListView);

            return view;
        }
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
       
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.active_list)
            {
                var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
                //menu.SetHeaderTitle(_countries[info.Position]);
                var menuItems = Resources.GetStringArray(Resource.Array.menu);
                for (var i = 0; i < menuItems.Length; i++)
                    menu.Add(Menu.None, i, i, menuItems[i]);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            try
            {
                var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
                var menuItemIndex = item.ItemId;
                var menuItems = Resources.GetStringArray(Resource.Array.menu);
                var menuItemName = menuItems[menuItemIndex];
                //var listItemName = _countries[info.Position];
                ViewModel.RemoveCommand.Execute(info.Position);
            }
            catch (Exception ex)
            {

            }
            //Toast.MakeText(this, string.Format("Selected {0} for item {1}", menuItemName,1), ToastLength.Short).Show();
            return true;
        }
    }
}
