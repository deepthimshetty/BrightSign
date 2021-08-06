using System;
using System.Threading.Tasks;
using System.Xml;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Interface;
using BrightSign.Core.Utility.Web;
using BrightSign.Localization;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace BrightSign.Core.ViewModels
{
    public class RemoteSnapshotViewModel : BaseViewModel
    {
        IDialogService dialogservice;
        private SnapshotConfigModel _snapshotconfig;
        public SnapshotConfigModel snapshotconfig
        {
            get
            {
                return _snapshotconfig;
            }
            set
            {
                _snapshotconfig = value;
                RaisePropertyChanged("snapshotconfig");
            }
        }


        public RemoteSnapshotViewModel(IDialogService _dialogservice, IMvxMessenger messenger) : base(messenger)
        {
            ViewTitle = "Remote Snapshots";
            dialogservice = _dialogservice;
            snapshotconfig = new SnapshotConfigModel();

            Task.Run(async () =>
                {
                    InvokeOnMainThread(() =>
                    {
                        IsBusy = true;
                    });

                    bool IsSnapshotsDownloaded = Constants.IsSnapShotsConfigurable = await BSUtility.Instance.GetSnapshotConfiguration();
                    snapshotconfig = Constants.SnapshotConfig;
                    InvokeOnMainThread(() =>
                    {
                        IsBusy = false;
                    });
                });


        }


        public MvxCommand CancelRSCommand
        {
            get { return new MvxCommand(() => ExecuteCancelCommand()); }
        }

        public MvxCommand SaveRSCommand
        {
            get { return new MvxCommand(async () => await ExecuteSaveCommand()); }
        }
        private void ExecuteCancelCommand()
        {
            Close(this);
        }

        private async Task ExecuteSaveCommand()
        {
            // Save Data


            IsBusy = true;

            try
            {
                var IsSuccess = await HttpBase.Instance.SaveSnapshotsConfiguration(snapshotconfig);
                if (IsSuccess)
                {
                    Close(this);
                    IsBusy = false;
                    Mvx.Resolve<ICustomAlert>().ShowCustomAlert(true, Strings.configuration, Strings.savedsuccessfully);

                }
                else
                {
                    IsBusy = false;
                    await dialogservice.ShowAlertAsync(Strings.something_went_wrong, Strings.error, Strings.ok);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await dialogservice.ShowAlertAsync(Strings.something_went_wrong, Strings.error, Strings.ok);

            }


        }

        ///// <summary>
        ///// The enable Remote snapshot is checked.
        ///// </summary>
        //bool _EnableRSIsChecked;
        //public bool EnableRSIsChecked
        //{
        //    get
        //    {
        //        return _EnableRSIsChecked;
        //    }
        //    set
        //    {
        //        //if (EnableRSIsChecked)
        //        //{
        //        //    Task.Run(async () =>
        //        //    {
        //        //        await SetStarterDisableStatus(value, true);
        //        //    });
        //        //}
        //        //else
        //        //{
        //        //    EnableRSIsChecked = true;
        //        //}
        //        _EnableRSIsChecked = value;
        //        RaisePropertyChanged(() => EnableRSIsChecked);

        //    }
        //}
        ///// <summary>
        ///// The Portrait mode display is checked.
        ///// </summary>
        //bool _PortraitModeIsChecked;
        //public bool PortraitModeIsChecked
        //{
        //    get
        //    {
        //        return _PortraitModeIsChecked;
        //    }
        //    set
        //    {
        //        _PortraitModeIsChecked = value;
        //        RaisePropertyChanged(() => PortraitModeIsChecked);

        //    }
        //}



    }
}
