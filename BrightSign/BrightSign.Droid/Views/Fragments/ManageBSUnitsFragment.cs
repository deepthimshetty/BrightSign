
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using BrightSign.Core.Models;
using BrightSign.Core.ViewModels;
using BrightSign.Core.ViewModels.Settings;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;

namespace BrightSign.Droid.Views.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    public class ManageBSUnitsFragment : MvxFragment<ManageBSUnitsViewModel>
    {
        Button btnSave;
        Button btnCancel;
        IMvxAdapter mAdapter;
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
            var view = this.BindingInflate(Resource.Layout.ManageBSUnitsView, null);
            var RLayoutAddBsUnit = view.FindViewById<RelativeLayout>(Resource.Id.rLayoutAddBSUnit);
            RLayoutAddBsUnit.Click+=RLayoutAddBsUnit_Click;
            RegisterEvents();
            ShowToolbarActions(true);

            var toolbar = ((MainActivity)this.Activity).FindViewById(Resource.Id.toolbar);
            var txtTitle = ((MainActivity)this.Activity).FindViewById<TextView>(Resource.Id.TitleText);
            txtTitle.Text = ViewModel.ViewTitle;

            var listView = view.FindViewById<MvxListView>(Resource.Id.lvBSUnits);

            mAdapter = new ManageBSUnitAdapter(this.Activity, (IMvxAndroidBindingContext)BindingContext);
            listView.Adapter = mAdapter;
            RegisterForContextMenu(listView);

            return view;
        }

        void RLayoutAddBsUnit_Click(object sender, EventArgs e)
        {
            ViewModel.AddUnitCommand.Execute();
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

        private class ManageBSUnitAdapter : MvxAdapter
        {

            public ManageBSUnitAdapter(FragmentActivity context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
            {

            }

            //public override View GetView(int position, View convertView, ViewGroup parent)
            //{
            //    //return base.GetView(position, convertView, parent);

            //    View row = convertView;

            //    if (row == null)
            //    {
            //        row = LayoutInflater.From(Context).BindingInflate(Resource.Layout.bsunitcell, null, false);
            //    }
            //    CheckBox checkBox = row.FindViewById<CheckBox>(Resource.Id.chkBSUnit);
            //    checkBox.Visibility = ViewStates.Gone;

            //    NotifyDataSetChanged();
            //    return row;
            //}

            protected override View GetBindableView(View convertView, object dataContext, ViewGroup parent, int templateId)
            {
                //View row = convertView;

                //if (row == null)
                //{
                templateId = Resource.Layout.bsunitcell;
                View row = base.GetBindableView(convertView, dataContext, parent, templateId);
                // row = LayoutInflater.From(Context).BindingInflate(Resource.Layout.bsunitcell, null, false);
                //}
                CheckBox checkBox = row.FindViewById<CheckBox>(Resource.Id.chkBSUnit);
                checkBox.Visibility = ViewStates.Gone;

                return row;
            }

            public override int ViewTypeCount
            {
                get
                {
                    return 1;
                }
            }
            public override int GetItemViewType(int position)
            {
                return 0;
            }
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.lvBSUnits)
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
                ViewModel.DeleteUnitCommand.Execute(info.Position);
            }
            catch (Exception ex)
            {

            }
            //Toast.MakeText(this, string.Format("Selected {0} for item {1}", menuItemName,1), ToastLength.Short).Show();
            return true;
        }
    }
}