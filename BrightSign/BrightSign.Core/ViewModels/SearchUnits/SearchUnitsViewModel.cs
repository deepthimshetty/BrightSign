using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.ViewModels.Units;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Zeroconf;

namespace BrightSign.Core.ViewModels.SearchUnits
{
    public class SearchUnitsViewModel : BaseViewModel
    {



        bool _AcquireLock = false;
        public bool AcquireLock
        {
            get
            {
                return _AcquireLock;
            }
            set
            {
                _AcquireLock = value;
                RaisePropertyChanged("AcquireLock");
            }
        }

        string _version;
        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
                RaisePropertyChanged("Version");
            }
        }
        private async Task ScanDevices()
        {
            //ClearTimer();
            InvokeOnMainThread(async () =>
            {
                AcquireLock = true;

                await BSUtility.Instance.EnumerateAllServicesFromAllHosts();

                AcquireLock = false;

                ShowViewModel<UnitsViewModel>();

                //ShowViewModel<SnapshotsViewModel>();


            });

        }

        public SearchUnitsViewModel(IMvxMessenger messenger) : base(messenger)
        {
            Constants.IsCredentialsRequiredforSnapshots = false;
            Constants.IsLWSCredentialsRequired = false;
            Task.Run(async () =>
            {
                await ScanDevices();
            });

        }



    }
}
