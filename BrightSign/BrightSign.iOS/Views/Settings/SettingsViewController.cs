using System;
using BrightSign.Core.ViewModels;
using BrightSign.iOS.Views.CustomViews;
using BrightSign.iOS.Views.Home.CustomViews;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using UIKit;

namespace BrightSign.iOS.Views.Home
{
    //[MvxModalPresentationAttribute(WrapInNavigationController = true, ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal)]
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Settings", TabIconName = "settings_new")]
    public partial class SettingsViewController : BaseView<SettingsViewModel>, IUITableViewDelegate
    {
        const int ACTION_SECTION = 4;
        const int DEVICE_SECTION = 0;
        UIBarButtonItem resetButton;
        UIBarButtonItem saveButton;

        public SettingsViewController() : base("SettingsViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                Title = "Settings";

                //NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, CancelBarButtonItemAction); ;

                //UIButton infoButton = new UIButton(UIButtonType.InfoLight);
                ////infoButton.AddTarget(this, new ObjCRuntime.Selector("DisplayAppInfo"), UIControlEvent.TouchUpInside);
                //UIBarButtonItem infoButtonItem = new UIBarButtonItem(infoButton);

                //UIBarButtonItem space = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace);
                //space.Width = 25;

                resetButton = new UIBarButtonItem("Reset", UIBarButtonItemStyle.Plain, CancelBarButtonItemAction);


                saveButton = new UIBarButtonItem("Save", UIBarButtonItemStyle.Plain, SaveBarButtonItemAction);

                //UIBarButtonItem[] rightBarButtonItems = new UIBarButtonItem[] { saveButton, space, infoButtonItem };


                //settingsTableView.RegisterNibForCellReuse(BSSwitchTableViewCell.Nib, BSSwitchTableViewCell.Key);
                //settingsTableView.RegisterNibForCellReuse(BSSizeSelectTableViewCell.Nib, BSSizeSelectTableViewCell.Key);


                //CoreGraphics.CGRect r = View.Frame;
                //var source = new SettingsViewTableViewSource(settingsTableView, ViewModel.BSActionList, ViewModel);
                //settingsTableView.Source = source;
                //settingsTableView.SetEditing(true, false);
                //settingsTableView.AllowsSelectionDuringEditing = true;
                ViewModel.PropertyChanged += ViewModel_PropertyChanged;
                //settingsTableView.AllowsSelection = false;

                this.CreateBinding(snapshotLabel).To((SettingsViewModel vm) => vm.SnapshotText).Apply();
                this.CreateBinding(snapshotConfigButton).For(o => o.Hidden).To((SettingsViewModel vm) => vm.IsSnapshotsAvailable).WithConversion("Inverse").Apply();
                this.CreateBinding(snapshotConfigButton).To((SettingsViewModel vm) => vm.RemoteSnapshotsCommand).Apply();
                this.CreateBinding(refreshSwitch).For(o => o.On).To((SettingsViewModel vm) => vm.AutomaticRefresh).Apply();

                deviceInfo.SetContext(ViewModel.CurrentDevice, showButton: true);
                this.CreateBinding(deviceInfo).For(o => o.DataContext).To((SettingsViewModel vm) => vm.CurrentDevice).Apply();
                deviceInfo.SelectDeviceClicked += (arg1, arg2) =>
                {
                    ViewModel.ChangeDeviceCommand.Execute();
                    return null;
                };

                smallButtonView.SetContext(ViewModel.smallButton);
                mediumButtonView.SetContext(ViewModel.mediumButton);
                largeButtonView.SetContext(ViewModel.largeButton);
                this.CreateBinding(smallButtonView).For(o => o.DataContext).To((SettingsViewModel vm) => vm.smallButton).Apply();

                this.CreateBinding(mediumButtonView).For(o => o.DataContext).To((SettingsViewModel vm) => vm.mediumButton).Apply();

                this.CreateBinding(largeButtonView).For(o => o.DataContext).To((SettingsViewModel vm) => vm.largeButton).Apply();

                smallButtonView.AddGestureRecognizer(new UITapGestureRecognizer(() =>
                {
                    if (!ViewModel.smallButton.IsSelected)
                    {
                        ViewModel.smallButton.IsSelected = true;
                        ViewModel.SelectedButtonType = ViewModel.smallButton.ID;
                        ViewModel.mediumButton.IsSelected = false;
                        ViewModel.largeButton.IsSelected = false;
                    }
                }));

                mediumButtonView.AddGestureRecognizer(new UITapGestureRecognizer(() =>
                {
                    if (!ViewModel.mediumButton.IsSelected)
                    {
                        ViewModel.smallButton.IsSelected = false;
                        ViewModel.mediumButton.IsSelected = true;
                        ViewModel.SelectedButtonType = ViewModel.mediumButton.ID;
                        ViewModel.largeButton.IsSelected = false;
                    }
                }));

                largeButtonView.AddGestureRecognizer(new UITapGestureRecognizer(() =>
                {
                    if (!ViewModel.largeButton.IsSelected)
                    {
                        ViewModel.smallButton.IsSelected = false;
                        ViewModel.mediumButton.IsSelected = false;
                        ViewModel.largeButton.IsSelected = true;
                        ViewModel.SelectedButtonType = ViewModel.largeButton.ID;
                    }
                }));



                settingsView.AddGestureRecognizer(new UITapGestureRecognizer(() =>
                {
                    ViewModel.RemoteSnapshotsCommand.Execute();
                }));
            }
            catch (Exception ex)
            {

            }
            // Perform any additional setup after loading the view, typically from a nib.
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ReloadActionSection")
            {
                if (ViewModel.ReloadActionSection)
                {
                    settingsTableView.ReloadSections(new Foundation.NSIndexSet(ACTION_SECTION), UITableViewRowAnimation.None);
                    ViewModel.ReloadActionSection = false;
                }
            }
            else if (e.PropertyName == "ReloadSettings")
            {
                if (ViewModel.ReloadSettings)
                {
                    settingsTableView.ReloadSections(new Foundation.NSIndexSet(DEVICE_SECTION), UITableViewRowAnimation.None);
                    ViewModel.ReloadSettings = false;
                }
            }
            else if (e.PropertyName == "IsDataModified")
            {
                if (ViewModel.IsDataModified)
                {
                    NavigationItem.LeftBarButtonItem = resetButton;
                    NavigationItem.RightBarButtonItem = saveButton;
                }
                else
                {
                    NavigationItem.RightBarButtonItem = null;
                    NavigationItem.LeftBarButtonItem = base.homeButton;
                }
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }


        private void DisplayAppInfo()
        {
        }

        private void SaveBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.SaveCommand.Execute();
        }

        private void CancelBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.CancelCommand.Execute();
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            deviceInfo.SetNeedsDisplay();
            RefreshNavigationBar();
        }
    }
}

