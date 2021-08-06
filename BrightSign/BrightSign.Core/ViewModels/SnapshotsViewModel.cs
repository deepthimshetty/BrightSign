using System;
using System.Collections.ObjectModel;
using System.Linq;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Messages;
using BrightSign.Core.ViewModels.Settings;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace BrightSign.Core.ViewModels
{
    public class SnapshotsViewModel : BaseViewModel
    {
        MvxSubscriptionToken ImagesRefreshToken;

        float currentTransformValue = 0;

        public SnapshotsViewModel(IMvxMessenger messenger) : base(messenger)
        {
            ViewTitle = "Gallery";


            //TODO Remove this code, this code added for testing
            //Constants.SnapshotConfig = new SnapshotConfigModel();
            //Constants.SnapshotConfig.ResY = 768;
            //Constants.SnapshotConfig.ResX = 1024;


            //Constants.BSSnapshotList = new System.Collections.Generic.List<BSSnapshot>();
            //for (int i = 0; i < 10; i++)
            //{
            //    Constants.BSSnapshotList.Add(new BSSnapshot()
            //    {
            //        ImageUrl = "https://homepages.cae.wisc.edu/~ece533/images/airplane.png"
            //    });
            //}


            if (Constants.BSSnapshotList != null)
                SnapshotsItemSource = new ObservableCollection<BSSnapshot>(Constants.BSSnapshotList.Where(o => o.ImageDataObj!=null && o.ImageDataObj.ImagePath!=null));





            foreach (var item in SnapshotsItemSource)
            {
                if (Constants.SnapshotConfig.DisplayPortraitMode)
                {
                    item.ImageTransform += (float)(Math.PI / 2);
                }
            }

            try
            {
                if (ImagesRefreshToken == null)
                {
                    ImagesRefreshToken = Messenger.Subscribe<ImageRefreshMessage>(OnImageRefreshResponse);
                }

            }
            catch (Exception ex)
            {

            }

        }

        private void OnImageRefreshResponse(ImageRefreshMessage obj)
        {
            BSUtility.Instance.GetSnapshotConfiguration().ContinueWith((arg) =>
            {
                SnapshotsItemSource = new ObservableCollection<BSSnapshot>(Constants.BSSnapshotList.Where(o => o.ImageDataObj != null && o.ImageDataObj.ImagePath != null));
            });
        }

        private ObservableCollection<BSSnapshot> _snapshotsItemSource;
        public ObservableCollection<BSSnapshot> SnapshotsItemSource
        {
            get { return _snapshotsItemSource; }
            set
            {
                _snapshotsItemSource = value;
                RaisePropertyChanged(() => SnapshotsItemSource);
            }

        }

        public void SetItemSource()
        {
            if (Constants.BSSnapshotList != null)
                SnapshotsItemSource = new ObservableCollection<BSSnapshot>(Constants.BSSnapshotList.Where(o => o.ImageDataObj != null && o.ImageDataObj.ImagePath != null));
        }

        public MvxCommand CancelCommand
        {
            get { return new MvxCommand(() => ExecuteCancelCommand()); }
        }

        private void ExecuteCancelCommand()
        {
            Close(this);
        }

        public IMvxCommand ItemSelected
        {
            get
            {
                return new MvxCommand<BSSnapshot>(OnItemSelected);
            }
        }

        public void OnItemSelected(BSSnapshot snapShot)
        {
            int selectedIndex = SnapshotsItemSource.IndexOf(snapShot);
            //ShowViewModel<SnapshotViewModel>(selectedIndex);
            ShowViewModel<ShareViewModel>(new { index = selectedIndex });


        }
    }
}
