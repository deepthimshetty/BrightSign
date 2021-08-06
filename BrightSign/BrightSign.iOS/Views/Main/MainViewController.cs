using System;
using Acr.UserDialogs;
using BrightSign.Core.Utility;
using BrightSign.Core.ViewModels;
using BrightSign.iOS.Utility;
using BrightSign.iOS.Views.Home;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using MvvmCross.Platform;
using UIKit;

namespace BrightSign.iOS.Views
{
    [MvxRootPresentation(WrapInNavigationController = false)]
    public partial class MainViewController : MvxTabBarViewController<MainViewModel>
    {
        private bool _firstTimePresented = true;

        public MainViewController()
        {
            base.ViewDidLoad();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (_firstTimePresented)
            {
                _firstTimePresented = false;
                (ViewModel).ShowVariablesViewModelCommand.Execute(null);
                (ViewModel).ShowActionsViewModelCommand.Execute(null);
                (ViewModel).ShowDiagnosticsViewModelCommand.Execute(null);
                (ViewModel).ShowGalleryViewModelCommand.Execute(null);
                (ViewModel).ShowSettingsViewModelCommand.Execute(null);
            }

            //var tab = UIView.AppearanceWhenContainedIn(typeof(UITabBar));
            //tab.TintColor = UIColor.Orange;

            UITabBar.Appearance.SelectedImageTintColor = UIColorUtility.FromHex(ColorConstants.EndColor);

            UITabBar.Appearance.BackgroundColor = UIColor.White;

            UITabBar.Appearance.BarTintColor = UIColor.White;

            //UITabBar.Appearance.TintColor = UIColor.Orange;
           

        }

    }
}