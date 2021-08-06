using System;
using MvvmCross.Core.ViewModels;

namespace BrightSign.Core.Models
{
    public class TabItem : MvxViewModel
    {
        string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                RaisePropertyChanged(() => Name);
            }
        }


        bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }


        int _ID;
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
                RaisePropertyChanged(() => ID);
            }
        }
    }
}
