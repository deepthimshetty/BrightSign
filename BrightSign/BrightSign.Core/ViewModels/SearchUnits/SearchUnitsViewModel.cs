using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.ViewModels.Units;
using MvvmCross.ViewModels;
using MvvmCross.Plugin.Messenger;
using Zeroconf;
using MvvmCross.Navigation;

namespace BrightSign.Core.ViewModels.SearchUnits
{
    public class SearchUnitsViewModel : BaseViewModel
    {

        //private readonly IMvxNavigationService _navigationService;

        public SearchUnitsViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
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

                //ShowViewModel<UnitsViewModel>();
                await NavigateToViewmodelMethod();

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

        public async Task NavigateToViewmodelMethod()
        {
            await _navigationService.Navigate<UnitsViewModel>();
        }


    }
}
