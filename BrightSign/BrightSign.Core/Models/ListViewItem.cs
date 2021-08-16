using System;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.ViewModels;
using static BrightSign.Core.Utility.BSUtility;

namespace BrightSign.Core.Models
{
    public class ListViewItem : MvxViewModel
    {
        public int ID { get; set; }
        private string _title;
        public string Title
        {
            get
            { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private string _subTitle;
        public string SubTitle
        {
            get
            { return _subTitle; }
            set
            {
                _subTitle = value;
                RaisePropertyChanged(() => SubTitle);
            }
        }
        public bool Enable { get; set; }


        CellTypes _cellType;
        public CellTypes CellType
        {
            get { return _cellType; }
            set
            {
                _cellType = value;
                RaisePropertyChanged(() => CellType);
            }
        }


        int _topMargin;
        public int TopMargin
        {
            get
            {

                if (_topMargin == 0)
                    _topMargin = 10;
                return _topMargin;
            }
            set
            {
                _topMargin = value;
                RaisePropertyChanged(() => TopMargin);
            }
        }


        bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                RaisePropertyChanged(() => IsVisible);
            }
        }

        public ICommand SwitchValueChanged { get; set; }

        //Switch
        private bool _isSwitchEnabled;
        public bool IsSwitchEnabled
        {
            get { return _isSwitchEnabled; }
            set
            {
                _isSwitchEnabled = value;
                if (SwitchValueChanged != null)
                {
                    SwitchValueChanged.Execute(_isSwitchEnabled);
                }
                RaisePropertyChanged(() => IsSwitchEnabled);
            }
        }

        private ActionButtonType _selectedItem;
        public ActionButtonType SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (RadioValueChanged != null)
                {
                    RadioValueChanged.Execute(_selectedItem);
                }
                RaisePropertyChanged(() => SelectedItem);
            }
        }

        private List<ActionButtonType> _items;
        public List<ActionButtonType> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(() => Items); }
        }

        public ICommand ListSelectionCommand { get; set; }
        public ICommand RemoveItemCommand { get; set; }
        public ICommand RadioValueChanged { get; set; }
        public string PresentationLabel { get; set; }
        public bool IsUserDefined { get; set; }
        public bool UpdateCheckDone { get; set; }


    }

    public class ActionButtonType : MvxViewModel
    {
        public int ID { get; set; }
        private string _title;
        public string Title
        {
            get
            { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }
    }
}