using System;
using BrightSign.Core.ViewModels.SearchUnits;
using BrightSign.iOS.Views.CustomViews;
using Foundation;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using UIKit;

namespace BrightSign.iOS.Views.SearchUnits
{
    //[MvxModalPresentation(WrapInNavigationController = true)]
    public partial class SearchUnitsView : MvxViewController<SearchUnitsViewModel>
    {
        public SearchUnitsView() : base("SearchUnitsView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            NavigationController.NavigationBarHidden = true;

            activityIndicator.StartAnimating();
            if (TabBarController!= null && TabBarController.TabBar != null)
            {
                TabBarController.TabBar.Hidden = true;
            }
            var versionString = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"];
            var bundleVersion = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"];
            versionLabel.Text = "version " + versionString.Description + string.Format(" ({0})", bundleVersion.Description);

            if (UIDevice.CurrentDevice.Orientation.IsLandscape())
            {
                //Add Landscape Image
                BGImageView.Image = UIImage.FromBundle("searchunits.png");
            }
            else{
                //Add Potrait Image
                BGImageView.Image = UIImage.FromBundle("searchunits.png");
            }

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);

            if (UIDevice.CurrentDevice.Orientation.IsLandscape())
            {
                //Add Landscape Image
                BGImageView.Image = UIImage.FromBundle("searchunits.png");
            }
            else
            {
                //Add Potrait Image
                BGImageView.Image = UIImage.FromBundle("searchunits.png");
            }
        }
    }
}

