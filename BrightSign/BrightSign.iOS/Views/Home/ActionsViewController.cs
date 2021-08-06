using System;
using System.Collections.Generic;
using System.Linq;
using BrightSign.Core.Utility;
using BrightSign.Core.ViewModels;
using BrightSign.iOS.Views.CustomViews;
using BrightSign.iOS.Views.ManageActions;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using UIKit;

namespace BrightSign.iOS.Views.Home
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Actions", TabIconName = "actions_new")]
    public partial class ActionsViewController : BaseView<ActionsViewModel>, IUICollectionViewDelegateFlowLayout
    {
        public ActionsViewController() : base("ActionsViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                Title = ViewModel.ViewTitle;

                //UIBarButtonItem refreshButton = new UIBarButtonItem(UIBarButtonSystemItem.Refresh, RefreshBarButtonItemAction);
                //NavigationItem.LeftBarButtonItem = refreshButton;

                //UIBarButtonItem snapshotsButton = new UIBarButtonItem(UIImage.FromFile("86-camera.png"), UIBarButtonItemStyle.Plain, SnapshotBarButtonItemAction);
                //UIBarButtonItem settingsButton = new UIBarButtonItem(UIImage.FromFile("19-gear.png"), UIBarButtonItemStyle.Plain, SettingsBarButtonItemAction);

                //UIBarButtonItem[] items = new UIBarButtonItem[2] { settingsButton, snapshotsButton };
                //NavigationItem.RightBarButtonItems = items;

                //UIBarButtonItem refreshButton = new UIBarButtonItem(UIBarButtonSystemItem.Refresh, RefreshBarButtonItemAction);
                //NavigationItem.LeftBarButtonItem = refreshButton;

                UIBarButtonItem EditButton = new UIBarButtonItem("Edit", UIBarButtonItemStyle.Plain, EditBarButtonItemAction);
                NavigationItem.RightBarButtonItem = EditButton;


                collectionView.RegisterNibForCell(ActionViewCell.Nib, ActionViewCell.Key);
                //collectionView.Source = new ActionViewCollectionViewSource(collectionView, ActionTypes.Small, ViewModel);

                var source = new ActionsDraggableCollectionViewSource(collectionView, new Foundation.NSString("ActionViewCell"));
                this.CreateBinding(source).For(o => o.ListItemsSource).To((ActionsViewModel vm) => vm.ActionsItemSource).Apply();
                collectionView.Source = source;
                collectionView.Delegate = this;

                ViewModel.PropertyChanged += ViewModel_PropertyChanged;

                deviceInfoView.SetContext(ViewModel.CurrentDevice);
                this.CreateBinding(deviceInfoView).For(o => o.DataContext).To((ActionsViewModel vm) => vm.CurrentDevice).Apply();
                deviceInfoView.SelectDeviceClicked += (arg1, arg2) =>
                {
                    ViewModel.ChangeDeviceCommand.Execute();
                    return null;
                };

                this.CreateBinding(segmentedControl).For(o => o.SelectedSegment).To((ActionsViewModel vm) => vm.SelectedTabIndex).Apply();



                // Create a custom gesture recognizer
                var longPressGesture = new UILongPressGestureRecognizer((gesture) => {

                    // Take action based on state
                    switch (gesture.State)
                    {
                        case UIGestureRecognizerState.Began:
                            var selectedIndexPath = collectionView.IndexPathForItemAtPoint(gesture.LocationInView(collectionView));
                            if (selectedIndexPath != null)
                            {
                                collectionView.BeginInteractiveMovementForItem(selectedIndexPath);
                            }
                            break;
                        case UIGestureRecognizerState.Changed:
                            collectionView.UpdateInteractiveMovement(gesture.LocationInView(collectionView));
                            break;
                        case UIGestureRecognizerState.Ended:
                            collectionView.EndInteractiveMovement();
                            break;
                        default:
                            collectionView.CancelInteractiveMovement();
                            break;
                    }

                });

                // Add the custom recognizer to the collection view
                collectionView.AddGestureRecognizer(longPressGesture);
            }
            catch (Exception ex)
            {

            }
            // Perform any additional setup after loading the view, typically from a nib.
        }

        private void EditBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.EditActionsCommand.Execute();
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ReloadActionSection")
            {
                if (ViewModel.ReloadActionSection)
                {
                    collectionView.ReloadData();
                    ViewModel.ReloadActionSection = false;
                }
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            TabBarController.TabBar.Hidden = false;
        }

        /// <summary>
        /// Gets the size for item.
        /// </summary>
        /// <returns>The size for item.</returns>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="layout">Layout.</param>
        /// <param name="indexPath">Index path.</param>
        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public virtual CoreGraphics.CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var buttonType = ViewModel.SelectedButtonType;
            var bounds = UIScreen.MainScreen.Bounds;
            if (buttonType == 0)
            {
                return new CGSize((bounds.Width - 20) / 3, 80);
            }
            else if (buttonType == 1)
            {
                return new CGSize((bounds.Width - (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? 140 : 60)), 80);
            }
            else if (buttonType == 2)
            {
                return new CGSize((bounds.Width - 20), 80);
            }
            return new CGSize((bounds.Width - 20) / 3, 80);

        }

        /// <summary>
        /// Items the selected.
        /// </summary>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="indexPath">Index path.</param>
        [Export("collectionView:didSelectItemAtIndexPath:")]
        public void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            ViewModel.UDPCommand.Execute(ViewModel.ActionsItemSource[indexPath.Row]);
            collectionView.DeselectItem(indexPath, false);
        }


        #region Overrides Methods
        [Export("collectionView:shouldHighlightItemAtIndexPath:")]
        public bool ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Always allow for highlighting
            return true;
        }

        [Export("collectionView:didHighlightItemAtIndexPath:")]
        public void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get cell and change to green background
            var cell = collectionView.CellForItem(indexPath);
            //cell.ContentView.BackgroundColor = UIColor.FromRGB(183, 208, 57);
        }

        [Export("collectionView:didUnhighlightItemAtIndexPath:")]
        public void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get cell and return to blue background
            var cell = collectionView.CellForItem(indexPath);
            //cell.ContentView.BackgroundColor = UIColor.FromRGB(164, 205, 255);
        }
        #endregion


        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            deviceInfoView.SetNeedsDisplay();
            RefreshNavigationBar();
        }
    }
}

