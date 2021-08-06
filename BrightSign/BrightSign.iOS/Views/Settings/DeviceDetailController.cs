using System;

using UIKit;

namespace BrightSign.iOS.Views.Settings
{
    public partial class DeviceDetailController : UIViewController
    {
        public DeviceDetailController() : base("DeviceDetailController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

