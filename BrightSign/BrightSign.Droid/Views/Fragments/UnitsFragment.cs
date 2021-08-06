
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using BrightSign.Core.ViewModels;
using BrightSign.Core.ViewModels.SearchUnits;
using BrightSign.Core.ViewModels.Units;
using BrightSign.Droid.Views.BaseClasses;
using BrightSign.Droid.Views.Fragments.SearchUnits;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;

namespace BrightSign.Droid.Views.Fragments
{
    [MvvmCross.Droid.Views.Attributes.MvxFragmentPresentation(typeof(SearchUnitsViewModel), Resource.Id.content_frame, AddToBackStack = false)]
    public class UnitsFragment : MvxFragment<UnitsViewModel>
    {
        string[] _tabHeader = { "Active", "Offline" };
        UnitsViewModel mhomeViewModel;
        private int tabSelected;
        public TabLayout _tabLayout;
        View view;
        Button btnSave;
        Button btnCancel, backbtn;
        ImageButton refreshImg,btnHome;
        ImageButton closeBtn;
        LinearLayout imgrotateLayout;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
            // Create your fragment here
        }

        SearchUnitsActivity activity;

        UnitsTabAdapter _adapter;

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            // mhomeViewModel.CreateTabsForAndroid();
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            base.OnCreateView(inflater, container, savedInstanceState);
            view = this.BindingInflate(Resource.Layout.units_layout, null);
            mhomeViewModel = ViewModel as UnitsViewModel;
            var toolbar = ((SearchUnitsActivity)Activity).FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                toolbar.Visibility = ViewStates.Visible;
            }

            refreshImg = toolbar.FindViewById<ImageButton>(Resource.Id.refreshImg);
            btnHome = toolbar.FindViewById<ImageButton>(Resource.Id.btnHome);
            imgrotateLayout = toolbar.FindViewById<LinearLayout>(Resource.Id.rotateLayout);
            refreshImg.Click += BtnRefresh_Click;
            activity = (SearchUnitsActivity)Activity;
            // activity.updateTitle(mhomeViewModel.ViewTitle);
            //activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //activity.SupportActionBar.SetHomeButtonEnabled(true);

            //I am temeporarily putting connectLayout parameters here, Change those parameters when new layout files are developed
            var fragments = new List<MvxViewPagerFragmentAdapter.FragmentInfo> {
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(ActiveView),
                    ViewModel = mhomeViewModel.activeViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(OfflineView),
                    ViewModel = mhomeViewModel.offlineViewModel
                }
            };
            var _viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPagerDevice);
            _adapter = new UnitsTabAdapter(activity, ChildFragmentManager, fragments);
            _tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabsDevice);
            // Set adapter to view pager
            _viewPager.Adapter = _adapter;
            // Setup tablayout with view pager
            _tabLayout.SetupWithViewPager(_viewPager);
            _tabLayout.TabSelected += (sender, e) =>
            {
                activity.ShowToolbarActionsForSelectBS(e.Tab.Position);
                ShowToolbarActions(true);
            };
            // Iterate over all tabs and set the custom view
            for (int i = 0; i < _tabLayout.TabCount; i++)
            {
                TabLayout.Tab tab = _tabLayout.GetTabAt(i);
                tab.SetCustomView(_adapter.GetTabView(i, tab.IsSelected, _tabHeader[i]));

            }

            ShowToolbarActions(true);
            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;
            mhomeViewModel.activeViewModel.ActiveItemSource = mhomeViewModel.ActiveDevices;
            mhomeViewModel.offlineViewModel.OfflineItemSource = mhomeViewModel.OfflineDevices;
            // RetainInstance = false;
            return view;
        }

        void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ViewModel.RefreshCommand.Execute();
            }
            catch (Exception ex)
            {

            }
        }
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ActiveDevices"))
            {
                mhomeViewModel.activeViewModel.ActiveItemSource = mhomeViewModel.ActiveDevices;
            }

            if (e.PropertyName.Equals("OfflineDevices"))
            {
                mhomeViewModel.offlineViewModel.OfflineItemSource = mhomeViewModel.OfflineDevices;
            }
        }
       
        public void ShowToolbarActions(bool isVisible)
        {
            if (imgrotateLayout != null)
            {
                imgrotateLayout.Visibility = ViewStates.Gone;
            }

            btnHome.Visibility = ViewStates.Gone;
            if (refreshImg == null)
                refreshImg = ((SearchUnitsActivity)this.Activity).FindViewById<ImageButton>(Resource.Id.refreshImg);
            refreshImg.Visibility = isVisible ? ViewStates.Visible : ViewStates.Invisible;


            if (btnCancel == null)
                btnCancel = ((SearchUnitsActivity)this.Activity).FindViewById<Button>(Resource.Id.btnCancel);
            btnCancel.Visibility = isVisible ? ViewStates.Invisible : ViewStates.Visible;

            if (backbtn == null)
                backbtn = ((SearchUnitsActivity)this.Activity).FindViewById<Button>(Resource.Id.btnBack);
            backbtn.Visibility = isVisible ? ViewStates.Invisible : ViewStates.Visible;

            if (btnSave == null)
                btnSave = ((SearchUnitsActivity)this.Activity).FindViewById<Button>(Resource.Id.btnSave);
            btnSave.Visibility = isVisible ? ViewStates.Invisible : ViewStates.Visible;

            if (closeBtn == null)
                closeBtn = ((SearchUnitsActivity)this.Activity).FindViewById<ImageButton>(Resource.Id.closeBtn);
            closeBtn.Visibility = ViewStates.Invisible;
        }

        class UnitsTabAdapter : MvxViewPagerFragmentAdapter
        {
            private Context _context;

            public UnitsTabAdapter(
                Context context, Android.Support.V4.App.FragmentManager fragmentManager, IEnumerable<FragmentInfo> fragments)
                : base(context, fragmentManager, fragments)
            {
                _context = context;
            }

            public View GetTabView(int position, bool isSelected, string title)
            {
                LinearLayout layout = (LinearLayout)LayoutInflater.From(_context).Inflate(Resource.Layout.CustomTabLayout, null);
                TextView tv = (TextView)layout.FindViewById(Resource.Id.tabTitle);
                tv.Text = title;

                return tv;
            }
        }
    }
}
