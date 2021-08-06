using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Interface;
using Foundation;
using MvvmCross.Binding.Bindings;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace BrightSign.iOS.Utility
{
    public class MvxEditDeleteStandardTableViewSource : MvxStandardTableViewSource
    {
        private IModify m_ViewModel;

        #region Constructors
        public MvxEditDeleteStandardTableViewSource(IModify viewModel, UITableView tableView, UITableViewCellStyle style, NSString cellIdentifier, IEnumerable<MvxBindingDescription> descriptions, UITableViewCellAccessory tableViewCellAccessory = 0)
            : base(tableView, style, cellIdentifier, descriptions, tableViewCellAccessory)
        {
            m_ViewModel = viewModel;
        }


        public MvxEditDeleteStandardTableViewSource(IModify viewModel, UITableView tableView, string bindingText) : base(tableView, bindingText)
        {
            m_ViewModel = viewModel;
        }

        public MvxEditDeleteStandardTableViewSource(IModify viewModel, UITableView tableView, NSString cellIdentifier) : base(tableView, cellIdentifier)
        {
            m_ViewModel = viewModel;
        }

        public MvxEditDeleteStandardTableViewSource(IModify viewModel, UITableView tableView) : base(tableView)
        {
            m_ViewModel = viewModel;
        }


        public MvxEditDeleteStandardTableViewSource(IModify viewModel, UITableView tableView, UITableViewCellStyle style, NSString cellId, string binding, UITableViewCellAccessory accessory)
            : base(tableView, style, cellId, binding, accessory)
        {
            m_ViewModel = viewModel;
        }
        #endregion

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            //if (m_ViewModel.Selectedtab == 1)
            //{
            //    return true;
            //}
            //return false;
            return true;

        }


        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    m_ViewModel.RemoveCommand.Execute(indexPath.Row);
                    break;
                case UITableViewCellEditingStyle.None:
                    break;
            }
        }

        //public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        //{
        //    return UITableViewCellEditingStyle.Delete;
        //}

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewRowAction editAction = UITableViewRowAction.Create(UITableViewRowActionStyle.Normal, "Edit", (arg1, arg2) =>
            {
                m_ViewModel.EditCommand.Execute(indexPath.Row);
            });
            //editAction.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("edit.png"));
            //editAction.BackgroundColor = UIColorUtility.FromHex(ColorConstants.ManageActionGreyColor);


            UITableViewRowAction deleteAction = UITableViewRowAction.Create(UITableViewRowActionStyle.Destructive, "Delete", (arg1, arg2) =>
            {
                m_ViewModel.RemoveCommand.Execute(indexPath.Row);
            });
            //deleteAction.BackgroundColor = UIColorUtility.FromHex(ColorConstants.ManageActionGreyColor);

            //return base.EditActionsForRow(tableView, indexPath);


            if (m_ViewModel.Selectedtab == 1)
            {
                return new UITableViewRowAction[] { deleteAction, editAction };
            }
            else
            {
                return new UITableViewRowAction[] { editAction };
            }

        }

        public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;

        }


        /// <summary>
        /// The items source.
        /// </summary>
        ObservableCollection<BSUdpAction> _itemsSource;
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
                //ReloadData();
            }
        }

        public override void MoveRow(UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        {
            var item = ListItemsSource[sourceIndexPath.Row];
            var deleteAt = sourceIndexPath.Row;
            var insertAt = destinationIndexPath.Row;

            // are we inserting 
            if (destinationIndexPath.Row < sourceIndexPath.Row)
            {
                // add one to where we delete, because we're increasing the index by inserting
                deleteAt += 1;
            }
            else
            {
                // add one to where we insert, because we haven't deleted the original yet
                insertAt += 1;
            }
            ListItemsSource.Insert(insertAt, item);
            ListItemsSource.RemoveAt(deleteAt);
        }

    }
}
