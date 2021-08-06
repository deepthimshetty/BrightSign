using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BrightSign.Core.Models;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform.Core;
using UIKit;

namespace BrightSign.iOS.Views.ManageActions
{
    public class ActionsDraggableCollectionViewSource : MvxCollectionViewSource
    {
        public UICollectionView CollectionView { get; set; }


        /// <summary>
        /// The items source.
        /// </summary>
        private ObservableCollection<BSUdpAction> _itemsSource;
        /// <summary>
        /// Gets or sets the list items source.
        /// </summary>
        /// <value>The list items source.</value>
        public ObservableCollection<BSUdpAction> ListItemsSource
        {
            get
            {
                return _itemsSource;
            }

            set
            {
                _itemsSource = value;
                ReloadData();
            }
        }

        public ActionsDraggableCollectionViewSource(UICollectionView collectionView, NSString defaultCellIdentifier) : base(collectionView, defaultCellIdentifier)
        {
            // Initialize
            CollectionView = collectionView;


        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            if (ListItemsSource == null)
            {
                return null;
            }

            return ListItemsSource[indexPath.Row];
        }





        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get a reusable cell and set {~~it's~>its~~} title from the item
            //var cell = collectionView.DequeueReusableCell("Cell", indexPath) as TextCollectionViewCell;
            //cell.Title = Numbers[(int)indexPath.Item].ToString();

            //return cell


            var item = GetItemAt(indexPath);
            var cell = (ActionViewCell)collectionView.DequeueReusableCell(ActionViewCell.Key, indexPath);

            var bindable = cell as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = item;
            return cell;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return ListItemsSource.Count;
        }

        public override bool CanMoveItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // We can always move items
            return false;
        }

        public override void MoveItem(UICollectionView collectionView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        {
            // Reorder our list of items
            var item = ListItemsSource[(int)sourceIndexPath.Item];
            ListItemsSource.RemoveAt((int)sourceIndexPath.Item);
            ListItemsSource.Insert((int)destinationIndexPath.Item, item);
        }
    }
}
