using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Database;
using BrightSign.Core.ViewModels.AddDevice;
using MvvmCross.ViewModels;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace BrightSign.Core.ViewModels
{
    public class OfflineViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public OfflineViewModel(IMvxMessenger messenger, IMvxNavigationService navigationService) : base(messenger)
        {
            _navigationService = navigationService;
        }
        private ObservableCollection<BSDevice> _offlineItemSource;
        public ObservableCollection<BSDevice> OfflineItemSource
        {
            get { return _offlineItemSource; }
            set
            {
                _offlineItemSource = value;
                RaisePropertyChanged(() => OfflineItemSource);
            }

        }
        public IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand<BSDevice>(ExecuteListSelectorCommand); }
        }
        private void ExecuteListSelectorCommand(BSDevice item)
        {
            // Constants.ActiveDevice = item;
            // ShowViewModel<MainViewModel>();
        }
        public IMvxCommand AddDeviceCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    AddDeviceClick();
                });
            }
        }

        private async void AddDeviceClick()
        {
            //ShowViewModel<AddDeviceViewModel>();
           await _navigationService.Navigate<AddDeviceViewModel>();
        }
       

        public ICommand RemoveCommand
        {
            get
            {
                return new MvxCommand<int>(RemoveClick);
            }
        }

        private void RemoveClick(int removeIndex)
        {
            var item = OfflineItemSource[removeIndex];
            OfflineItemSource.RemoveAt(removeIndex);
            Constants.FullDevices.Remove(item);
            DBHandler.Instance.RemoveDevice(item);

        }
    }
}
