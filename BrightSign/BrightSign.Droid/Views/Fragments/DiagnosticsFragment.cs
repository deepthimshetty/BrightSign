
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using BrightSign.Core.Utility;
using BrightSign.Core.ViewModels;
using BrightSign.Localization;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using Plugin.CurrentActivity;

namespace BrightSign.Droid.Views.Fragments
{
    [MvvmCross.Droid.Views.Attributes.MvxTabLayoutPresentation("Diagnostics", Resource.Id.viewPagerDevice, Resource.Id.tabsDevice, typeof(MainViewModel))]
    public class DiagnosticsFragment : MvxFragment<DiagnosticsViewModel>
    {
        WebView webView;
        ImageButton btnHome;
        AlertDialog loginView;
        EditText userName;
        EditText password;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            ((MainActivity)this.Activity).HideKeybord();
            return this.BindingInflate(Resource.Layout.DiagnosticsView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            webView = view.FindViewById<WebView>(Resource.Id.webViewControl);

            var toolbar = ((MainActivity)this.Activity).FindViewById(Resource.Id.toolbar);

            var refreshImg = toolbar.FindViewById<ImageButton>(Resource.Id.refreshImg);
            try
            {
                refreshImg.Click -= BtnRefresh_Click;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            refreshImg.Click += BtnRefresh_Click;


            var topView = view.FindViewById(Resource.Id.actions_top_view);
            TextView deviceName = topView.FindViewById<TextView>(Resource.Id.top_device_name);
            deviceName.Text = ViewModel.CurrentDevice.Name;
            TextView deviceIP = topView.FindViewById<TextView>(Resource.Id.top_device_ip);
            deviceIP.Text = "IP Address:" + ViewModel.CurrentDevice.IpAddress;
            ImageView deviceImg = topView.FindViewById<ImageView>(Resource.Id.top_deviceImg);

            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.LoadWithOverviewMode = true;
            webView.Settings.BuiltInZoomControls = true;
            webView.Settings.DisplayZoomControls = true;
            webView.Settings.UseWideViewPort = true;
            WebView.SetWebContentsDebuggingEnabled(true);
            webView.SetWebViewClient(new CustomWebClient(this));
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            RegisterEvents();



        }

        private void ShowPopup()
        {
            if (ViewModel.ViewTitle.Equals("diagnostics", StringComparison.OrdinalIgnoreCase))
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

                    userName.Text = "admin";
                    userName.Enabled = false;

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
                        ViewModel.LoginPwd = password.Text;
                        await ViewModel.LoginClick();
                    };
                }
                else
                {
                    loginView.Show();
                }
            }

        }

        void BtnRefresh_Click(object sender, EventArgs e)
        {
            if (ViewModel.ViewTitle.Equals("diagnostics", StringComparison.OrdinalIgnoreCase))
            {
                ViewModel.SetupWebView();

            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("DiagnosticsURL"))
            {
                if (ViewModel.IsDeviceOnline)
                {
                    webView.LoadUrl(ViewModel.DiagnosticsURL);
                }
                else
                    webView.LoadData(ViewModel.DiagnosticsURL, "text/html", null);
            }
            else if (e.PropertyName.Equals("ShowAuthenticationPopup"))
            {
				if (Constants.CurrentTab == TitleType.Diagnostics)
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
			else if (e.PropertyName=="showEmailComposer")
			{
				if (ViewModel.showEmailComposer)
				{

				}
			}
		}

		public void RegisterEvents()
        {
            try
            {
                var toolbar = ((MainActivity)this.Activity).FindViewById(Resource.Id.toolbar);
                btnHome = toolbar.FindViewById<ImageButton>(Resource.Id.btnHome);
                try
                {
                    btnHome.Click -= btnHome_Click;
                    Constants.DiagnosticsTabSelected -= OnDiagnosticsTabSelected;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                btnHome.Click += btnHome_Click;

                Constants.DiagnosticsTabSelected += OnDiagnosticsTabSelected;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override void OnPause()
        {
            base.OnPause();
            btnHome.Click -= btnHome_Click;
            Constants.DiagnosticsTabSelected -= OnDiagnosticsTabSelected;
        }
        async void OnDiagnosticsTabSelected()
        {
            await Task.Run(async () =>
            {
                bool isReachable = false;
                ViewModel.IsBusy = true;

                //Task.Run(async () =>
                //{
                ViewModel.IsDeviceOnline = isReachable = await BSUtility.Instance.IsDeviceOnline();
                //}).Wait();


                CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
                {
                    //ViewModel.IsBusy = false;
                    if (Constants.CurrentTab == TitleType.Diagnostics)
                    {

                        if (isReachable)
                        {

                            if (ViewModel.ShowAuthenticationPopup && !ViewModel.diagnosticPageAuthorized)
                            {
                                if (!ViewModel.IsPopupShown)
                                {
                                    ShowPopup();
                                    webView.LoadData(ViewModel.failureHtml, "text/html", null);
                                }
                            }
                            else
                            {
                                if (ViewModel.IsDeviceOnline)
                                    webView.LoadUrl(ViewModel.DiagnosticsURL);
                                else
                                    webView.LoadData(ViewModel.DiagnosticsURL, "text/html", null);
                            }
                        }
                        else
                        {
                            webView.LoadData(ViewModel.VariableOfflineURL, "text/html", null);
                        }
                    }
                });
            });
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            try
            {
                btnHome.Click -= btnHome_Click;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Constants.DiagnosticsTabSelected -= OnDiagnosticsTabSelected;
        }

        void btnHome_Click(object sender, EventArgs e)
        {
            webView.ClearCache(true);
            ViewModel.ChangeDeviceCommand.Execute();
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.Finish();
        }
    }
}
