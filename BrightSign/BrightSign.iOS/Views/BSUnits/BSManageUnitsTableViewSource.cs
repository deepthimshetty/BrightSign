using System;
using System.Collections.Generic;
using BrightSign.Core.Models;
using BrightSign.Core.ViewModels;
using BrightSign.Core.ViewModels.Settings;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace BrightSign.iOS.Views.BSUnits
{
    public class BSManageUnitsTableViewSource : MvxTableViewSource
    {
        ManageBSUnitsViewModel viewModel;
        public BSManageUnitsTableViewSource(UITableView tableView, ManageBSUnitsViewModel _viewModel)
            : base(tableView)
        {
            viewModel = _viewModel;
        }

        #region MvxTableViewSource

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 50;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return viewModel.BSUnitsItemSource.Count + 1;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            if (viewModel.BSUnitsItemSource.Count > 0)
            {
                return @"ACTIVE BRIGHTSIGN UNITS";
            }
            else
            {
                return @"NO ACTIVE BRIGHTSIGN UNITS FOUND";
            }
        }

        public override string TitleForFooter(UITableView tableView, nint section)
        {
            return @"";
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            UITableViewCell cell = tableView.DequeueReusableCell("cell");
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, "cell");
            }
            if (indexPath.Row == viewModel.BSUnitsItemSource.Count) {
                cell.TextLabel.Text = "Add Unit ...";
                cell.DetailTextLabel.Text = "";
                cell.TextLabel.TextColor = UIColor.Black;
            } else {
                BSDevice device = viewModel.BSUnitsItemSource[indexPath.Row];
                cell.TextLabel.Text = device.Name;
                cell.DetailTextLabel.Text = device.IpAddress;
                if (device.IsOnline)
                {
                    cell.TextLabel.TextColor = UIColor.Black;
                }
                else
                {
                    cell.TextLabel.TextColor = UIColor.Red;
                }
                cell.DetailTextLabel.TextColor = UIColor.Gray;
            }
            cell.TextLabel.Font = UIFont.BoldSystemFontOfSize(17);
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row == viewModel.BSUnitsItemSource.Count) {
                CommitEditingStyle(tableView,UITableViewCellEditingStyle.Insert, indexPath);
            }
        }

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView.Editing)
            {
                if (indexPath.Row == tableView.NumberOfRowsInSection(0) - 1)
                    return UITableViewCellEditingStyle.Insert;
                else
                    return UITableViewCellEditingStyle.Delete;
            }
            else // not in editing mode, enable swipe-to-delete for all rows
                return UITableViewCellEditingStyle.Delete;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    // remove the item from the underlying data source
                    viewModel.DeleteUnitCommand.Execute(indexPath.Row);
                    // delete the row from the table
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
                    break;
                case UITableViewCellEditingStyle.Insert :
                    {
                        viewModel.AddUnitCommand.Execute();
                    }
                    break;
                case UITableViewCellEditingStyle.None:
                    Console.WriteLine("CommitEditingStyle:None called");
                    break;
            }
        }

        public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
        {   // Optional - default text is 'Delete'
            return "Delete";
        }

        #endregion
    }
}
