using System;
using System.Diagnostics;
using BrightSign.Core.ViewModels;
using BrightSign.iOS.Views.CustomViews;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using UIKit;

namespace BrightSign.iOS.Views.Settings
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class RemoteSnapshotViewController : BaseView<RemoteSnapshotViewModel>
    {
        public RemoteSnapshotViewController() : base("RemoteSnapshotViewController", null,false)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                var backButton = new UIBarButtonItem(UIImage.FromBundle("ic_chevron_left_white.png"), UIBarButtonItemStyle.Plain, CancelBarButtonItemAction);
                NavigationItem.LeftBarButtonItem = backButton;

                UIBarButtonItem saveButton = new UIBarButtonItem(UIBarButtonSystemItem.Save, SaveBarButtonItemAction);

                UIBarButtonItem[] rightBarButtonItems = new UIBarButtonItem[] { saveButton };

                NavigationItem.RightBarButtonItems = rightBarButtonItems;
                enableRSVw.Layer.BorderWidth = 1;
                enableRSVw.Layer.BorderColor = UIColor.FromRGB(224, 224, 224).CGColor;
                snapshotFrquncyVw.Layer.BorderWidth = 1;
                snapshotFrquncyVw.Layer.BorderColor = UIColor.FromRGB(224, 224, 224).CGColor;
                noOfSnapshotsVw.Layer.BorderWidth = 1;
                noOfSnapshotsVw.Layer.BorderColor = UIColor.FromRGB(224, 224, 224).CGColor;

                jpegQultyVw.Layer.BorderWidth = 1;
                jpegQultyVw.Layer.BorderColor = UIColor.FromRGB(224, 224, 224).CGColor;
                portatModeDisplyVw.Layer.BorderWidth = 1;
                portatModeDisplyVw.Layer.BorderColor = UIColor.FromRGB(224, 224, 224).CGColor;

                this.CreateBinding(enableRemotSnpShtSwitch).For(o => o.On).To((RemoteSnapshotViewModel vm) => vm.snapshotconfig.Enabled).Apply();
                this.CreateBinding(portraitModDisplySwitch).For(o => o.On).To((RemoteSnapshotViewModel vm) => vm.snapshotconfig.DisplayPortraitMode).Apply();
                this.CreateBinding(noOfSnapshotsValLbl).To((RemoteSnapshotViewModel vm) => vm.snapshotconfig.MaxImages).Apply();
                this.CreateBinding(noOfSnapshotsSlider).To((RemoteSnapshotViewModel vm) => vm.snapshotconfig.MaxImages).Apply();
                this.CreateBinding(snapShotFrqncyVal).To((RemoteSnapshotViewModel vm) => vm.snapshotconfig.Interval).Apply();
                this.CreateBinding(snapShotFreqncySlider).To((RemoteSnapshotViewModel vm) => vm.snapshotconfig.Interval).Apply();
                this.CreateBinding(jpegQultyValLbl).To((RemoteSnapshotViewModel vm) => vm.snapshotconfig.Quality).Apply();
                this.CreateBinding(jpegQultySlider).To((RemoteSnapshotViewModel vm) => vm.snapshotconfig.Quality).Apply();

                deviceInfoView.SetContext(ViewModel.CurrentDevice);
                this.CreateBinding(deviceInfoView).For(o => o.DataContext).To((VariablesViewModel vm) => vm.CurrentDevice).Apply();
                deviceInfoView.SelectDeviceClicked += (arg1, arg2) =>
                {
                    ViewModel.ChangeDeviceCommand.Execute();
                    return null;
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in remoteVC", ex);
            }

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
        private void SaveBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.SaveRSCommand.Execute();
        }

        private void CancelBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.CancelRSCommand.Execute();
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            deviceInfoView.SetNeedsDisplay();
            RefreshNavigationBar();
        }
    }
}

