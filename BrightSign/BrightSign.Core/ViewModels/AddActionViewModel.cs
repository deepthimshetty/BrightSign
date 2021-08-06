using System;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System.Linq;
using MvvmCross.Plugins.Messenger;
using BrightSign.Core.Utility.Interface;
using BrightSign.Localization;

namespace BrightSign.Core.ViewModels
{
    public class AddActionViewModel : BaseViewModel
    {
        IDialogService _dialogService;
        public AddActionViewModel(IMvxMessenger messenger, IDialogService dialogService) : base(messenger)
        {
            _dialogService = dialogService;
        }

        private string _label;
        public string Label
        {
            get
            { return _label; }
            set
            {
                _label = value;
                RaisePropertyChanged(() => Label);
            }
        }

        private string _data;
        public string Data
        {
            get
            { return _data; }
            set
            {
                _data = value;
                RaisePropertyChanged(() => Data);
            }
        }

        public IMvxCommand SaveCommand
        {
            get { return new MvxCommand(() => ExecuteSaveCommand()); }
        }

        public IMvxCommand CancelCommand
        {
            get { return new MvxCommand(() => ExecuteCancelCommand()); }
        }

        private void ExecuteSaveCommand()
        {
            if (string.IsNullOrEmpty(Label))
            {
                _dialogService.ShowAlertAsync(Strings.labelerror, Strings.error, Strings.ok);
            }
            else if (string.IsNullOrEmpty(Data))
            {
                _dialogService.ShowAlertAsync(Strings.dataerror, Strings.error, Strings.ok);
            }
            else
            {
                //if (Plugin.DeviceInfo.CrossDeviceInfo.Current.Platform == Plugin.DeviceInfo.Abstractions.Platform.iOS) {
                Messenger.Publish(new AddActionMessage(this, new BSUdpAction()
                {
                    Label = Label,
                    DataUDP = Data,
                    IsUserDefined = true
                }));
                //} else {
                //    var listItem = new ListViewItem();
                //    listItem.Title = Label;
                //    listItem.SubTitle = Data;
                //    listItem.CellType = Utility.BSUtility.CellTypes.ActionItem;
                //    listItem.ID = (BSUtility.Instance?.ActionItemSource?.Count > 0) ?
                //        ((BSUtility.Instance.ActionItemSource.LastOrDefault().ID) + 1) : 1;
                //    if (!BSUtility.Instance.ActionItemSource.Any(x => x.ID.Equals(listItem.ID)))
                //        BSUtility.Instance.ActionItemSource.Add(listItem);
                //}
                Close(this);
                //ShowViewModel<SettingsViewModel>();
                //Mvx.Resolve<IMvxMessenger>().Publish(new ListItemCreatedMessage(this, listItem));
            }
        }

        private void ExecuteCancelCommand()
        {
            Close(this);
            // ShowViewModel<SettingsViewModel>();
            // Mvx.Resolve<IMvxMessenger>().Publish(new ListItemCreatedMessage(this, null));
        }
    }
}
