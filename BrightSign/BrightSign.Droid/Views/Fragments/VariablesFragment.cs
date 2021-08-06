
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Interface;
using BrightSign.Core.Utility.Web;
using BrightSign.Core.ViewModels;
using BrightSign.Localization;
using Java.IO;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform;
using Plugin.CurrentActivity;

namespace BrightSign.Droid.Views.Fragments
{
    //[MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, false)]
    [MvvmCross.Droid.Views.Attributes.MvxTabLayoutPresentation("Variables", Resource.Id.viewPagerDevice, Resource.Id.tabsDevice, typeof(MainViewModel))]
    public class VariablesView : MvxFragment<VariablesViewModel>
    {
        WebView webView;
        ImageButton btnHome;
        AlertDialog loginView;
        EditText userName;
        EditText password;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            base.OnCreateView(inflater, container, savedInstanceState);

            return this.BindingInflate(Resource.Layout.VariablesView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var toolbar = ((MainActivity)this.Activity).FindViewById(Resource.Id.toolbar);
            var mTitleTextView = toolbar.FindViewById<TextView>(Resource.Id.TitleText);
            if (mTitleTextView!=null)
            {
                mTitleTextView.Text = TitleType.Variables.ToString();
            }
            var btnBack = toolbar.FindViewById<Button>(Resource.Id.btnBack);
            var refreshImg = toolbar.FindViewById<ImageButton>(Resource.Id.refreshImg);
            try
            {
                refreshImg.Click -= BtnRefresh_Click;
            }
            catch (Exception ex)
            {

            }
            refreshImg.Click += BtnRefresh_Click;

            btnBack.Visibility = ViewStates.Gone;

            webView = view.FindViewById<WebView>(Resource.Id.webViewControl);
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.UseWideViewPort = true;
            webView.Settings.LoadWithOverviewMode = true;
            webView.SetWebViewClient(new CustomWebClient(this)
            {

            });

            var topView = view.FindViewById(Resource.Id.top_view);
            TextView deviceName = topView.FindViewById<TextView>(Resource.Id.top_device_name);

            //Constants.ActiveDevice = Constants.ScannedDevices[0];
            deviceName.Text = ViewModel.CurrentDevice.Name;
            TextView deviceIP = topView.FindViewById<TextView>(Resource.Id.top_device_ip);
            deviceIP.Text = "IP Address:" + ViewModel.CurrentDevice.IpAddress;
            ImageView deviceImg = topView.FindViewById<ImageView>(Resource.Id.top_deviceImg);
           
            RegisterEvents();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            if (Constants.CurrentTab == TitleType.Variables || ( Constants.CurrentTab == TitleType.Actions ))
            {
                if (ViewModel.ShowAuthenticationPopup)
                {
                    if (Constants.CurrentTab == TitleType.Variables)
                    {
                        ShowPopup();
                    }
                        webView.LoadData(ViewModel.failureHtml, "text/html", null);

                }
                else
                {
                    LoadWebView();
                }

            }

        }

        public void RegisterEvents()
        {
            try
            {
                var toolbar = ((MainActivity)this.Activity).FindViewById(Resource.Id.toolbar);
                btnHome = toolbar.FindViewById<ImageButton>(Resource.Id.btnHome);
                btnHome.Click += BtnHome_Click;

            }
            catch (Exception ex)
            {

            }
        }


        private void ShowPopup()
        {
            if (ViewModel.ViewTitle.Equals("variables", StringComparison.OrdinalIgnoreCase))
            {
                ViewModel.IsBusy = false;

                if (loginView == null)
                {
                    LayoutInflater inflater = (LayoutInflater)CrossCurrentActivity.Current.Activity.GetSystemService(Context.LayoutInflaterService);

                    View view = inflater.Inflate(Resource.Layout.AuthorizationPopup, null);

                    Button loginBtn = (Button)view.FindViewById(Resource.Id.loginBtn);
                    Button cancelBtn = (Button)view.FindViewById(Resource.Id.cancelBtn);
                    view.FindViewById<TextView>(Resource.Id.title).Text = Strings.auth_required;
                    view.FindViewById<TextView>(Resource.Id.message).Text = ViewModel.LoginPopupTitle;

                    userName = view.FindViewById<EditText>(Resource.Id.username);
                    password = view.FindViewById<EditText>(Resource.Id.password);

                    try
                    {

                        loginView = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity).SetView(view).Show();
                    }
                    catch (Exception ex)
                    {

                    }

                    WindowManagerLayoutParams lp = new WindowManagerLayoutParams();
                    Window window = loginView.Window;
                    lp.CopyFrom(window.Attributes);
                    //This makes the dialog take up the full width
                    lp.Width = WindowManagerLayoutParams.MatchParent;
                    lp.Height = WindowManagerLayoutParams.WrapContent;
                    window.Attributes = lp;

                    cancelBtn.Click += (sender, e) =>
                    {
                        loginView.Hide();
                        webView.LoadData(ViewModel.failureHtml, "text/html", null);
                    };

                    loginBtn.Click += async (sender, e) =>
                    {
                        loginView.Hide();
                        ViewModel.LoginUser = userName.Text.Trim();
                        ViewModel.LoginPwd = password.Text.Trim();
                        await ViewModel.LoginClick();
                    };
                }
                else
                {
                    loginView.Show();
                }
            }

        }

        void BtnHome_Click(object sender, EventArgs e)
        {
            ViewModel.ChangeDeviceCommand.Execute();
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.Finish();
        }

        private async void LoadWebView()
        {
            await Task.Run(async () =>
            {
                ViewModel.IsBusy = true;
                var IsDeviceReachable = await BSUtility.Instance.IsDeviceOnline();
                ViewModel.IsBusy = false;
                if (!IsDeviceReachable)
                {
                    ViewModel.VariableURL = ViewModel.VariableOfflineURL;
                    ViewModel.IsDeviceOnline = false;
                }
                else
                {
                    ViewModel.IsDeviceOnline = true;
                }

                CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
                {

                    if (ViewModel.IsDeviceOnline)
                        webView.LoadUrl(ViewModel.VariableURL);
                    else
                        webView.LoadData(ViewModel.VariableURL, "text/html", null);
                });
            });
        }

        void BtnRefresh_Click(object sender, EventArgs e)
        {
            if (ViewModel.ViewTitle.Equals("variables", StringComparison.OrdinalIgnoreCase))
                ViewModel.SetupWebView();
        }


        public async Task CreateAndSaveFile(string url)
        {
            // string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filename = Path.Combine(path, "brightsign.txt");
            string extension = url.ToLower().Contains("log") ? "log" : "dump";
            var byteData = await HttpBase.Instance.DownloadLog(url);


            StreamWriter stream = System.IO.File.CreateText(filename);
            stream.Write(byteData);
            stream.Close();


            using (var streamReader = new StreamReader(filename))
            {
                string content = streamReader.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(content);
            }


            Intent emailIntent = new Intent(Intent.ActionSend);
            emailIntent.SetType("text/plain");
            emailIntent.PutExtra(Android.Content.Intent.ExtraEmail,
                new String[] { "kishore@gmail.com" });
            emailIntent.PutExtra(Intent.ExtraSubject, "Log File");
            emailIntent.PutExtra(Intent.ExtraText, "Brightsign Log File");

            var uri = Android.Net.Uri.Parse("file://" + filename);

            emailIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
            emailIntent.PutExtra(Intent.ExtraStream, uri);
            emailIntent.SetType("message/rfc822");
            ((MainActivity)this.Activity).StartActivity(Intent.CreateChooser(emailIntent, "Email"));
        }


        void BtnCamera_Click(object sender, EventArgs e)
        {
            ViewModel.SnapshotCommand.Execute();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("VariableURL"))
            {
                if (ViewModel.IsDeviceOnline)
                    webView.LoadUrl(ViewModel.VariableURL);
                else
                    webView.LoadData(ViewModel.VariableURL, "text/html", null);
            }
            else if (e.PropertyName.Equals("RefreshWebview"))
            {
                if (ViewModel.RefreshWebview)
                {
                    webView.Reload();
                    ViewModel.RefreshWebview = false;
                }
            }
            else if (e.PropertyName.Equals("ShowAuthenticationPopup"))
            {
                if (Constants.CurrentTab == TitleType.Variables)
                {


                    if (ViewModel.ShowAuthenticationPopup)
                    {
                        ShowPopup();
                        webView.LoadData(ViewModel.failureHtml, "text/html", null);
                    }
                    else
                    {
                        if (loginView != null)
                        {
                            loginView.Hide();
                        }
                    }
                }
            }
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            btnHome.Click -= BtnHome_Click;

        }



    }

    public class CustomWebClient : WebViewClient
    {

        VariablesView vFragment;
        DiagnosticsFragment dFragment;

        public CustomWebClient(Object fragment)
        {
            if (fragment is VariablesView)
                vFragment = (VariablesView)fragment;
            else
                dFragment = (DiagnosticsFragment)fragment;
        }
        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            view.LoadUrl(request.Url.ToString());
            return false;
        }

        public override void OnReceivedError(WebView view, IWebResourceRequest request, WebResourceError error)
        {
            //base.OnReceivedError(view, request, error);
            view.LoadData(String.Format("<body><br/><br/><h1 align=\"center\" style=\"font-size: 4em;\">Could not access web server for BrightSign unit {0}</h1><br/><p align=\"center\" style=\"font-size: 3em;\">Please touch Settings button at upper right to check selected unit</p></body>", Constants.ActiveDevice.Name), "text/html", null);

        }

        public override void OnPageStarted(WebView view, string url, Android.Graphics.Bitmap favicon)
        {
            if (vFragment != null && Constants.CurrentTab == TitleType.Variables)
            {
                vFragment.ViewModel.IsBusy = true;
            }
            else if (dFragment != null && Constants.CurrentTab == TitleType.Diagnostics)
            {
                dFragment.ViewModel.IsBusy = true;

                if (url.ToLower().Contains("_log") || url.ToLower().Contains("dump"))
                {

                    dFragment.ViewModel.DownloadLogCommand.Execute(url);
                }
                else if (url.Contains("set_password"))
                {
                    dFragment.ViewModel.diagnosticPageAuthorized = false;
                    dFragment.ViewModel.SetupWebView();
                    return;
                }
                else if (url.Contains("clear_password"))
                {
                    dFragment.ViewModel.diagnosticPageAuthorized = true;
                    dFragment.ViewModel.SetupWebView();
                    return;
                }
                else if (url.Contains(Constants.httpPort.ToString()) && !url.Contains("bas.html"))
                {
                    if (Constants.IsCredentialsRequiredforSnapshots)
                    {
                        if (!url.Contains("@"))
                        {
                            char[] charsToTrim = { 'h', 't', 't', 'p', ':', '/', '/' };

                            string loadurl = string.Format("http://{0}:{1}@{2}bas.html#/", Constants.LoginUser, Constants.LoginPwd, url.TrimStart(charsToTrim));

                            view.LoadUrl(loadurl);

                            return;
                        }
                    }

                }


            }
            base.OnPageStarted(view, url, favicon);
        }
        public override void OnPageFinished(WebView view, string url)
        {
            Task.Delay(1500);
            if (vFragment != null && Constants.CurrentTab == TitleType.Variables)
            {

                vFragment.ViewModel.IsBusy = false;
            }
            else if (dFragment != null && Constants.CurrentTab == TitleType.Diagnostics)
            {
                dFragment.ViewModel.IsBusy = false;
            }
        }

        public override void OnReceivedHttpAuthRequest(WebView view, HttpAuthHandler handler, string host, string realm)
        {
            if(dFragment!=null)
            {
                if (dFragment.ViewModel.diagnosticPageAuthorized && !String.IsNullOrEmpty(dFragment.ViewModel.LoginPwd))
                {
                    handler.Proceed(dFragment.ViewModel.LoginUser, dFragment.ViewModel.LoginPwd);
                    base.OnReceivedHttpAuthRequest(view, handler, host, realm);
                }
                else
                {
                    base.OnReceivedHttpAuthRequest(view, handler, host, realm);
                }
            }
            else
            {
                base.OnReceivedHttpAuthRequest(view, handler, host, realm);
            }
            
        }
    }




}
