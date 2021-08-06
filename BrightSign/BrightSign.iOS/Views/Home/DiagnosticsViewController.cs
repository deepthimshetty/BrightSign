using System;
using System.Threading.Tasks;
using BrightSign.Core.Utility;
using BrightSign.Core.ViewModels;
using BrightSign.iOS.Utility.Interface;
using BrightSign.iOS.Views.CustomViews;
using BrightSign.Localization;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using UIKit;

namespace BrightSign.iOS.Views.Home
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Diagnostics", TabIconName = "diagnostics_new")]
    public partial class DiagnosticsViewController : BaseView<DiagnosticsViewModel>, IUIWebViewDelegate
    {
        UIAlertView loginView;
        public DiagnosticsViewController() : base("DiagnosticsViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                Title = ViewModel.ViewTitle;

                ViewModel.IsBusy = true;

                UIBarButtonItem refreshButton = new UIBarButtonItem(UIBarButtonSystemItem.Refresh, RefreshBarButtonItemAction);

                NavigationItem.RightBarButtonItem = refreshButton;


                //if (ViewModel.ShowAuthenticationPopup)
                //{
                //    if (!ViewModel.IsPopupShown)
                //    {
                //        ShowPopup();
                //        webBrowserView.LoadHtmlString(ViewModel.failureHtml, null);
                //    }
                //}
                //else
                //{
                //    if (Constants.ActiveDevice != null && ViewModel.IsDeviceOnline)
                //        webBrowserView.LoadRequest(new NSUrlRequest(new NSUrl(ViewModel.DiagnosticsURL)));
                //    else
                //        webBrowserView.LoadHtmlString(ViewModel.DiagnosticsURL, null);
                //}

                ViewModel.IsBusy = true;
                ViewModel.SetupWebView();

                deviceInfoView.SetContext(ViewModel.CurrentDevice);
                this.CreateBinding(deviceInfoView).For(o => o.DataContext).To((VariablesViewModel vm) => vm.CurrentDevice).Apply();
                deviceInfoView.SelectDeviceClicked += (arg1, arg2) =>
                {
                    ViewModel.ChangeDeviceCommand.Execute();
                    return null;
                };


                ViewModel.PropertyChanged += ViewModel_PropertyChanged;

                webBrowserView.Delegate = this;
            }
            catch (Exception ex)
            {

            }
            // Perform any additional setup after loading the view, typically from a nib.
        }

        [Foundation.Export("webViewDidStartLoad:")]
        public virtual void LoadStarted(UIWebView webView)
        {
            ViewModel.IsBusy = true;
        }


        [Foundation.Export("webView:didFailLoadWithError:")]
        public virtual void LoadFailed(UIWebView webView, NSError error)
        {
            ViewModel.IsBusy = false;
        }

        [Foundation.Export("webView:shouldStartLoadWithRequest:navigationType:")]
        bool WebBrowserView_ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            if (request.Url != null)
            {
                
                if (request.Url.AbsoluteString.Contains("_log") || request.Url.AbsoluteString.Contains("dump"))
                {
                    ViewModel.DownloadLogCommand.Execute(request.Url.AbsoluteUrl);
                    return false;

                }
                else if (request.Url.AbsoluteString.Contains("set_password"))
                {
                    ViewModel.diagnosticPageAuthorized = false;
                    ViewModel.SetupWebView();
                    //return true;
                }
                else if (request.Url.AbsoluteString.Contains("clear_password"))
                {
                    ViewModel.diagnosticPageAuthorized = true;
                    ViewModel.SetupWebView();
                    //return true;
                }
                else if (request.Url.AbsoluteString.Contains(Constants.httpPort.ToString()) && !request.Url.AbsoluteString.Contains("bas.html"))
                {
                    if (Constants.IsLWSCredentialsRequired)
                    {
                        if (Constants.IsCredentialsRequiredforSnapshots)
                        {
                            if (!request.Url.AbsoluteString.Contains("@"))
                            {
                                //http://192.168.35.146:8080/bas.html#/
                                char[] charsToTrim = { 'h', 't', 't', 'p', ':', '/', '/' };

                                string url = string.Format("http://{0}:{1}@{2}bas.html#/", Constants.LoginUser, Constants.LoginPwd, request.Url.AbsoluteString.TrimStart(charsToTrim));

                                webView.LoadRequest(new NSUrlRequest(NSUrl.FromString(url)));
                                return false;
                            }
                        }
                        else
                        {
                            new DialogService().ShowAlertAsync("Please enter valid credentials in variables page.", Strings.error, Strings.ok);
                            return false;
                        }
                    }


                }

            }
            return true;
        }

        [Foundation.Export("webViewDidFinishLoad:")]
        public virtual void LoadingFinished(UIWebView webView)
        {
            ViewModel.IsBusy = false;
        }


        private void ShowPopup()
        {
            ViewModel.IsBusy = false;
            loginView = new UIAlertView() { Title = Strings.auth_required, Message = ViewModel.LoginPopupTitle };
            loginView.AddButton("Cancel");
            loginView.CancelButtonIndex = 0;

            loginView.AddButton("Login");
            loginView.Clicked += LoginView_Clicked;

            loginView.AlertViewStyle = UIAlertViewStyle.LoginAndPasswordInput;
            loginView.GetTextField(0).Text = "admin";
            loginView.GetTextField(0).Enabled = false;

            loginView.Show();
            
        }


        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("DiagnosticsURL"))
            {
               

                if (Constants.ActiveDevice != null && ViewModel.IsDeviceOnline)
                    webBrowserView.LoadRequest(new NSUrlRequest(new NSUrl(ViewModel.DiagnosticsURL)));
                else
                    webBrowserView.LoadHtmlString(ViewModel.DiagnosticsURL, null);
            }
            else if (e.PropertyName == "ShowAuthenticationPopup")
            {
                if (ViewModel.ShowAuthenticationPopup)
                {
                    ShowPopup();
                    webBrowserView.LoadHtmlString(ViewModel.failureHtml, null);
                }
                else
                {
                    if (loginView != null)
                    {
                        loginView.Hidden = true;
                    }

                }
            }
            else if(e.PropertyName == "diagnosticPageAuthorized")
            {
                if (ViewModel.diagnosticPageAuthorized)
                {
                    // Probably store the credentials
                    NSUrlCredential credential = new NSUrlCredential(ViewModel.LoginUser, ViewModel.LoginPwd, NSUrlCredentialPersistence.ForSession);
                    NSUrlCredentialStorage credentialStorage = NSUrlCredentialStorage.SharedCredentialStorage;
                    NSUrlProtectionSpace pSpace = new NSUrlProtectionSpace(Constants.ActiveDevice.IpAddress, 80, "http", "brightsign", "Digest");
                    credentialStorage.SetCredential(credential, pSpace);
                }


            }
        }

        void LoginView_Clicked(object sender, UIButtonEventArgs e)
        {
            if (loginView != null)
            {
                loginView.GetTextField(1).ResignFirstResponder();
                loginView.GetTextField(1).EndEditing(true);
                loginView.GetTextField(1).ShouldReturn += (textField) =>
                {
                    textField.ResignFirstResponder();
                    return true;
                };
            }

            if (e.ButtonIndex == 1)
            {
                loginView.Hidden = true;
                ViewModel.LoginPwd = loginView.GetTextField(1).Text;
                ViewModel.LoginCommand.Execute();
            }


        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        void BackBarButtonItemAction(object sender, EventArgs e)
        {

        }

        void ForwardBarButtonItemAction(object sender, EventArgs e)
        {

        }

        void RefreshBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.SetupWebView();
            //if (Constants.ActiveDevice != null && ViewModel.IsDeviceOnline)
            //    ViewModel.SetupWebView();
            //else
            //webBrowserView.LoadHtmlString(ViewModel.DiagnosticsURL, null);
        }

        void SettingsBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.SettingsCommand.Execute();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            TabBarController.TabBar.Hidden = false;
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            deviceInfoView.SetNeedsDisplay();
            RefreshNavigationBar();
        }
    }
}

