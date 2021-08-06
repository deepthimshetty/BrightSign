
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using BrightSign.Core.Utility;
using BrightSign.Core.ViewModels;
using BrightSign.Droid.Views.BaseClasses;
using BrightSign.Droid.Views.Fragments.Snapshot;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace BrightSign.Droid.Views.Fragments
{
	[MvvmCross.Droid.Views.Attributes.MvxFragmentPresentation(AddToBackStack = true)]
	public class HomeFragment : MvxFragment
	{
		Android.Support.Design.Widget.BottomNavigationView bottomNavigation;
		MainViewModel mhomeViewModel;
		public TabLayout _tabLayout;
		View view;
		MainActivity activity;
		TabAdapter _adapter;
        ViewPager _viewPager;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

        }

        public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			Constants.CurrentTab = TitleType.Variables;
			HasOptionsMenu = true;
            RetainInstance = true;

		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);
		}


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            view = this.BindingInflate(Resource.Layout.HomeView, null);
           
                mhomeViewModel = ViewModel as MainViewModel;
                activity = (MainActivity)Activity;


                try
                {
                    //I am temeporarily putting connectLayout parameters here, Change those parameters when new layout files are developed
                    var fragments = new List<MvxViewPagerFragmentAdapter.FragmentInfo> {
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(VariablesView),
                    ViewModel = mhomeViewModel.variablesViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(ActionsFragment),
                    ViewModel = mhomeViewModel.actionsViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(DiagnosticsFragment),
                    ViewModel = mhomeViewModel.diagnosticViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(SnapshotsFragment),
                    ViewModel = mhomeViewModel.snapshotsViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(SettingsFragment),
                    ViewModel = mhomeViewModel.settingsViewModel
                }
            };
                     _viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPagerDevice);

                    _adapter = new TabAdapter(activity, ChildFragmentManager, fragments);

                    // Set adapter to view pager
                    _viewPager.Adapter = _adapter;
                    _viewPager.HorizontalScrollBarEnabled = false;
                    bottomNavigation = view.FindViewById<Android.Support.Design.Widget.BottomNavigationView>(Resource.Id.bottom_navigation);

                    bottomNavigation.NavigationItemSelected += (sender, e) =>
                    {
                        switch (e.Item.ItemId)
                        {
                            case Resource.Id.menu_variables:
                                Constants.CurrentTab = TitleType.Variables;
                                activity.ShowToolbarActions(0);
                                _viewPager.SetCurrentItem(0, true);
                                break;
                            case Resource.Id.menu_actions:
                                Constants.CurrentTab = TitleType.Actions;
                                activity.ShowToolbarActions(1);
                                _viewPager.SetCurrentItem(1, true);

                                break;
                            case Resource.Id.menu_diagnostics:
                                Constants.CurrentTab = TitleType.Diagnostics;
                                _viewPager.SetCurrentItem(2, true);
                                activity.ShowToolbarActions(2);
                                break;
                            case Resource.Id.menu_gallery:
                                Constants.CurrentTab = TitleType.Gallery;
                                _viewPager.SetCurrentItem(3, true);
                                activity.ShowToolbarActions(3);
                                break;
                            case Resource.Id.menu_settings:
                                Constants.CurrentTab = TitleType.Settings;
                                activity.ShowToolbarActions(4);
                                _viewPager.SetCurrentItem(4, true);
                                try
                                {
                                    var frags = (_viewPager.Adapter as TabAdapter).Fragments;
                                    var frag4 = frags.LastOrDefault();
                                    (frag4.ViewModel as SettingsViewModel).IsDataModified = (frag4.ViewModel as SettingsViewModel).IsDataModified;

                                }
                                catch (System.Exception ex)
                                {

                                }


                                break;
                            default:
                                break;
                        }

                    };

                    Utility.Utility.SetShiftMode(bottomNavigation, false, false);
                    return view;

                }
                catch (System.Exception e)
                {
                    var x = e.Data;

                    var z = e.Message;
                    return view;

                }
        
		}

        public override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("LastId", _viewPager.CurrentItem);
            base.OnSaveInstanceState(outState);

        }

        public override void OnViewStateRestored(Bundle savedInstanceState)
        {

            if (savedInstanceState != null)
            {
                int tabId = savedInstanceState.GetInt("LastId", -1);
                if (tabId != -1)
                {
                    if (_viewPager != null && activity != null)
                    {
                        _viewPager.SetCurrentItem(tabId, true);
                        activity.ShowToolbarActions(tabId);
                    }
                }
            }
            base.OnViewStateRestored(savedInstanceState);

        }

        void _tabLayout_TabUnselected(object sender, TabLayout.TabUnselectedEventArgs e)
		{
			TabLayout.Tab tab = _tabLayout.GetTabAt(e.Tab.Position);
		}

		class TabAdapter : MvxViewPagerFragmentAdapter
		{
			public int[] mTabsIcons =
			{
			Resource.Drawable.ic_settings_black_24dp,
			Resource.Drawable.ic_errorstatus,
			Resource.Drawable.abc_btn_check_material
			};

			private Context _context;

			public TabAdapter(
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
				ImageView icon = (ImageView)layout.FindViewById(Resource.Id.icon);
				icon.SetImageResource(mTabsIcons[position]);

				return tv;
			}
		}
	}
}
