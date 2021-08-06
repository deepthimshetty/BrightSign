using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace BrightSign.iOS.Views.CustomViews
{
    public class SnapshotCollectionViewSource:MvxCollectionViewSource
    {
        public SnapshotCollectionViewSource(UICollectionView collectionView) : base(collectionView)
        {
            collectionView.RegisterNibForCell(SnapShotCollectionViewCell.Nib, SnapShotCollectionViewCell.Key);
            collectionView.CollectionViewLayout = new UICollectionViewFlowLayout
            {
                MinimumInteritemSpacing = 2,
                MinimumLineSpacing = 2,
                //ItemSize = new CGSize(width, 30),
                SectionInset = new UIEdgeInsets(10, 5, 0, 0),
            };

        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return 10;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (SnapShotCollectionViewCell)collectionView.DequeueReusableCell(SnapShotCollectionViewCell.Key, indexPath);
            cell.BackgroundColor = UIColor.FromRGB(216, 235, 254);
            cell.snapshotIndex = indexPath.Row;
            return cell;
        }
       
        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            CGSize size = new CGSize(160, 120);
            if(Constants.SnapshotConfig.ResY > 0) //ResY = height
            {
                double aspectScale = Constants.SnapshotConfig.ResX / Constants.SnapshotConfig.ResY;
                //if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPad) {
                //    size = CGSizeMake(kDefaultSnapshotThumbnailImageSize.height * aspectScale, kDefaultSnapshotThumbnailImageSize.height);
                //}
        //else {
                nfloat viewWidth = collectionView.Frame.Size.Width;
                nfloat width = aspectScale > 1.0 ? viewWidth / 2 - 1 : viewWidth / 3 - 2;
                size = new CGSize(width, width / aspectScale);
                //}
            }

            return size;
        }
        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
           
        }

        public override bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return true;
        }
       
    }
}
