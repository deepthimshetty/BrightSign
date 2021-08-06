using System;
using System.Collections.ObjectModel;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System.Linq;
using MvvmCross.Plugins.Messenger;
using static BrightSign.Core.Utility.BSUtility;
using System.Collections.Generic;
using BrightSign.Core.Utility.Interface;
using System.Diagnostics;
using BrightSign.Core.Utility.Database;
using BrightSign.Core.ViewModels.SearchUnits;
using BrightSign.Localization;

namespace BrightSign.Core.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{
		MvxSubscriptionToken AddActionToken;
		MvxSubscriptionToken SettingsRefreshToken;
		MvxSubscriptionToken SettingsToken;
		IUserPreferences UserPreferences;

		int _SelectedButtonType;
		public int SelectedButtonType
		{
			get
			{
				return _SelectedButtonType;
			}
			set
			{
				_SelectedButtonType = value;
				RaisePropertyChanged("SelectedButtonType");
				if (value == 0)
				{
					mediumButton.IsSelected = false;
					largeButton.IsSelected = false;
					smallButton.IsSelected = true;
				}
				else if (value == 1)
				{
					mediumButton.IsSelected = true;
					smallButton.IsSelected = false;
					largeButton.IsSelected = false;
				}
				else
				{
					largeButton.IsSelected = true;
					smallButton.IsSelected = false;
					mediumButton.IsSelected = false;
				}
				CheckValuesModified();
			}
		}

		ButtonSizeItem _smallButton;
		public ButtonSizeItem smallButton
		{
			get
			{
				return _smallButton;
			}
			set
			{
				_smallButton = value;
				RaisePropertyChanged("smallButton");
			}
		}

		ButtonSizeItem _largeButton;
		public ButtonSizeItem largeButton
		{
			get
			{
				return _largeButton;
			}
			set
			{
				_largeButton = value;
				RaisePropertyChanged("largeButton");
			}
		}


		ButtonSizeItem _mediumButton;
		public ButtonSizeItem mediumButton
		{
			get
			{
				return _mediumButton;
			}
			set
			{
				_mediumButton = value;
				RaisePropertyChanged("mediumButton");
			}
		}




		BSDevice NewDevice;

		public SettingsViewModel(IMvxMessenger messenger, IUserPreferences userPreferences) : base(messenger)
		{
			if (SettingsToken == null)
			{
				SettingsToken = Messenger.Subscribe<SettingsMessage>(OnSettingsResponse);
			}

			UserPreferences = userPreferences;
			if (CurrentDevice != null)
			{
				CurrentDevice.IsRightArrowVisible = true;
			}
			ViewTitle = TitleType.Settings.ToString();


			IsSnapshotsAvailable = Constants.IsSnapShotsConfigurable;

			IsDataModified = false;

			smallButton = new ButtonSizeItem()
			{
				ButtonText = "Small",
				IsSelected = false,
				ID = 0
			};
			mediumButton = new ButtonSizeItem()
			{
				ButtonText = "Medium",
				IsSelected = false,
				ID = 1
			};
			largeButton = new ButtonSizeItem()
			{
				ButtonText = "Large",
				IsSelected = false,
				ID = 2
			};

			RetreiveValuesfromUserPreferences();

		}

		private void OnSettingsResponse(SettingsMessage obj)
		{
			IsSnapshotsAvailable = Constants.IsSnapShotsConfigurable;
		}

		private void RetreiveValuesfromUserPreferences()
		{
			AutomaticRefresh = UserPreferences.GetBool(Constants.USER_PREFS_AUTO_REFRESH);
			SelectedButtonType = UserPreferences.GetInt(Constants.USER_PREFS_BUTTON_TYPE);
		}


		private void SaveValuestoUserPreferences()
		{
			UserPreferences.SetBool(Constants.USER_PREFS_AUTO_REFRESH, AutomaticRefresh);
			UserPreferences.SetInt(Constants.USER_PREFS_BUTTON_TYPE, SelectedButtonType);
			IsDataModified = false;
		}

		void CheckValuesModified()
		{
			if (AutomaticRefresh != UserPreferences.GetBool(Constants.USER_PREFS_AUTO_REFRESH) || SelectedButtonType != UserPreferences.GetInt(Constants.USER_PREFS_BUTTON_TYPE))
			{
				IsDataModified = true;
			}
			else
			{
				IsDataModified = false;
			}
		}

		private bool _IsSnapshotsAvailable;
		public bool IsSnapshotsAvailable
		{
			get { return _IsSnapshotsAvailable; }
			set
			{
				_IsSnapshotsAvailable = value;
				RaisePropertyChanged(() => IsSnapshotsAvailable);
				if (value)
				{
					SnapshotText = "Snapshot Configuration";
				}
				else
				{
					SnapshotText = "Remote Snapshots not available";
				}
			}
		}


		private string _SnapshotText;
		public string SnapshotText
		{
			get { return _SnapshotText; }
			set
			{
				_SnapshotText = value;
				RaisePropertyChanged(() => SnapshotText);
			}
		}

		private bool _AutomaticRefresh;
		public bool AutomaticRefresh
		{
			get { return _AutomaticRefresh; }
			set
			{
				_AutomaticRefresh = value;
				RaisePropertyChanged(() => AutomaticRefresh);
				CheckValuesModified();
			}
		}
		private List<BSUdpAction> _BSActionList;
		public List<BSUdpAction> BSActionList
		{
			get { return _BSActionList; }
			set
			{
				_BSActionList = value;
				RaisePropertyChanged(() => BSActionList);
			}

		}

		private ObservableCollection<ListViewItem> _settingsItemSource;
		public ObservableCollection<ListViewItem> SettingsItemSource
		{
			get { return _settingsItemSource; }
			set
			{
				_settingsItemSource = value;
				RaisePropertyChanged(() => SettingsItemSource);
			}

		}

		private bool _ReloadSettings;
		public bool ReloadSettings
		{
			get { return _ReloadSettings; }
			set
			{
				_ReloadSettings = value;
				RaisePropertyChanged(() => ReloadSettings);
			}
		}

		private bool _IsDataModified;
		public bool IsDataModified
		{
			get { return _IsDataModified; }
			set
			{
				_IsDataModified = value;
				RaisePropertyChanged(() => IsDataModified);
			}
		}



		private bool _ReloadActionSection;
		public bool ReloadActionSection
		{
			get { return _ReloadActionSection; }
			set
			{
				_ReloadActionSection = value;
				RaisePropertyChanged(() => ReloadActionSection);
			}
		}


		private string _DeviceName;
		public string DeviceName
		{
			get { return _DeviceName; }
			set
			{
				_DeviceName = value;
				RaisePropertyChanged(() => DeviceName);
			}
		}

		private List<ActionButtonType> _items = new List<ActionButtonType>()
			{
			new ActionButtonType{Title="Small",ID=0},
			new ActionButtonType{Title="Medium",ID=1},
			new ActionButtonType{Title="Large",ID=2}
			};
		public List<ActionButtonType> Items
		{
			get { return _items; }
			set { _items = value; RaisePropertyChanged(() => Items); }
		}

		private ActionButtonType _selectedItem = new ActionButtonType();
		public ActionButtonType SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value;
				RaisePropertyChanged(() => SelectedItem);
			}
		}

		internal void SetData()
		{
			IsSnapshotsAvailable = Constants.IsSnapShotsConfigurable;
		}

		/// <summary>
		/// Gets the list selector command.
		/// </summary>
		/// <value>The list selector command.</value>
		public IMvxCommand ListSelectorCommand
		{
			get { return new MvxCommand<ListViewItem>(ExecuteListSelectorCommand); }
		}

		public MvxCommand CancelCommand
		{
			get { return new MvxCommand(() => ExecuteCancelCommand()); }
		}

		public MvxCommand SaveCommand
		{
			get { return new MvxCommand(() => ExecuteSaveCommand()); }
		}

		public IMvxCommand SetButtonSizeCommand
		{
			get
			{
				return new MvxCommand<ActionButtonType>(ExecuteSetButtonSizeCommand);
			}
		}

		private void ExecuteSetButtonSizeCommand(ActionButtonType actionButtonType)
		{
			UserPreferences.SetInt(Constants.USER_PREFS_BUTTON_TYPE, actionButtonType.ID);
		}

		public IMvxCommand SetAutoRefreshCommand
		{
			get { return new MvxCommand<bool>(ExecuteSetAutoRefreshCommand); }
		}

		private void ExecuteSetAutoRefreshCommand(bool isAutoRefresh)
		{
			UserPreferences.SetBool(Constants.USER_PREFS_AUTO_REFRESH, isAutoRefresh);
		}

		//public IMvxCommand RemoveItemCommand
		//{
		//    get { return new MvxCommand<ListViewItem>(ExecuteRemoveItemCommand); }
		//}

		//private void ExecuteRemoveItemCommand(ListViewItem item)
		//{

		//}

		private void ExecuteCancelCommand()
		{
			RetreiveValuesfromUserPreferences();
			IsDataModified = false;
		}

		private void ExecuteSaveCommand()
		{

			SaveValuestoUserPreferences();

			Mvx.Resolve<ICustomAlert>().ShowCustomAlert(true, Strings.settings, Strings.savedsuccessfully);

			Messenger.Publish(new LoadButtonsMessage(this, DeviceStatus.Connected, true));

		}

		private void ExecuteListSelectorCommand(ListViewItem item)
		{
			if (item.CellType == CellTypes.ActionItem)
			{
				_settingsItemSource.Remove(item);
				var deletedItem = BSUtility.Instance.ActionItemSource.Where(x => x.ID.Equals(item.ID)).FirstOrDefault();
				if (deletedItem != null)
					BSUtility.Instance.ActionItemSource.Remove(deletedItem);
			}
			else if (item.CellType == CellTypes.TwoLabel)
			{
				GoToRemoteSnapshotVM();
			}
			if (item.CellType == CellTypes.NavigationLabel)
			{
				GoToSelectUnitsVM();

			}

			//if (item.CellType == CellTypes.ListSelector)
			//{
			//    _mListSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<ListSelectorMessage>(OnListSelectorMessage);
			//    ShowViewModel<ListSelectorViewModel>(new { type = item.ListSelectorType, selectedItemIndex = BatterySettingsItemSource.IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items) });
			//}
		}

		//private void OnItemCreatedMessage(ListItemCreatedMessage obj)
		//{
		//    Mvx.Resolve<IMvxMessenger>().Unsubscribe<ListItemCreatedMessage>(_mAddListItemToken);
		//    if (obj.listItem != null)
		//    {
		//        var item = obj.listItem;
		//        //item.RemoveItemCommand = RemoveItemCommand;
		//        _settingsItemSource.Add(item);
		//    }
		//}

		public IMvxCommand RemoteSnapshotsCommand
		{
			get { return new MvxCommand(GoToRemoteSnapshotVM); }
		}

		public void GoToRemoteSnapshotVM()
		{
			if (Constants.IsSnapShotsConfigurable)
			{
				ShowViewModel<RemoteSnapshotViewModel>();
			}
		}

		public void GoToSelectUnitsVM()
		{
			ShowViewModel<BSUnitsViewModel>();

			if (SettingsRefreshToken == null)
			{
				SettingsRefreshToken = Messenger.Subscribe<SettingsRefreshMessage>(OnSettingsRefreshResponse);
			}
		}

		private void OnSettingsRefreshResponse(SettingsRefreshMessage obj)
		{
			if (obj.IsRefresh)
			{
				if (obj.device != null)
				{
					NewDevice = obj.device;
					DeviceName = obj.device.Name;
					ReloadSettings = true;
				}
			}
		}

		

		private void PrepareData()
		{
			_settingsItemSource = new ObservableCollection<ListViewItem>();

			//var listItem = new ListViewItem();

			//listItem.TopMargin = 30;
			//listItem.Title = "BRIGHTSIGN UNIT";
			//listItem.CellType = CellTypes.LabelWithTopMargin;
			//_settingsItemSource.Add(listItem);

			//var navigationCell = new ListViewItem();

			//navigationCell.Title = DeviceName;
			//navigationCell.CellType = CellTypes.NavigationLabel;
			//navigationCell.ListSelectionCommand = ListSelectorCommand;

			//_settingsItemSource.Add(navigationCell);

			//var labelItem = new ListViewItem();

			//labelItem.Title = "Touch the arrow to select a different BrightSign Unit";
			//labelItem.CellType = CellTypes.Label;
			//_settingsItemSource.Add(labelItem);

			var labelItem1 = new ListViewItem();
			labelItem1.TopMargin = 30;
			labelItem1.Title = "USER VARIABLE UPDATES";
			labelItem1.CellType = CellTypes.LabelWithTopMargin;
			_settingsItemSource.Add(labelItem1);

			var switchlItem = new ListViewItem();

			switchlItem.Title = "Automatic Refresh";
			switchlItem.CellType = CellTypes.LabelSwitch;
			var isAutoRefreshEnabled = Mvx.Resolve<IUserPreferences>().GetBool(Constants.USER_PREFS_AUTO_REFRESH);
			switchlItem.IsSwitchEnabled = isAutoRefreshEnabled;
			switchlItem.SwitchValueChanged = SetAutoRefreshCommand;
			_settingsItemSource.Add(switchlItem);

			var topMarginItem = new ListViewItem();

			topMarginItem.Title = "REMOTE SNAPSHOTS";
			topMarginItem.CellType = CellTypes.LabelWithTopMargin;
			_settingsItemSource.Add(topMarginItem);

			var twoLabelItem = new ListViewItem();

			twoLabelItem.Title = "Snapshot Configuration";
			twoLabelItem.SubTitle = "Remote snapshots are not supported";
			twoLabelItem.ListSelectionCommand = ListSelectorCommand;
			twoLabelItem.CellType = CellTypes.TwoLabel;
			_settingsItemSource.Add(twoLabelItem);

			var labelItem2 = new ListViewItem();
			labelItem2.TopMargin = 30;
			labelItem2.Title = "ACTION BUTTONS";
			labelItem2.CellType = CellTypes.LabelWithTopMargin;
			_settingsItemSource.Add(labelItem2);

			var radioItem = new ListViewItem();

			radioItem.Title = "Size:";
			radioItem.CellType = CellTypes.LabelWithRadioGroup;
			radioItem.Items = Items;
			var itemId = Mvx.Resolve<IUserPreferences>().GetInt(Constants.USER_PREFS_BUTTON_TYPE);
			SelectedItem = Items.Where(x => x.ID.Equals(itemId)).FirstOrDefault();
			radioItem.SelectedItem = SelectedItem;
			radioItem.RadioValueChanged = SetButtonSizeCommand;
			_settingsItemSource.Add(radioItem);

			//var addButtonItem = new ListViewItem();

			//addButtonItem.Title = "Add Button ...";
			//addButtonItem.CellType = CellTypes.AddButton;
			//addButtonItem.ListSelectionCommand = ListSelectorCommand;
			//_settingsItemSource.Add(addButtonItem);


			foreach (var item in BSActionList)
			{
				_settingsItemSource.Add(new ListViewItem()
				{
					Title = item.Label,
					SubTitle = item.DataUDP,
					IsUserDefined = item.IsUserDefined,
					CellType = CellTypes.ActionItem
				});
			}


			if (BSUtility.Instance?.ActionItemSource?.Count > 0)
			{
				foreach (var item in BSUtility.Instance.ActionItemSource)
				{
					var isExist = SettingsItemSource.Any(x => x.ID.Equals(item.ID));
					if (!isExist)
					{
						item.ListSelectionCommand = ListSelectorCommand;
						_settingsItemSource.Add(item);
					}
				}
			}
		}
	}
}
