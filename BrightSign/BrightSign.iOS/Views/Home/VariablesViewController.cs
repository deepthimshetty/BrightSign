using System;
using Acr.UserDialogs;
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
	[MvxTabPresentation(WrapInNavigationController = true, TabName = "Variables", TabIconName = "variables_new")]
	public partial class VariablesViewController : BaseView<VariablesViewModel>, IUIWebViewDelegate
	{
		UIAlertView loginView;
        public VariablesViewController() : base("VariablesViewController", null)
        {
        }

		public override void ViewDidLoad()
		{
			try
			{
				base.ViewDidLoad();

				Title = ViewModel.ViewTitle;

				ViewModel.IsBusy = true;

				//Left Barbutton Item
				UIBarButtonItem refreshButton = new UIBarButtonItem(UIBarButtonSystemItem.Refresh, RefreshBarButtonItemAction);




				//activityView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
				//activityView.StartAnimating();
				//UIBarButtonItem activityButton = new UIBarButtonItem(activityView);
				//UIBarButtonItem[] leftBarButtonItems = new UIBarButtonItem[] { refreshButton, activityButton };



				//NavigationItem.LeftBarButtonItems = leftBarButtonItems;

				//Right Barbutton Items
				//UIBarButtonItem snapshotsButton = new UIBarButtonItem(UIImage.FromFile("86-camera.png"), UIBarButtonItemStyle.Plain, SnapshotBarButtonItemAction);
				//UIBarButtonItem settingsButton = new UIBarButtonItem(UIImage.FromFile("19-gear.png"), UIBarButtonItemStyle.Plain, SettingsBarButtonItemAction);

				//UIBarButtonItem[] rightBarButtonItems = new UIBarButtonItem[] { settingsButton, snapshotsButton };
				//NavigationItem.RightBarButtonItems = rightBarButtonItems;

				NavigationItem.RightBarButtonItem = refreshButton;

				ViewModel.PropertyChanged += ViewModel_PropertyChanged;

                deviceInfoView.SetContext(ViewModel.CurrentDevice);
				 
				this.CreateBinding(deviceInfoView).For(o => o.DataContext).To((VariablesViewModel vm) => vm.CurrentDevice).Apply();
				deviceInfoView.SelectDeviceClicked += (arg1, arg2) =>
				{
					ViewModel.ChangeDeviceCommand.Execute();
					return null;
				};

				if (ViewModel.ShowAuthenticationPopup)
				{
					ShowPopup();
                    webBrowserView.LoadHtmlString(ViewModel.failureHtml, null);
				}
				else
				{
					if (Constants.ActiveDevice != null && ViewModel.IsDeviceOnline)
                        webBrowserView.LoadRequest(new NSUrlRequest(new NSUrl(ViewModel.VariableURL)));
                    else
                        webBrowserView.LoadHtmlString(ViewModel.VariableURL, null);

				}

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
            if (!webBrowserView.IsLoading)
            {
                webBrowserView.EndEditing(true);
                webBrowserView.ResignFirstResponder();
                ViewModel.IsBusy = true;
            }
        }

        [Foundation.Export("webViewDidFinishLoad:")]
        public virtual void LoadingFinished(UIWebView webView)
        {
            ViewModel.IsBusy = false;
        }

        [Foundation.Export("webView:didFailLoadWithError:")]
        public virtual void LoadFailed(UIWebView webView, NSError error)
        {
            ViewModel.IsBusy = false;
            ViewModel.loadFailureDisplayed = true;
            ViewModel.webPageLoadPending = false;
            webBrowserView.LoadHtmlString(ViewModel.LoadFailureURL, null);
        }

		void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("VariableURL"))
			{
				if (Constants.ActiveDevice != null && ViewModel.IsDeviceOnline)
					webBrowserView.LoadRequest(new NSUrlRequest(new NSUrl(ViewModel.VariableURL)));
				else
					webBrowserView.LoadHtmlString(ViewModel.VariableURL, null);
			}
			else if (e.PropertyName.Equals("RefreshWebview"))
			{
				if (ViewModel.RefreshWebview)
				{
					webBrowserView.Reload();
					ViewModel.RefreshWebview = false;
				}
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
			loginView.GetTextField(0).Text = "";

			loginView.Show();
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


				ViewModel.LoginUser = loginView.GetTextField(0).Text;
                
				ViewModel.LoginPwd = loginView.GetTextField(1).Text;
				ViewModel.LoginCommand.Execute();
			}


		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		void RefreshBarButtonItemAction(object sender, EventArgs e)
		{
			//if (Constants.ActiveDevice != null && ViewModel.IsDeviceOnline)
			//    webBrowserView.LoadRequest(new NSUrlRequest(new NSUrl(ViewModel.VariableURL)));
			//else
			//webBrowserView.LoadHtmlString(ViewModel.VariableURL, null);
			ViewModel.SetupWebView();
		}

		void SnapshotBarButtonItemAction(object sender, EventArgs e)
		{
			ViewModel.SnapshotCommand.Execute();
		}

		void SettingsBarButtonItemAction(object sender, EventArgs e)
		{
			ViewModel.SettingsCommand.Execute();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			TabBarController.TabBar.Hidden = false;
			//UserDialogs.Init(dummyfunction);
		}

		private UIViewController dummyfunction()
		{
			return this;
		}

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            deviceInfoView.SetNeedsDisplay();
            RefreshNavigationBar();
        }

    }
}

