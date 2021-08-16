using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Database;
using BrightSign.Core.Utility.Interface;
using BrightSign.Core.Utility.Messages;
using BrightSign.Core.ViewModels.Settings;
using MvvmCross.ViewModels;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace BrightSign.Core.ViewModels
{
    public class ActionsViewModel : BaseViewModel
    {
        MvxSubscriptionToken ButtonsToken;
        IUserPreferences UserPreferences;

        //private readonly IMvxNavigationService _navigationServic;

        
        public int SelectedButtonType
        {
            get
            {
                return UserPreferences.GetInt(Constants.USER_PREFS_BUTTON_TYPE);
            }
        }

        public IMvxCommand SettingsCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _ = SettingsClickAsync();
                });
            }
        }


        private bool _ReloadActionSection;
        public bool ReloadActionSection
        {
            get { return _ReloadActionSection; }
            set
            {
                _ReloadActionSection = value;
                RaisePropertyChanged(() => ReloadActionSection);
            }
        }

        private async Task SettingsClickAsync()
        {
            //ShowViewModel<SettingsViewModel>();
            await _navigationService.Navigate<SnapshotsViewModel>();
        }

        public ActionsViewModel(IMvxMessenger messenger, IUserPreferences userPreferences, IMvxNavigationService navigationService) : base(messenger)
        {
            UserPreferences = userPreferences;

            //DefaultActionsList = new ObservableCollection<BSUdpAction>(Constants.BSActionList);
            //UserDefinedActionsList = new ObservableCollection<BSUdpAction>(Constants.UserDefinedActionsList);


            DefaultActionsList = Constants.BSActionList;
            UserDefinedActionsList = Constants.UserDefinedActionsList;
            _navigationService = navigationService;

            ViewTitle = "Actions";

            ReloadActionSection = false;
            //DefaultActionsList = new ObservableCollection<BSUdpAction>(Constants.BSActionList);
            //UserDefinedActionsList = new ObservableCollection<BSUdpAction>(DBHandler.Instance.GetActionsfromDB());
            SelectedTabIndex = 0;
            //GetData();
            if (ButtonsToken == null)
            {
                ButtonsToken = Messenger.Subscribe<LoadButtonsMessage>(OnLoadButtonsResponse);
            }

        }

        private void GetData()

        {
            ActionsItemSource.Add(new BSUdpAction
            {
                Label = "label1"
            });
            ActionsItemSource.Add(new BSUdpAction
            {
                Label = "label2"
            });
            ActionsItemSource.Add(new BSUdpAction
            {
                Label = "label1"
            });
            ActionsItemSource.Add(new BSUdpAction
            {
                Label = "label2"
            });

        }

        private void OnLoadButtonsResponse(LoadButtonsMessage obj)
        {
            if (obj != null)
            {
                if (obj.deviceStatus == DeviceStatus.Connected || obj.RefreshData)
                {
                    DefaultActionsList = Constants.BSActionList;
                    UserDefinedActionsList = Constants.UserDefinedActionsList;
                    ReloadActionSection = true;
                }
                else
                {
                    ActionsItemSource = new ObservableCollection<BSUdpAction>();
                }
            }

            if (SelectedTabIndex == 0)
            {
                ActionsItemSource = DefaultActionsList;
            }
            else
            {
                ActionsItemSource = UserDefinedActionsList;
            }

            for (int i = 0; i < DefaultActionsList.Count; i++)
            {
                DefaultActionsList[i].Sno = i + 1;
            }

            DBHandler.Instance.InsertorReplaceAllActions(DefaultActionsList.ToList());

            for (int i = 0; i < UserDefinedActionsList.Count; i++)
            {
                UserDefinedActionsList[i].Sno = i + 1;
            }
            DBHandler.Instance.InsertorReplaceAllActions(UserDefinedActionsList.ToList());


        }


        private int _SelectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _SelectedTabIndex; }
            set
            {
                _SelectedTabIndex = value;
                if (value == 0)
                {
                    ActionsItemSource = DefaultActionsList;
                }
                else
                {
                    ActionsItemSource = UserDefinedActionsList;
                }
                RaisePropertyChanged(() => SelectedTabIndex);
                RaisePropertyChanged(() => ActionsItemSource);

            }
        }

        //public IMvxCommand UDPCommand
        //{
        //    get
        //    {
        //        return new MvxCommand<BSUdpAction>(ExecuteUDPCommand);
        //    }
        //}


        private MvxCommand<BSUdpAction> m_SelectGridItemCommand;

        public ICommand UDPCommand
        {
            get
            {
                return this.m_SelectGridItemCommand ?? (this.m_SelectGridItemCommand = new MvxCommand<BSUdpAction>(this.ExecuteUDPCommand));
            }
        }

        private ObservableCollection<BSUdpAction> _actionsItemSource;
        public ObservableCollection<BSUdpAction> ActionsItemSource
        {
            get { return _actionsItemSource; }
            set
            {
                _actionsItemSource = value;
                RaisePropertyChanged(() => ActionsItemSource);
            }
        }

        private ObservableCollection<BSUdpAction> _DefaultActionsList;
        public ObservableCollection<BSUdpAction> DefaultActionsList
        {
            get { return _DefaultActionsList; }
            set
            {
                _DefaultActionsList = value;
                RaisePropertyChanged(() => DefaultActionsList);
            }
        }

        private ObservableCollection<BSUdpAction> _UserDefinedActionsList;
        public ObservableCollection<BSUdpAction> UserDefinedActionsList
        {
            get { return _UserDefinedActionsList; }
            set
            {
                _UserDefinedActionsList = value;
                RaisePropertyChanged(() => UserDefinedActionsList);
            }
        }

        private void ExecuteUDPCommand(BSUdpAction bsUDPAction)
        {
            if (Constants.ActiveDevice != null && Constants.ActiveDevice.IsOnline)
            {
                using (UdpClient client = new UdpClient())
                {

                    //IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.2.126"), 41234);
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(Constants.ActiveDevice?.IpAddress), Constants.actionPort);
                    client.Client.Connect(endPoint);
                    Byte[] senddata = Encoding.ASCII.GetBytes(bsUDPAction.DataUDP);//("buttonpress!31!1");
                                                                                   //client.SendAsync(senddata,senddata.Length,endPoint);

                    try
                    {
                        var datasent = client.Client.Send(senddata);
                    }
                    catch (SocketException socketException)
                    {
                        Debug.WriteLine("Something went wrong with UDP Command");
                        Constants.ActiveDevice.IsOnline = false; // Also mapping that the device is offline
                    }
                    catch (ObjectDisposedException disposedException)
                    {
                        Debug.WriteLine("Object is disposed");
                    }
                    
                  
                }
            }
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

        private async void SnapshotClick()
        {
            //ShowViewModel<SnapshotsViewModel>();
            await _navigationService.Navigate<SnapshotsViewModel>();
        }



        public IMvxCommand RefreshCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await RefreshClick();
                });
            }
        }

        private async Task RefreshClick()
        {
            try
            {
                var userDefinedButtons = Constants.BSActionList.Where(o => o.IsUserDefined);
                IsBusy = true;
                bool IsSuccess = await BSUtility.Instance.GetDeviceRemoteData(false);
                if (IsSuccess)
                {
                    if (userDefinedButtons != null)
                    {
                        foreach (var item in userDefinedButtons)
                        {
                            Constants.BSActionList.Add(item);
                        }
                    }
                    ActionsItemSource = new ObservableCollection<BSUdpAction>(Constants.BSActionList);
                    ReloadActionSection = true;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }


        }

        public IMvxCommand EditActionsCommand
        {
            get
            {
                return new MvxCommand(async () =>
               {
                   //TODO: Solve the navigation with parameters. Add the navigation here
                   //ShowViewModel<ManageActionsViewModel>(new { TabIndex = SelectedTabIndex });
                   //object p = await _navigationService.Navigate<ManageActionsViewModel>( new { TabIndex = SelectedTabIndex});
                   if (ButtonsToken == null)
                   {
                       ButtonsToken = Messenger.Subscribe<LoadButtonsMessage>(OnLoadButtonsResponse);
                   }
               });
            }
        }
    }
}
