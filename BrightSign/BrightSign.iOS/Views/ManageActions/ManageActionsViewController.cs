using System;
using BrightSign.Core.ViewModels;
using BrightSign.iOS.Utility;
using BrightSign.iOS.Views.CustomViews;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using UIKit;

namespace BrightSign.iOS.Views.ManageActions
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class ManageActionsViewController : BaseView<ManageActionsViewModel>
    {
        UIBarButtonItem cancelButton;

        private NSObject _keyboardUp;
        private NSObject _keyboardDown;


        public ManageActionsViewController() : base("ManageActionsViewController", null, false)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            cancelButton = new UIBarButtonItem(UIImage.FromBundle("close.png"), UIBarButtonItemStyle.Plain, CancelBarButtonItemAction);
            NavigationItem.LeftBarButtonItem = cancelButton;

            this.CreateBinding(segmentedControl).For(o => o.SelectedSegment).To((ManageActionsViewModel vm) => vm.SelectedTabIndex).Apply();


            actionsTableView.RegisterNibForCellReuse(ManageActionTableViewCell.Nib, ManageActionTableViewCell.Key);
            //actionsTableView.SeparatorColor = UIColor.Clear;
            var source = new MvxEditDeleteStandardTableViewSource(ViewModel, actionsTableView, new Foundation.NSString("ManageActionTableViewCell"));
            this.CreateBinding(source).For(o => o.ItemsSource).To((ManageActionsViewModel vm) => vm.ActionsItemSource).Apply();
            this.CreateBinding(source).For(o => o.ListItemsSource).To((ManageActionsViewModel vm) => vm.ActionsItemSource).Apply();

            actionsTableView.Source = source;
            actionsTableView.EstimatedRowHeight = 80;
            UIView footerView = new UIView(new CoreGraphics.CGRect(0, 0, actionsTableView.Frame.Size.Width, 1));
            footerView.BackgroundColor = actionsTableView.SeparatorColor;
            actionsTableView.TableFooterView = footerView;
            actionsTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
            actionsTableView.SetEditing(true, true);
            deviceInfoView.SetContext(ViewModel.CurrentDevice);
            this.CreateBinding(deviceInfoView).For(o => o.DataContext).To((ManageActionsViewModel vm) => vm.CurrentDevice).Apply();
            deviceInfoView.SelectDeviceClicked += (arg1, arg2) =>
            {
                ViewModel.ChangeDeviceCommand.Execute();
                return null;
            };

            this.CreateBinding(addBtn).To((ManageActionsViewModel vm) => vm.AddActionCommand).Apply();
            this.CreateBinding(addBtn).For(o => o.Hidden).To((ManageActionsViewModel vm) => vm.IsAddButtonVisible).WithConversion("Inverse").Apply();
            this.CreateBinding(cancleBtn).To((ManageActionsViewModel vm) => vm.CancelEditCommand).Apply();
            this.CreateBinding(updateBtn).To((ManageActionsViewModel vm) => vm.UpdateCommand).Apply();
            this.CreateBinding(updateBtn).For("Title").To((ManageActionsViewModel vm) => vm.UpdateButtonName).Apply();
            this.CreateBinding(editingView).For(o => o.Hidden).To((ManageActionsViewModel vm) => vm.IsEditViewVisible).WithConversion("Inverse").Apply();
            this.CreateBinding(title).To((ManageActionsViewModel vm) => vm.EditTitle).Apply();
            this.CreateBinding(messsage).To((ManageActionsViewModel vm) => vm.EditMessage).Apply();
            this.CreateBinding(actionNameLabel).To((ManageActionsViewModel vm) => vm.EditButtonLabel).Apply();
            this.CreateBinding(actionUDPLabel).To((ManageActionsViewModel vm) => vm.EditButtonUDPText).Apply();
            this.CreateBinding(actionUDPLabel).For(o => o.Enabled).To((ManageActionsViewModel vm) => vm.IsUDPTextEditable).Apply();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;


            actionNameLabel.ShouldReturn += (textField) =>
            {
                if (ViewModel.IsUDPTextEditable)
                {
                    actionUDPLabel.BecomeFirstResponder();
                    return true;
                }
                else
                {
                    actionNameLabel.ResignFirstResponder();
                    return true;
                }

            };


            actionUDPLabel.ShouldReturn += (textField) =>
            {
                actionUDPLabel.ResignFirstResponder();
                return true;
            };

            cancleBtn.TouchUpInside += (object sender, EventArgs e) =>
            {
                try
                {
                    actionNameLabel.ResignFirstResponder();
                    actionUDPLabel.ResignFirstResponder();
                }
                catch (Exception ex)
                {

                }

            };
            updateBtn.TouchUpInside += (object sender, EventArgs e) =>
            {
                try
                {
                    actionNameLabel.ResignFirstResponder();
                    actionUDPLabel.ResignFirstResponder();
                }
                catch (Exception ex)
                {

                }
            };
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEditViewVisible")
            {
                if (ViewModel.IsEditViewVisible)
                {
                    NavigationItem.LeftBarButtonItem = null;
                }
                else
                {
                    NavigationItem.LeftBarButtonItem = cancelButton;
                }
            }
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


        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _keyboardUp = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyBoardShow);
            _keyboardDown = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardHide);
        }

        /// <summary>
        /// Ons the key board show.
        /// </summary>
        /// <param name="notification">Notification.</param>
        public void OnKeyBoardShow(NSNotification notification)
        {
            editingVIewConstraint.Constant = KEYBOARD_HEIGHT;
        }


        /// <summary>
        /// Ons the keyboard hide.
        /// </summary>
        /// <param name="notification">Notification.</param>
        public void OnKeyboardHide(NSNotification notification)
        {
            editingVIewConstraint.Constant = 0;
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            deviceInfoView.SetNeedsDisplay();
            RefreshNavigationBar();
        }
    }
}

