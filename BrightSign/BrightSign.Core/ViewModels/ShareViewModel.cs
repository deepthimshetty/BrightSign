using System;
using System.Collections.ObjectModel;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace BrightSign.Core.ViewModels
{
    public class ShareViewModel : BaseViewModel
    {

        private BSSnapshot _snapshot;
        public BSSnapshot SnapShot
        {
            get { return _snapshot; }
            set
            {
                _snapshot = value;
                RaisePropertyChanged(() => SnapShot);
            }
        }

        public MvxCommand CancelCommand
        {
            get { return new MvxCommand(() => ExecuteCancelCommand()); }
        }

        int _selectedIndex = 0;
        public int selectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                this.SnapShot = SnapshotsItemSource[_selectedIndex];
                ViewTitle = SnapShot.TimeStamp;
                RaisePropertyChanged(() => selectedIndex);
            }
        }

        private ObservableCollection<BSSnapshot> _snapshotsItemSource;
        public ObservableCollection<BSSnapshot> SnapshotsItemSource
        {
            get { return _snapshotsItemSource; }
            set
            {
                _snapshotsItemSource = value;
            }

        }

        private void ExecuteCancelCommand()
        {
            Close(this);
        }

        public ShareViewModel(IMvxMessenger messenger) : base(messenger)
        {

        }

        public void Init(int index)
        {
            SnapshotsItemSource = new ObservableCollection<BSSnapshot>(Constants.BSSnapshotList);
            this.SnapShot = SnapshotsItemSource[index];
            selectedIndex = index;

            ViewTitle = SnapShot.TimeStamp;
        }

        public void SwipeRight()
        {
            selectedIndex = --selectedIndex;
            if (selectedIndex >= 0)
            {
                SnapShot = SnapshotsItemSource[selectedIndex];
                ViewTitle = SnapShot.TimeStamp;
            }
        }

        public void SwipeLeft()
        {
            selectedIndex = ++selectedIndex;
            if (selectedIndex < SnapshotsItemSource.Count)
            {
                SnapShot = SnapshotsItemSource[selectedIndex];
                ViewTitle = SnapShot.TimeStamp;
            }

        }
    }
}
