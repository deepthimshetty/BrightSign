using System;
using BrightSign.Core.ViewModels;
using BrightSign.Localization;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using UIKit;

namespace BrightSign.iOS.Views.Settings
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class DeviceSelectController : MvxViewController
    {
        public DeviceSelectController() : base("DeviceSelectController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            //Title = ViewModel.ViewTitle;

            //Left Barbutton Items
            UIBarButtonItem backButton = new UIBarButtonItem(Strings.cancel, UIBarButtonItemStyle.Plain, BackBarButtonItemAction);
            UIBarButtonItem refreshButton = new UIBarButtonItem(UIBarButtonSystemItem.Refresh, RefreshBarButtonItemAction);

            UIBarButtonItem[] leftBarButtonItems = new UIBarButtonItem[] { backButton, refreshButton };
            NavigationItem.LeftBarButtonItems = leftBarButtonItems;



            //Right Barbutton Items
            UIBarButtonItem saveButton = new UIBarButtonItem(UIBarButtonSystemItem.Save, SaveBarButtonItemAction);

        }

        private void SaveBarButtonItemAction(object sender, EventArgs e)
        {

        }

        private void RefreshBarButtonItemAction(object sender, EventArgs e)
        {

        }

        private void BackBarButtonItemAction(object sender, EventArgs e)
        {

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

