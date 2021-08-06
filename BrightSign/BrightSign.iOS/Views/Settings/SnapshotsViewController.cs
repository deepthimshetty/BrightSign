using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.ViewModels;
using BrightSign.iOS.Views.CustomViews;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using UIKit;

namespace BrightSign.iOS.Views.Settings
{
    //[MvxModalPresentationAttribute(WrapInNavigationController = true)]
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Gallery", TabIconName = "gallery_new")]
    public partial class SnapshotsViewController : BaseView<SnapshotsViewModel>, IUICollectionViewDelegateFlowLayout
    {


        public SnapshotsViewController() : base("SnapshotsViewController", null)
        {
        }

        public ObservableCollection<BSSnapshot> sourceArry;
        public bool isVertical = false;
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                //NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, CancelBButtonItemAction); ;

                //snapshotsCollectnVw.RegisterNibForCell(SnapShotCollectionViewCell.Nib, SnapShotCollectionViewCell.Key);
                //snapshotsCollectnVw.Source = new SnapshotCollectionViewSource(snapshotsCollectnVw);


                UIButton leftrotateButton = UIButton.FromType(UIButtonType.Custom);
            leftrotateButton.SetImage(UIImage.FromFile("baseline_rotate_left_white.png"), UIControlState.Normal);
            leftrotateButton.Frame = new CGRect(0, 0, 30, 30);
            leftrotateButton.TouchUpInside -= LeftToolBarButton_Clicked;

            leftrotateButton.TouchUpInside += LeftToolBarButton_Clicked;
                UIBarButtonItem leftToolBarButton = new UIBarButtonItem(leftrotateButton);
           
                UIButton rightrotateButton = UIButton.FromType(UIButtonType.Custom);
                rightrotateButton.SetImage(UIImage.FromFile("baseline_rotate_right_white.png"), UIControlState.Normal);
                rightrotateButton.Frame = new CGRect(0, 0, 30, 30);
                rightrotateButton.TouchUpInside -= RightToolbarButton_Clicked;

                rightrotateButton.TouchUpInside += RightToolbarButton_Clicked;
                UIBarButtonItem rightToolbarButton = new UIBarButtonItem(rightrotateButton);

                //UIBarButtonItem leftToolBarButton = new UIBarButtonItem(UIImage.FromFile("baseline_rotate_left_white.png"), UIBarButtonItemStyle.Plain, LeftToolBarButton_Clicked);
                //leftToolBarButton.ImageInsets = new UIEdgeInsets(0, 45, 0, 0);
                //UIBarButtonItem rightToolbarButton = new UIBarButtonItem(UIImage.FromFile("baseline_rotate_right_white.png"), UIBarButtonItemStyle.Plain, RightToolbarButton_Clicked);
                //rightToolbarButton.ImageInsets = new UIEdgeInsets(0, 0, 0, 0);

                UIBarButtonItem[] items = new UIBarButtonItem[2] { rightToolbarButton, leftToolBarButton };
                NavigationItem.RightBarButtonItems = items;

                isVertical = Constants.SnapshotConfig.DisplayPortraitMode;

                snapshotsCollectnVw.RegisterNibForCell(UINib.FromName("SnapShotCollectionViewCell", NSBundle.MainBundle), "SnapShotCollectionViewCell");
                var source = new MvxCollectionViewSource(snapshotsCollectnVw, new NSString("SnapShotCollectionViewCell"));
                this.CreateBinding(source).For(o => o.ItemsSource).To((SnapshotsViewModel vm) => vm.SnapshotsItemSource).Apply();
                //this.CreateBinding(source).For(o => o.SelectionChangedCommand).To((SnapshotsViewModel vm) => vm.ItemSelected).Apply();
                snapshotsCollectnVw.Source = source;
                snapshotsCollectnVw.Delegate = this;

                //leftToolBarButton.Clicked += LeftToolBarButton_Clicked;
                //rightToolbarButton.Clicked += RightToolbarButton_Clicked;

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
                Debug.WriteLine("Exception in snapshotsVC", ex);
            }

            // Perform any additional setup after loading the view, typically from a nib.
        }
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        [Export("collectionView:layout:minimumInteritemSpacingForSectionAtIndex:")]         public virtual float GetMinimumInteritemSpacingForSection(UICollectionView view, UICollectionViewLayout layout, int section)         {
            if (isVertical)
            {
                return 0;
            }else
            {
                return 2;
            }          }          [Export("collectionView:layout:minimumLineSpacingForSectionAtIndex:")]         public virtual float GetMinimumLineSpacingForSection(UICollectionView view, UICollectionViewLayout layout, int section)         {
                return 2;
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
            //CGRect screenSize = UIScreen.MainScreen.Bounds;
            //nfloat screenWidth = screenSize.Size.Width;
            //if(isHorizontal == true)
            //{
            //    float cellWidth = (float)(screenWidth / 2.0);
            //    return new CGSize(cellWidth, 100);
            //}else{
            //    float cellWidth = (float)(screenWidth / 3.0);
            //    return new CGSize(cellWidth, 200);
            //}
           
            CGSize size = new CGSize(160, 100);

           
            //if (Constants.SnapshotConfig.DisplayPortraitMode)
            //{
            //    size = new CGSize(100, 160);
            //}

            //if (Constants.SnapshotConfig.ResY > 0)
            //{
            //    double aspectScale = Constants.SnapshotConfig.ResX / Constants.SnapshotConfig.ResY;
            //    double viewWidth = (double)collectionView.Frame.Size.Width;
            //    double width = aspectScale > 1.0 ? viewWidth / 2 - 1 : viewWidth / 3 - 2;
            //    size = new CGSize(width, width / aspectScale);
            //}


            if (Constants.SnapshotConfig.ResY > 0)             {                 double aspectScale = Constants.SnapshotConfig.ResX / Constants.SnapshotConfig.ResY;                 double viewWidth = (double)collectionView.Frame.Size.Width;                 //double width = aspectScale > 1.0 ? viewWidth / 2 - 1 : viewWidth / 3 - 2;                  double width = (viewWidth) / 2 ;                   if (isVertical)                 {                     size = new CGSize( width-5,width+10);                 }                 else{                     size = new CGSize(width-5, width / aspectScale);                     }              }


            return size;
        }
        /// <summary>
        /// Items the selected.
        /// </summary>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="indexPath">Index path.</param>
        [Export("collectionView:didSelectItemAtIndexPath:")]
        public void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            ViewModel.ItemSelected.Execute(ViewModel.SnapshotsItemSource[indexPath.Row]);
        }

        private void CancelBButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.CancelCommand.Execute();
        }

        void LeftToolBarButton_Clicked(object sender, EventArgs e)
        {
            //sourceArry = ViewModel.SnapshotsItemSource;
            //isHorizontal = !isHorizontal;
            //for (int i = 0; i < sourceArry.Count; i++)
            //{
            //    BSSnapshot snap = sourceArry[i];
            //    float currentTransform = snap.ImageTransform;
            //    snap.ImageTransform = currentTransform - (float)(Math.PI / 2);
            //}
            //double temp = Constants.SnapshotConfig.ResX;
            //Constants.SnapshotConfig.ResX = Constants.SnapshotConfig.ResY;
            //Constants.SnapshotConfig.ResY = temp;
            //snapshotsCollectnVw.ReloadData();

            isVertical = !isVertical;
            foreach (var item in ViewModel.SnapshotsItemSource)
            {
                item.ImageTransform -= (float)(Math.PI / 2);
            }

            snapshotsCollectnVw.ReloadData();

        }

        void RightToolbarButton_Clicked(object sender, EventArgs e)
        {
            //sourceArry = ViewModel.SnapshotsItemSource;
            //isHorizontal = !isHorizontal;
            //for (int i = 0; i < sourceArry.Count; i++)
            //{
            //    BSSnapshot snap = sourceArry[i];
            //    float currentTransform = snap.ImageTransform;
            //    snap.ImageTransform = currentTransform + (float)(Math.PI / 2);
            //}
            //double temp = Constants.SnapshotConfig.ResX;
            //Constants.SnapshotConfig.ResX = Constants.SnapshotConfig.ResY;
            //Constants.SnapshotConfig.ResY = temp;
            //snapshotsCollectnVw.ReloadData();
            isVertical = !isVertical;
            foreach (var item in ViewModel.SnapshotsItemSource)
            {
                item.ImageTransform += (float)(Math.PI / 2);
            }
            snapshotsCollectnVw.ReloadData();

        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            deviceInfoView.SetNeedsDisplay();
            RefreshNavigationBar();
        }

    }
}

