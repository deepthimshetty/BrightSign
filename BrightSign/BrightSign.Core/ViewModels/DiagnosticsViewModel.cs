using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Interface;
using BrightSign.Core.Utility.Messages;
using BrightSign.Core.Utility.Web;
using BrightSign.Localization;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Plugins.Email;
using System.Collections.Generic;

namespace BrightSign.Core.ViewModels
{
	public class DiagnosticsViewModel : BaseViewModel
	{
		MvxSubscriptionToken AciveDeviceToken;
		IDialogService dialogService;
		public string LoginUser = "admin";
		public string LoginPopupTitle;
		public string LoginPwd = string.Empty;
		public bool IsPopupShown = false;
		public string attachmentfileName = String.Empty;
		public bool showEmailComposer = false;
		public string failureHtml = String.Format("<body><br/><br/><h1 align=\"center\" style=\"font-size: 4em;\">You must enter a valid user name and password to access the Diagnostic page on BrightSign unit {0}</h1><br/><p align=\"center\" style=\"font-size: 3em;\">You can touch the Refresh button to try again.</p></body>", Constants.ActiveDevice.Name);

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

		private bool _diagnosticPageAuthorized;
		public bool diagnosticPageAuthorized
		{
			get { return _diagnosticPageAuthorized; }
			set
			{
				_diagnosticPageAuthorized = value;
				RaisePropertyChanged(() => diagnosticPageAuthorized);
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

		private string _LoadFailureURL;
		public string LoadFailureURL
		{
			get { return _LoadFailureURL; }
			set
			{
				_LoadFailureURL = value;
				RaisePropertyChanged(() => LoadFailureURL);
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

		public DiagnosticsViewModel(IMvxMessenger messenger) : base(messenger)
		{
			ShowAuthenticationPopup = false;
			dialogService = Mvx.Resolve<IDialogService>();
			IsDeviceOnline = true;
			ViewTitle = TitleType.Diagnostics.ToString();
			SetupWebView();
			//if (AciveDeviceToken == null)
			//{
			//    AciveDeviceToken = Messenger.Subscribe<ActiveDeviceMessage>(OnDeviceStatusResponse);
			//}
			LoginPopupTitle = string.Format("{0}_Diagnostics", Constants.ActiveDevice.Name);
		}

		private void OnDeviceStatusResponse(ActiveDeviceMessage obj)
		{
			if (IsDeviceOnline != (obj.deviceStatus == DeviceStatus.Connected))
			{
				IsDeviceOnline = obj.deviceStatus == DeviceStatus.Connected;
				if (IsDeviceOnline)
				{
					LoadFailureURL = String.Format("< body >< br />< br />< h1 align =\"center\" style=\"font-size: 4em;\">Could not access web server for BrightSign unit {0}</h1><br/><p align=\"center\" style=\"font-size: 3em;\">Please touch Settings button at upper right to check selected unit</p></body>", Constants.ActiveDevice.Name);
					LoginPopupTitle = string.Format("{0}_Diagnostics", Constants.ActiveDevice.Name);
				}
				SetupWebView();
			}
		}

		private string _DiagnosticsURL;
		public string DiagnosticsURL
		{
			get { return _DiagnosticsURL; }
			set
			{
				_DiagnosticsURL = value;
				RaisePropertyChanged(() => DiagnosticsURL);
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

				Debug.WriteLine("Logging in for Diagnostics of the application");
				var status = await HttpBase.Instance.Login(LoginUser, LoginPwd);

				if (status == 1)
				{
					

                    if (Plugin.DeviceInfo.CrossDeviceInfo.Current.Platform == Plugin.DeviceInfo.Abstractions.Platform.Android)
                    {
						DiagnosticsURL = String.Format("http://{0}", Constants.ActiveDevice?.IpAddress);
					}
                    else
                    {

						//DiagnosticsURL = string.Format("http://{0}:{1}@{2}/index.html", LoginUser, LoginPwd, Constants.ActiveDevice?.IpAddress);
						DiagnosticsURL = string.Format("http://{0}/index.html", Constants.ActiveDevice?.IpAddress);
						// Lets try and store in NSURLCredentials
					
					}
					diagnosticPageAuthorized = true;
					ShowAuthenticationPopup = false;
				}
				else
				{
					IsBusy = false;
					ShowAuthenticationPopup = true;
				}

				//DiagnosticsURL = String.Format("http://{0}", Constants.ActiveDevice?.IpAddress);

			}
		}

		public string VariableOfflineURL =
			 "<body><br/><br/><h1 align=\"center\" style=\"font-size: 4em;\">A BrightSign unit could not be found on the local network</h1><br/><p align=\"center\" style=\"font-size: 3em;\">You can touch the Refresh button to try again.</p></body>";

		public async void SetupWebView()
		{
			ViewTitle = "Diagnostics";
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
					DiagnosticsURL = VariableOfflineURL;
				}
				else
				{
					//Task.Run(async () =>
					//{
					if (!diagnosticPageAuthorized)
					{
						diagnosticPageAuthorized = await HttpBase.Instance.IsAuthorized(Constants.ActiveDevice.IpAddress);
						if (diagnosticPageAuthorized)
						{
							DiagnosticsURL = String.Format("http://{0}", Constants.ActiveDevice?.IpAddress);
						}
						else
						{
							ShowAuthenticationPopup = true;
						}
					}
					else
					{
						//if (!string.IsNullOrEmpty(LoginPwd))
						//{
						//	DiagnosticsURL = string.Format("http://{0}:{1}@{2}/index.html", LoginUser, LoginPwd, Constants.ActiveDevice?.IpAddress); String.Format("http://{0}", Constants.ActiveDevice?.IpAddress);
						//}
						//else
						//{
						//	DiagnosticsURL = String.Format("http://{0}", Constants.ActiveDevice?.IpAddress);
						//}
						DiagnosticsURL = String.Format("http://{0}", Constants.ActiveDevice?.IpAddress);
					}

					// });

				}

				Debug.WriteLine("DiagnosticsURL " + DiagnosticsURL);

				
			});
		}

		public IMvxCommand DownloadLogCommand
		{
			get
			{
				return new MvxCommand<string>(async (obj) =>
				{
					await DownloadLogandEmailClick(obj);
				});
			}
		}

		private async Task DownloadLogandEmailClick(string url)
		{
			IsBusy = true;
			var response = await HttpBase.Instance.DownloadFile(url, LoginPwd);
			IsBusy = false;
			if (response.Item1)
			{
				var fileStream = response.Item2;

				EmailAttachment emailAttachment = new EmailAttachment();
				emailAttachment.FileName = String.Format("BrightSign_{0}.{1}", Constants.ActiveDevice.SerialNumber, url.Contains("_log") ? "log" : "dump");
				emailAttachment.ContentType = "text/plain";
				emailAttachment.Content = fileStream;

				var attachments = new List<EmailAttachment>();
				attachments.Add(emailAttachment);
				if (Mvx.Resolve<IMvxComposeEmailTaskEx>().CanSendEmail)
				{
					//if (Plugin.DeviceInfo.CrossDeviceInfo.Current.Platform == Plugin.DeviceInfo.Abstractions.Platform.Android)
					//{
					//	showEmailComposer = true;
					//}
					//else
					//{
					//	Mvx.Resolve<IMvxComposeEmailTaskEx>().ComposeEmail(new List<string> { }, subject: String.Format("BrightSign_{0}_{1}", Constants.ActiveDevice.SerialNumber, url.Contains("_log") ? "log" : "dump"), attachments: attachments);
					//}

					Mvx.Resolve<IMvxComposeEmailTaskEx>().ComposeEmail(new List<string> { }, subject: String.Format("BrightSign_{0}_{1}", Constants.ActiveDevice.SerialNumber, url.Contains("_log") ? "log" : "dump"), attachments: attachments);
					//Mvx.Resolve<IMvxComposeEmailTask>().ComposeEmail("me@slodge.com", string.Empty, "MvvmCross Email", "Mvvm", false);
				}
				else
				{
					await dialogService.ShowAlertAsync("This device cannot send mail.", Strings.error, Strings.ok);
				}


			}
			else
			{
				await dialogService.ShowAlertAsync("Unable to download log, Please try again", Strings.error, Strings.ok);
			}

		}
	}
}
