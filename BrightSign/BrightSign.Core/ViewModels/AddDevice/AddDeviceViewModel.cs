using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Interface;
using BrightSign.Core.Utility.Messages;
using BrightSign.Core.Utility.Web;
using BrightSign.Localization;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace BrightSign.Core.ViewModels.AddDevice
{
    public class AddDeviceViewModel : BaseViewModel
    {
        IDialogService dialogservice;


        BSDeviceTemp _bsdeviceAdd;
        public BSDeviceTemp bsdeviceAdd
        {
            get
            {
                return _bsdeviceAdd;
            }
            set
            {
                _bsdeviceAdd = value;
                RaisePropertyChanged(() => bsdeviceAdd);
            }
        }

        public AddDeviceViewModel(IDialogService _dialogservice, IMvxMessenger messenger) : base(messenger)
        {
            ViewTitle = "Search Units";
            dialogservice = _dialogservice;
            Error = string.Empty;
            Description = Strings.enter_ipaddress_desc;
            AddButtonVisibility = false;
            IPAddress = string.Empty;
//#if DEBUG
//            IPAddress = "192.168.0.108";
//#endif 
            bsdeviceAdd = new BSDeviceTemp();
        }


        bool _AddButtonVisibility;
        public bool AddButtonVisibility
        {
            get
            {
                return _AddButtonVisibility;
            }
            set
            {
                _AddButtonVisibility = value;
                RaisePropertyChanged("AddButtonVisibility");
            }
        }


        string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                RaisePropertyChanged("Description");
            }
        }

        string _Error;
        public string Error
        {
            get
            {
                return _Error;
            }
            set
            {
                _Error = value;
                RaisePropertyChanged("Error");
            }
        }

        string _IPAddress;
        public string IPAddress
        {
            get
            {
                return _IPAddress;
            }
            set
            {
                _IPAddress = value;
                RaisePropertyChanged("IPAddress");
            }
        }

        public IMvxCommand CancelCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Close(this);
                });
            }
        }


        public IMvxCommand SearchCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await SearchClick();
                });
            }
        }

        private async Task SearchClick()
        {
            if (IsValidIPAddress(IPAddress))
            {
                try
                {
                    IsBusy = true;
                    var searchResponse = await HttpBase.Instance.GetDeviceStatus(IPAddress, "8080");
                    if (searchResponse.Item1)
                    {
                        bsdeviceAdd = BSUtility.Instance.ParseDeviceInfoXMLDummy(searchResponse.Item2);
                        bsdeviceAdd.IpAddress = IPAddress;
                        bsdeviceAdd.IsOnline = true;
                        if (!string.IsNullOrEmpty(bsdeviceAdd.Name))
                        {
                            AddButtonVisibility = true;
                            Error = String.Empty;
                            RaisePropertyChanged(() => bsdeviceAdd);
                        }
                    }
                    else
                    {
                        Error = Strings.search_error;
                        AddButtonVisibility = false;
                        await dialogservice.ShowAlertAsync(Strings.search_error, Strings.error, Strings.ok);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Error = Strings.search_error;
                    AddButtonVisibility = false;
                }
                finally
                {
                    IsBusy = false;
                }


            }
        }



        public IMvxCommand AddCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    AddDeviceClick();
                });
            }
        }

        private void AddDeviceClick()
        {
            bsdeviceAdd.IsOnline = true;

            var item = Constants.FullDevices.FirstOrDefault(o => o.IpAddress == bsdeviceAdd.IpAddress);
            if (item != null)
            {
                // Item already present in the list
                dialogservice.ShowAlertAsync(Strings.devicealreadypresenterror, Strings.error, Strings.ok);
            }
            else
            {
                Close(this);
                BSDevice bsDevice = bsdeviceAdd.GetBSdeviceObj();
                Messenger.Publish(new AddDeviceRefreshMessage(this, bsDevice));
                Mvx.Resolve<ICustomAlert>().ShowCustomAlert(true, bsDevice.Name, Strings.addedtolist);
            }
        }




    }
}
