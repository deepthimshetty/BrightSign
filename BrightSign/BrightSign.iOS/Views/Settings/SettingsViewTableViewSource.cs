using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using MvvmCross.Binding.iOS.Views;
using BrightSign.iOS.Views.Home.CustomViews;
using BrightSign.Core.Models;
using BrightSign.Core.ViewModels;
using BrightSign.Core.Utility;

namespace BrightSign.iOS.Views.Home
{
	public class SettingsViewTableViewSource : MvxTableViewSource
	{
		const int DEVICE_SECTION = 0;
		const int UPDATE_SECTION = 1;
		const int SNAPSHOT_SECTION = 2;
		const int BUTTON_SIZE_SECTION = 3;
		const int ACTION_SECTION = 4;
		const int NUM_SETTINGS_SECTIONS = 5;

		List<BSUdpAction> _actionList;
		SettingsViewModel vm;
		#region Constructor

		public SettingsViewTableViewSource(UITableView tableView, List<BSUdpAction> actionList, SettingsViewModel vwml)
			: base(tableView)
		{
			_actionList = actionList;
			vm = vwml;
			tableView.RegisterNibForCellReuse(BSSwitchTableViewCell.Nib, BSSwitchTableViewCell.Key);
			tableView.RegisterNibForCellReuse(BSSizeSelectTableViewCell.Nib, BSSizeSelectTableViewCell.Key);
		}

		#endregion

		#region MvxTableViewSource

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 50;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return NUM_SETTINGS_SECTIONS;
		}
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == SNAPSHOT_SECTION)
			{
				vm.GoToRemoteSnapshotVM();
			}
			else if (indexPath.Section == DEVICE_SECTION)
			{
				vm.GoToSelectUnitsVM();
			}
		}
		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (section == DEVICE_SECTION || section == UPDATE_SECTION | section == SNAPSHOT_SECTION || section == BUTTON_SIZE_SECTION)
				return 1;
			else if (section == ACTION_SECTION)
				return _actionList != null ? _actionList.Count + 1 : 1;
			//return 1;
			return 0;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			if (section == DEVICE_SECTION)
				return @"BrightSign Unit";
			else if (section == UPDATE_SECTION)
				return @"User Variable Updates";
			else if (section == SNAPSHOT_SECTION)
				return @"Remote Snapshots";
			else if (section == BUTTON_SIZE_SECTION)
				return @"Action Buttons";
			return @"";
		}

		public override string TitleForFooter(UITableView tableView, nint section)
		{
			if (section == DEVICE_SECTION)
			{
				return @"Touch the arrow to select a different BrightSign Unit";
			}
			return @"";
		}

		static string BasicCell = "BsBasicCell";
		static string DeviceCell = "BsDeviceCell";
		static string SnapShotCell = "BsSnapShotCell";
		static string ActionCell = "bsActionCell";

		protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, Foundation.NSIndexPath indexPath, object item)
		{
			UITableViewCell cell = new UITableViewCell(UITableViewCellStyle.Default, DeviceCell); ;
			if (indexPath.Section == DEVICE_SECTION)
			{
				UITableViewCell deviceCell = new UITableViewCell(UITableViewCellStyle.Default, DeviceCell);
				deviceCell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
				deviceCell.TextLabel.Text = vm.DeviceName;
				deviceCell.SelectionStyle = UITableViewCellSelectionStyle.None;
				return deviceCell;
			}
			else if (indexPath.Section == UPDATE_SECTION)
			{
				BSSwitchTableViewCell switchTableViewCell = (BSSwitchTableViewCell)tableView.DequeueReusableCell(BSSwitchTableViewCell.Key, indexPath);
				switchTableViewCell.RefreshSwitch.On = vm.AutomaticRefresh;
				switchTableViewCell.SelectionStyle = UITableViewCellSelectionStyle.None;
				switchTableViewCell.RefreshSwitch.ValueChanged += (sender, e) =>
				{
					vm.SetAutoRefreshCommand.Execute((sender as UISwitch).On);
				};
				return switchTableViewCell;
			}
			else if (indexPath.Section == SNAPSHOT_SECTION)
			{
				if (Constants.IsSnapShotsConfigurable)
				{
					UITableViewCell snapShotCell = new UITableViewCell(UITableViewCellStyle.Default, DeviceCell);
					snapShotCell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
					snapShotCell.TextLabel.Text = "Snapshot Configuration";
					snapShotCell.SelectionStyle = UITableViewCellSelectionStyle.None;
					return snapShotCell;
				}
				else
				{
					UITableViewCell snapShotCell = new UITableViewCell(UITableViewCellStyle.Subtitle, SnapShotCell);
					snapShotCell.TextLabel.Text = "Snapshot Configuration";
					snapShotCell.DetailTextLabel.Text = "Remote snapshots are not supported";
					snapShotCell.SelectionStyle = UITableViewCellSelectionStyle.None;
					return snapShotCell;
				}

			}
			else if (indexPath.Section == BUTTON_SIZE_SECTION)
			{
				BSSizeSelectTableViewCell sizeSelectTableViewCell = (BSSizeSelectTableViewCell)tableView.DequeueReusableCell(BSSizeSelectTableViewCell.Key, indexPath);
				sizeSelectTableViewCell.SelectionStyle = UITableViewCellSelectionStyle.None;
				return sizeSelectTableViewCell;
			}
			else if (indexPath.Section == ACTION_SECTION)
			{
				if (indexPath.Row == 0)
				{
					UITableViewCell actionCell = new UITableViewCell(UITableViewCellStyle.Default, BasicCell); ;
					actionCell.TextLabel.Text = "Add Button ...";
					actionCell.SelectionStyle = UITableViewCellSelectionStyle.None;
					return actionCell;
				}
				else if (_actionList != null && _actionList.Count > 0)
				{
					int buttonIndex = indexPath.Row - 1;
					var button = _actionList[buttonIndex];
					UITableViewCell deviceCell = new UITableViewCell(UITableViewCellStyle.Subtitle, ActionCell);
					deviceCell.TextLabel.Text = button.Label;
					deviceCell.DetailTextLabel.Text = button.DataUDP;
					deviceCell.SelectionStyle = UITableViewCellSelectionStyle.None;
					return deviceCell;
				}
			}
			return null;
		}

		public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == ACTION_SECTION)
			{
				if (indexPath.Row == 0)
					return UITableViewCellEditingStyle.Insert;
				else
					return UITableViewCellEditingStyle.Delete;
			}
			else
			{
				return UITableViewCellEditingStyle.None;
			}
		}

		
		public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
		{   // Optional - default text is 'Delete'
			return "Delete";
		}
		public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == ACTION_SECTION)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
		{
            return false;
		}
		
		#endregion
	}
}

