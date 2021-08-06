using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Messages;
using BrightSign.Core.Utility.Web;
using BrightSign.Core.ViewModels.AddDevice;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;



namespace BrightSign.Core.ViewModels
{
    public class VariablesViewModel : BaseViewModel
    {
        MvxSubscriptionToken AciveDeviceToken;
        public string LoginUser = string.Empty;
        public string LoginPopupTitle;
        public string LoginPwd = string.Empty;

        public string failureHtml = String.Format("<body><br/><br/><h1 align=\"center\" style=\"font-size: 4em;\">You must enter a valid user name and password to access the Variables page on BrightSign unit {0}</h1><br/><p align=\"center\" style=\"font-size: 3em;\">You can touch the Refresh button to try again.</p></body>", Constants.ActiveDevice.Name);


        public VariablesViewModel(IMvxMessenger messenger) : base(messenger)
        {
            ShowAuthenticationPopup = false;
            IsDeviceOnline = true;
            SetupWebView();
            if (AciveDeviceToken == null)
            {
                AciveDeviceToken = Messenger.Subscribe<ActiveDeviceMessage>(OnDeviceStatusResponse);
            }
            ViewTitle = "Variables";

            LoginPopupTitle = string.Format("{0}Variables", Constants.ActiveDevice.Name);
        }

        private bool _ShowAuthenticationPopup;
        public bool ShowAuthenticationPopup
        {
            get { return _ShowAuthenticationPopup; }
            set
            {
                _ShowAuthenticationPopup = value;

                RaisePropertyChanged(() => ShowAuthenticationPopup);
            }
        }

        private bool _variablesPageAuthorized;
        public bool variablesPageAuthorized
        {
            get { return _variablesPageAuthorized; }
            set
            {
                _variablesPageAuthorized = value;
                RaisePropertyChanged(() => variablesPageAuthorized);
            }
        }

        private void OnDeviceStatusResponse(ActiveDeviceMessage obj)
        {
            IsDeviceOnline = true;
            VariableURL = String.Format("http://{0}:8008/", Constants.ActiveDevice?.IpAddress);
        }

        public string VariableOfflineURL =
            "<body><br/><br/><h1 align=\"center\" style=\"font-size: 4em;\">A BrightSign unit could not be found on the local network</h1><br/><p align=\"center\" style=\"font-size: 3em;\">You can touch the Refresh button to try again.</p></body>";

        public string LoadFailureURL =
            "<body><br/><br/><h1 align=\"center\" style=\"font-size: 4em;\">A BrightSign unit could not be found on the local network</h1><br/><p align=\"center\" style=\"font-size: 3em;\">You can touch the Refresh button to try again.</p></body>";


        private string _variableURL;
        public string VariableURL
        {
            get { return _variableURL; }
            set
            {
                _variableURL = value;
                RaisePropertyChanged(() => VariableURL);
            }
        }

        private bool _RefreshWebview;
        public bool RefreshWebview
        {
            get { return _RefreshWebview; }
            set
            {
                _RefreshWebview = value;
                RaisePropertyChanged(() => RefreshWebview);
            }
        }

        private bool _webPageLoadPending;
        public bool webPageLoadPending
        {
            get { return _webPageLoadPending; }
            set
            {
                _webPageLoadPending = value;
                RaisePropertyChanged(() => webPageLoadPending);
            }
        }


        private bool _loadFailureDisplayed;
        public bool loadFailureDisplayed
        {
            get { return _loadFailureDisplayed; }
            set
            {
                _loadFailureDisplayed = value;
                RaisePropertyChanged(() => loadFailureDisplayed);
            }
        }


        private bool _contentPageAuthorized;
        public bool contentPageAuthorized
        {
            get { return _contentPageAuthorized; }
            set
            {
                _contentPageAuthorized = value;
                RaisePropertyChanged(() => contentPageAuthorized);
            }
        }


        private bool _IsDeviceOnline;
        public bool IsDeviceOnline
        {
            get { return _IsDeviceOnline; }
            set
            {
                _IsDeviceOnline = value;
                RaisePropertyChanged(() => IsDeviceOnline);
            }
        }


        public async void SetupWebView()
        {
            await Task.Run(async () =>
            {
                InvokeOnMainThread(() =>
                {
                    IsBusy = true;
                });
                IsDeviceOnline = await BSUtility.Instance.IsDeviceOnline();

                System.Diagnostics.Debug.WriteLine("IsDeviceOnline:" + IsDeviceOnline);
                if (!IsDeviceOnline)
                {
                    VariableURL = VariableOfflineURL;
                }
                else
                {
                    //if (IsDeviceOnline)
                    //{
                    //	VariableURL = String.Format("http://{0}:8008/", Constants.ActiveDevice?.IpAddress);
                    //	LoadFailureURL = string.Format("<body><br/><br/><h1 align=\"center\" style=\"font-size: 4em;\">Could not access web server for BrightSign unit {0}</h1><br/><p align=\"center\" style=\"font-size: 3em;\">You can touch the Refresh button to try again.</p></body>", Constants.ActiveDevice.Name);
                    //}
                    //else
                    //	VariableURL = VariableOfflineURL;

                    //Debug.WriteLine("VariableURL " + VariableURL);

                    //Task.Run(async () =>
                    //{
                    if (!variablesPageAuthorized)
                    {

                        variablesPageAuthorized = await HttpBase.Instance.IsAuthorized(Constants.ActiveDevice.IpAddress, portnumber: "8008");

                        if (variablesPageAuthorized)
                        {
                            VariableURL = String.Format("http://{0}:8008/", Constants.ActiveDevice?.IpAddress);
                            Constants.IsLWSCredentialsRequired = false;
                        }
                        else
                        {
                            Constants.BSSnapshotList = new System.Collections.Generic.List<Models.BSSnapshot>();
                            ShowAuthenticationPopup = true;
                            Constants.IsLWSCredentialsRequired = true;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(LoginPwd))
                        {
                            VariableURL = string.Format("http://{0}:{1}@{2}", LoginUser, LoginPwd, String.Format("{0}:8008/", Constants.ActiveDevice?.IpAddress));
                        }
                        else
                        {
                            VariableURL = String.Format("http://{0}:8008/", Constants.ActiveDevice?.IpAddress);
                        }

                    }

                    // });


                }

                InvokeOnMainThread(() =>
                {
                    IsBusy = false;
                });
            });



            ViewTitle = "Variables";
        }

        public IMvxCommand LoginCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await LoginClick();
                });
            }
        }

        public async Task LoginClick()
        {
            if (!string.IsNullOrEmpty(LoginPwd))
            {
                InvokeOnMainThread(() =>
                {
                    IsBusy = true;
                });


                var status = await HttpBase.Instance.Login(LoginUser, LoginPwd, "8008");

                if (status == 1)
                {

                    Constants.IsCredentialsRequiredforSnapshots = true;

                    bool IsSnapshotsDownloaded = Constants.IsSnapShotsConfigurable = await BSUtility.Instance.GetSnapshotConfiguration();


                    variablesPageAuthorized = true;
                    //VariableURL = string.Format("http://{0}:{1}@{2}/index.html", LoginUser, LoginPwd, Constants.ActiveDevice?.IpAddress); String.Format("http://{0}:8008/", Constants.ActiveDevice?.IpAddress);
                    VariableURL = string.Format("http://{0}:{1}@{2}", LoginUser, LoginPwd, String.Format("{0}:8008/", Constants.ActiveDevice?.IpAddress));
                    Constants.LoginPwd = LoginPwd;
                    Constants.LoginUser = LoginUser;
                    ShowAuthenticationPopup = false;
                    Mvx.Resolve<IMvxMessenger>().Publish(new ImageRefreshMessage(this));
                }
                else
                {
                    IsBusy = false;
                    ShowAuthenticationPopup = true;
                }

                //DiagnosticsURL = String.Format("http://{0}", Constants.ActiveDevice?.IpAddress);

            }
        }



        public IMvxCommand SettingsCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    SettingsClick();
                });
            }
        }

        private void SettingsClick()
        {
            ShowViewModel<SettingsViewModel>();
        }

        public IMvxCommand SnapshotCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    SnapshotClick();
                });
            }
        }

        private void SnapshotClick()
        {
            ShowViewModel<SnapshotsViewModel>();
        }

    }
}
