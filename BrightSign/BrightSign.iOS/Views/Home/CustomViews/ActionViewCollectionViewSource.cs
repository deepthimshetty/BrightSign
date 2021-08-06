using System;
using UIKit;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using System.Collections.Generic;
using System.Collections;
using BrightSign.Core.Utility;
using CoreGraphics;
using BrightSign.Core.Models;
using BrightSign.Core.ViewModels;
using System.Linq;

namespace BrightSign.iOS
{
    public class ActionViewCollectionViewSource : MvxCollectionViewSource
    {

        List<IList> multiColumnData = new List<IList>();
        ActionTypes listType;
        ActionsViewModel vm;
        public ActionViewCollectionViewSource(UICollectionView collectionView, ActionTypes _listType, ActionsViewModel model) : base(collectionView)
        {
            collectionView.RegisterNibForCell(ActionViewCell.Nib, ActionViewCell.Key);

            listType = _listType;
            vm = model;
            float width = 0;
            if (listType == ActionTypes.Small)
            {
                width = (float)((UIScreen.MainScreen.Bounds.Size.Width) / 2 - 10 - 4); //10: layout margins, 4: item spacing
                //CalculateColumnData();
            }
            else
            {
                width = (float)(UIScreen.MainScreen.Bounds.Size.Width - 20 - 4);

            }
            collectionView.CollectionViewLayout = new UICollectionViewFlowLayout
            {
                MinimumInteritemSpacing = 2,
                MinimumLineSpacing = 2,
                ItemSize = new CGSize(width, 30),
                SectionInset = new UIEdgeInsets(10, 0, 0, 0),
            };

        }

        void CalculateColumnData()
        {
            multiColumnData = new List<IList>();
            for (int i = 0; i < vm.ActionsItemSource.Count;)
            {
                List<BSUdpAction> subList;
                if ((i + 2) > vm.ActionsItemSource.Count)
                {
                    subList = vm.ActionsItemSource.ToList().GetRange(i, 1);
                }
                else
                {
                    subList = vm.ActionsItemSource.ToList().GetRange(i, 2);
                }
                multiColumnData.Add(subList);
                i = i + 2;
            }
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            switch (listType)
            {
                case ActionTypes.Small:
                    CalculateColumnData();
                    return multiColumnData.Count;
                case ActionTypes.Medium:
                case ActionTypes.Large:
                    return vm.ActionsItemSource.Count;
                default:
                    return vm.ActionsItemSource.Count;
            }
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            switch (listType)
            {
                case ActionTypes.Small:
                    return multiColumnData[(int)section].Count;
                case ActionTypes.Medium:
                case ActionTypes.Large:
                    return 1;
                default:
                    return 1;
            }
        }



        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (ActionViewCell)collectionView.DequeueReusableCell(ActionViewCell.Key, indexPath);
            if (listType == ActionTypes.Small)
            {
                cell.titleLbl.Text = ((BSUdpAction)multiColumnData[indexPath.Section][indexPath.Row]).Label;
            }
            else
            {
                cell.titleLbl.Text = vm.ActionsItemSource[(indexPath.Section)].Label;
            }
            cell.titleLbl.BackgroundColor = UIColor.FromRGB(216, 235, 254);
            cell.titleLbl.Layer.BorderWidth = (System.nfloat)0.5;
            cell.titleLbl.Layer.CornerRadius = (System.nfloat)5.0;
            cell.titleLbl.Layer.BorderColor = UIColor.DarkGray.CGColor;
            cell.titleLbl.TextColor = UIColor.Blue;
            return cell;
        }


        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            BSUdpAction action;
            if (listType == ActionTypes.Small)
            {
                action = ((BSUdpAction)multiColumnData[indexPath.Section][indexPath.Row]);
            }
            else
            {
                action = vm.ActionsItemSource[(indexPath.Section)];
            }
            vm.UDPCommand.Execute(action);
            //action.UDPActionCommand.Execute(action);
        }

        public override bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return true;
        }
    }
}
