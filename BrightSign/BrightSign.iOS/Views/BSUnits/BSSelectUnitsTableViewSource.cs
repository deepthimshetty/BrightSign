using System;
using System.Collections.Generic;
using BrightSign.Core.Models;
using BrightSign.Core.ViewModels;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace BrightSign.iOS.Views.BSUnits
{
    public class BSSelectUnitsTableViewSource : MvxTableViewSource
    {
        BSUnitsViewModel viewModel;
        public BSSelectUnitsTableViewSource(UITableView tableView, BSUnitsViewModel _viewModel)
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
            return 2;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (section == 0)
            {
                return viewModel.BSUnitsItemSource.Count;
            }
            else
            {
                return 1;
            }
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            if (section == 0) {
                if (viewModel.BSUnitsItemSource.Count > 0) {
                    return @"ACTIVE BRIGHTSIGN UNITS";
                } else {
                    return @"NO ACTIVE BRIGHTSIGN UNITS FOUND";
                }
            } else
                return @"";
        }

        public override string TitleForFooter(UITableView tableView, nint section)
        {
            if (section == 1)
            {
                return @"Add or remove BrightSign units from the Active Unit list";
            }
            return @"";
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            UITableViewCell cell = tableView.DequeueReusableCell("cell");
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, "cell");
            }
            if (indexPath.Section == 0)
            {
                BSDevice device = viewModel.BSUnitsItemSource[indexPath.Row];
                cell.TextLabel.Text = device.Name;
                cell.DetailTextLabel.Text = device.IpAddress;
                if (device.IsDefault) {
                    cell.Accessory = UITableViewCellAccessory.Checkmark;
                } else {
                    cell.Accessory = UITableViewCellAccessory.None;
                }
                if (device.IsOnline) {
                    cell.TextLabel.TextColor = UIColor.Black;
                } else {
                    cell.TextLabel.TextColor = UIColor.Red;
                }
                cell.DetailTextLabel.TextColor = UIColor.Gray;
            }
            else
            {
                cell.TextLabel.Text = "Manage Active BrightSigns";
                cell.DetailTextLabel.Text = "";
                cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
            }
            cell.TextLabel.Font = UIFont.BoldSystemFontOfSize(17);
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Section == 0) {
                foreach (BSDevice dev in viewModel.BSUnitsItemSource)
                {
                    dev.IsDefault = false;
                }
                BSDevice device = viewModel.BSUnitsItemSource[indexPath.Row];
                device.IsDefault = true;
                tableView.ReloadSections(new NSIndexSet(0), UITableViewRowAnimation.None);
            } else {
                viewModel.ManageBSUnitsCommand.Execute();
            }
        }
        #endregion
    }
}
