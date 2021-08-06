using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Database;
using BrightSign.Core.Utility.Interface;
using BrightSign.Core.Utility.Messages;
using BrightSign.Localization;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;


namespace BrightSign.Core.ViewModels
{
    public class ManageActionsViewModel : BaseViewModel, IModify
    {
        MvxSubscriptionToken ButtonsToken;
        IMvxMessenger messanger;
        bool IsUserDefinedActionAdded = false;
        IDialogService dialogservice;

        public ManageActionsViewModel(IMvxMessenger messenger, IDialogService _dialogservice) : base(messenger)
        {
            DefaultActionsList = new ObservableCollection<BSUdpAction>();
            UserDefinedActionsList = new ObservableCollection<BSUdpAction>();
            ViewTitle = "Manage Actions";
            dialogservice = _dialogservice;

            //DefaultActionsList = new ObservableCollection<BSUdpAction>(Constants.BSActionList);
            //UserDefinedActionsList = new ObservableCollection<BSUdpAction>(DBHandler.Instance.GetActionsfromDB());

            //DefaultActionsList = new ObservableCollection<BSUdpAction>(Constants.BSActionList);
            //UserDefinedActionsList = new ObservableCollection<BSUdpAction>(Constants.UserDefinedActionsList);

            DefaultActionsList = Constants.BSActionList;
            UserDefinedActionsList = Constants.UserDefinedActionsList;


            //SelectedTabIndex = 0;

            IsEditViewVisible = false;
            IsUserDefinedActionAdded = false;

        }

        public void Init(int TabIndex)
        {
            SelectedTabIndex = TabIndex;
        }


        int editButtonIndex = -1;

        private bool _IsEditViewVisible;
        public bool IsEditViewVisible
        {
            get { return _IsEditViewVisible; }
            set
            {
                _IsEditViewVisible = value;
                RaisePropertyChanged(() => IsEditViewVisible);
            }
        }

        private bool _IsRefresh;
        public bool IsRefresh
        {
            get { return _IsRefresh; }
            set
            {
                _IsRefresh = value;
                RaisePropertyChanged(() => IsRefresh);
            }
        }

        private bool _IsAddButtonVisible;
        public bool IsAddButtonVisible
        {
            get { return _IsAddButtonVisible; }
            set
            {
                _IsAddButtonVisible = value;
                RaisePropertyChanged(() => IsAddButtonVisible);
            }
        }

        private bool _IsUDPTextEditable;
        public bool IsUDPTextEditable
        {
            get { return _IsUDPTextEditable; }
            set
            {
                _IsUDPTextEditable = value;
                RaisePropertyChanged(() => IsUDPTextEditable);
            }
        }

        private string _EditButtonLabel;
        public string EditButtonLabel
        {
            get { return _EditButtonLabel; }
            set
            {
                _EditButtonLabel = value;
                RaisePropertyChanged(() => EditButtonLabel);
            }
        }

        private string _EditButtonUDPText;
        public string EditButtonUDPText
        {
            get { return _EditButtonUDPText; }
            set
            {
                _EditButtonUDPText = value;
                RaisePropertyChanged(() => EditButtonUDPText);
            }
        }

        private string _EditMessage;
        public string EditMessage
        {
            get { return _EditMessage; }
            set
            {
                _EditMessage = value;
                RaisePropertyChanged(() => EditMessage);
            }
        }

        private string _EditTitle;
        public string EditTitle
        {
            get { return _EditTitle; }
            set
            {
                _EditTitle = value;
                RaisePropertyChanged(() => EditTitle);
            }
        }

        private string _UpdateButtonName;
        public string UpdateButtonName
        {
            get { return _UpdateButtonName; }
            set
            {
                _UpdateButtonName = value;
                RaisePropertyChanged(() => UpdateButtonName);
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
                    IsAddButtonVisible = false;
                    IsUDPTextEditable = false;
                }
                else
                {
                    ActionsItemSource = UserDefinedActionsList;
                    IsAddButtonVisible = true;
                    IsUDPTextEditable = true;
                }
                RaisePropertyChanged(() => SelectedTabIndex);
            }
        }

        public MvxCommand CancelCommand
        {
            get
            {
                return new MvxCommand(() => ExecuteCancelCommand());
            }
        }


        public ICommand RemoveCommand
        {
            get
            {
                return new MvxCommand<int>(RemoveClick);
            }
        }

        private void RemoveClick(int removeIndex)
        {
            var item = UserDefinedActionsList[removeIndex];
            DBHandler.Instance.RemoveAction(item);
            UserDefinedActionsList.Remove(item);
            Constants.UserDefinedActionsList.Remove(item);
            IsUserDefinedActionAdded = true;
            IsRefresh = true;
        }

        public ICommand EditCommand
        {
            get
            {
                return new MvxCommand<int>(EditClick);
            }
        }

        private void ExecuteCancelCommand()
        {
            Close(this);
            //if (IsUserDefinedActionAdded)
            //{
            Messenger.Publish(new LoadButtonsMessage(this, DeviceStatus.Connected, true));
            //}
        }

        private void EditClick(int index)
        {
            if (SelectedTabIndex == 0)
            {
                IsUDPTextEditable = false;
                EditMessage = "Defined by Presentation";
                EditButtonLabel = DefaultActionsList[index].Label;
                EditButtonUDPText = DefaultActionsList[index].DataUDP;

            }
            else
            {
                IsUDPTextEditable = true;
                EditMessage = "User Defined";
                EditButtonLabel = UserDefinedActionsList[index].Label;
                EditButtonUDPText = UserDefinedActionsList[index].DataUDP;

            }
            EditTitle = "Edit Action";
            editButtonIndex = index;
            UpdateButtonName = "Update";
            IsEditViewVisible = true;
            RaisePropertyChanged(() => IsEditViewVisible);

        }

        public int Selectedtab
        {
            get
            {
                return SelectedTabIndex;
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                return new MvxCommand(UpdateClick);
            }
        }

        private void UpdateClick()
        {
            //Check validations
            if (string.IsNullOrWhiteSpace(EditButtonLabel))
            {
                dialogservice.ShowAlertAsync(Strings.labelerror, Strings.error, Strings.ok);
                return;
            }
            else if (string.IsNullOrWhiteSpace(EditButtonUDPText))
            {
                dialogservice.ShowAlertAsync(Strings.dataerror, Strings.error, Strings.ok);
                return;
            }

            if (UpdateButtonName == "Save")
            {
                //Add the user defined button
                BSUdpAction addaction = new BSUdpAction();
                addaction.Label = EditButtonLabel;
                addaction.DataUDP = EditButtonUDPText;
                addaction.IsUserDefined = true;
                addaction.Sno = Constants.UserDefinedActionsList.Count + 1;

                BSUdpAction action = DBHandler.Instance.GetActionInstanceByNameLabel(EditButtonUDPText);
                if (action != null)
                {
                    if (action.IsUserDefined)
                    {
                        // Show error toast
                        dialogservice.ShowAlertAsync("Command already exist", Strings.error, Strings.ok);

                        IsRefresh = true;
                        return;
                    }
                    else
                    {
                        DefaultActionsList.Remove(DefaultActionsList.Where(i => i.DataUDP == action.DataUDP).Single());
                        DBHandler.Instance.RemoveAction(action);
                        IsRefresh = true;
                    }
                }


                if (!string.IsNullOrEmpty(Constants.ActivePresentation))
                {
                    addaction.PresentationLabel = Constants.ActivePresentation;

                    DBHandler.Instance.InsertAction(addaction);

                    //Retrieve the Added Action from DB and add to Default List
                    var acion = DBHandler.Instance.GetActionWithName(EditButtonLabel);

                    // Add the Action to User Defined action list
                    UserDefinedActionsList.Add(acion);
                    //Constants.UserDefinedActionsList.Add(acion);
                }
                else
                {
                    //Constants.UserDefinedActionsList.Add(addaction);
                    UserDefinedActionsList.Add(addaction);
                }

                IsUserDefinedActionAdded = true;

            }
            else
            {
                //Update the user defined button
                BSUdpAction updatedAction;
                if (SelectedTabIndex == 0)
                {
                    updatedAction = DefaultActionsList[editButtonIndex];
                    updatedAction.Label = EditButtonLabel;
                    DefaultActionsList[editButtonIndex] = updatedAction;
                    //Constants.BSActionList[editButtonIndex] = updatedAction;
                    RaisePropertyChanged(() => ActionsItemSource);
                    IsUserDefinedActionAdded = true;
                }
                else
                {
                    updatedAction = UserDefinedActionsList[editButtonIndex];
                    updatedAction.Label = EditButtonLabel;
                    updatedAction.DataUDP = EditButtonUDPText;
                    UserDefinedActionsList[editButtonIndex] = updatedAction;
                    //Constants.UserDefinedActionsList[editButtonIndex] = updatedAction;
                    RaisePropertyChanged(() => ActionsItemSource);
                    IsUserDefinedActionAdded = true;
                }
                DBHandler.Instance.InsertorReplaceAction(updatedAction);
            }
            IsEditViewVisible = false;
            IsRefresh = true;

        }


        public ICommand CancelEditCommand
        {
            get
            {
                return new MvxCommand(CancelEditClick);
            }
        }

        private void CancelEditClick()
        {
            IsEditViewVisible = false;
        }

        public ICommand AddActionCommand
        {
            get
            {
                return new MvxCommand(AddActionClick);
            }
        }

        private void AddActionClick()
        {
            EditTitle = "Add New Action";
            EditMessage = "User Defined";
            UpdateButtonName = "Save";
            IsEditViewVisible = true;
            EditButtonLabel = string.Empty;
            EditButtonUDPText = string.Empty;
        }

        public async void swapItem(int from, int to)
        {

            BSUdpAction temp1 = ActionsItemSource[from];
            BSUdpAction temp2 = ActionsItemSource[to];

            ActionsItemSource.Move(from, to);
            ActionsItemSource.RemoveAt(from);
            ActionsItemSource.Insert(from, temp2);

            //ActionsItemSource.RemoveAt(from);
            //ActionsItemSource.RemoveAt(to);

            //ActionsItemSource.Insert(from, temp2);
            //ActionsItemSource.Insert(to, temp1);


        }



    }
}
