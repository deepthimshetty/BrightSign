using System;
using Acr.UserDialogs;
using BrightSign.Core.ViewModels.SearchUnits;
using BrightSign.Core.ViewModels.Units;
using BrightSign.iOS.Utility;
using BrightSign.iOS.Views.CustomViews;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using UIKit;

namespace BrightSign.iOS.Views.Units
{
    //[MvxModalPresentation(WrapInNavigationController = true)]
    public partial class UnitsViewController : BaseView<UnitsViewModel>
    {
        public UnitsViewController() : base("UnitsViewController", null, false)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            if (TabBarController != null && TabBarController.TabBar != null)
            {
                TabBarController.TabBar.Hidden = true;
            }


            //Right Bar Button Item
            UIBarButtonItem refreshButton = new UIBarButtonItem(UIImage.FromBundle("refresh.png"), UIBarButtonItemStyle.Plain, RefreshBarButtonItemAction);
            NavigationItem.RightBarButtonItem = refreshButton;


            devicesTableView.SeparatorColor = UIColor.Clear;
            devicesTableView.RegisterNibForCellReuse(DeviceTableViewCell.Nib, DeviceTableViewCell.Key);
            var source = new MvxDeleteStandardTableViewSource(ViewModel, devicesTableView, new Foundation.NSString("DeviceTableViewCell"));
            this.CreateBinding(source).For(o => o.ItemsSource).To((UnitsViewModel vm) => vm.deviceList).Apply();
            this.CreateBinding(source).For(s => s.SelectionChangedCommand).To((UnitsViewModel vm) => vm.ItemClickCommand).Apply();
            devicesTableView.Source = source;
            devicesTableView.EstimatedRowHeight = 80;

            this.CreateBinding(addDeviceButton).For(o => o.Hidden).To((UnitsViewModel vm) => vm.AddButtonVisible).WithConversion("Inverse").Apply();
            this.CreateBinding(addDeviceButton).To((UnitsViewModel vm) => vm.AddDeviceCommand).Apply();

            activeTab.SetContext(ViewModel.firsttabItem);
            offlineTab.SetContext(ViewModel.secondtabItem);

            activeTab.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                ViewModel.secondtabItem.IsSelected = false;
                ViewModel.firsttabItem.IsSelected = true;
                ViewModel.TabChange();
            }));

            offlineTab.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                ViewModel.secondtabItem.IsSelected = true;
                ViewModel.firsttabItem.IsSelected = false;
                ViewModel.TabChange();
            }));
        }

        private void RefreshBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.RefreshCommand.Execute();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            //UserDialogs.Init(dummyfunction);
        }

        private UIViewController dummyfunction()
        {
            return this;
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            gradientView.SetNeedsDisplay();
            RefreshNavigationBar();
        }
    }
}

