using System;
using System.ComponentModel;
using BrightSign.Core.ViewModels.AddDevice;
using BrightSign.iOS.Utility;
using BrightSign.iOS.Views.CustomViews;
using BrightSign.Localization;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using UIKit;

namespace BrightSign.iOS.Views.AddDevice
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class AddDeviceView : BaseView<AddDeviceViewModel>
    {
        public AddDeviceView() : base("AddDeviceView", null,false)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            //TabBarController.TabBar.Hidden = true;

            UIBarButtonItem closeButton = new UIBarButtonItem(UIImage.FromBundle("close.png"), UIBarButtonItemStyle.Plain, CancelBarButtonItemAction);

            NavigationItem.LeftBarButtonItem = closeButton;


            this.CreateBinding(descriptionLabel).To((AddDeviceViewModel vm) => vm.Description).Apply();

            this.CreateBinding(searchBtn).To((AddDeviceViewModel vm) => vm.SearchCommand).Apply();
            this.CreateBinding(ipAddressText).To((AddDeviceViewModel vm) => vm.IPAddress).Apply();
            //this.CreateBinding(addBtn).For(o => o.Hidden).To((AddDeviceViewModel vm) => vm.AddButtonVisibility).WithConversion("Inverse").Apply();
            this.CreateBinding(detailsView).For(o => o.Hidden).To((AddDeviceViewModel vm) => vm.AddButtonVisibility).WithConversion("Inverse").Apply();

            ipAddressText.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                ViewModel.SearchCommand.Execute();
                return true;
            };


            searchBtn.Layer.CornerRadius = 5;
            searchBtn.Layer.BorderColor = UIColorUtility.FromHex("F66A3B").CGColor;
            searchBtn.Layer.BorderWidth = 1;

            deviceInfoView.SetContext(ViewModel.bsdeviceAdd);
            this.CreateBinding(deviceInfoView).For(o => o.DataContext).To((AddDeviceViewModel vm) => vm.bsdeviceAdd).Apply();

            detailsView.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                ViewModel.AddCommand.Execute();
            }));
        }


        private void CancelBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.CancelCommand.Execute();
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            deviceInfoView.SetNeedsDisplay();
            RefreshNavigationBar();
        }
    }
}

