using System;
using MvvmCross.ViewModels;

namespace BrightSign.Core.Models
{
    public class ButtonSizeItem : MvxViewModel
    {
        private string _ButtonText;
        public string ButtonText
        {
            get
            {
                return _ButtonText;
            }
            set
            {
                _ButtonText = value;
                RaisePropertyChanged(() => ButtonText);
            }
        }

        private string _Image;
        public string Image
        {
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;

                RaisePropertyChanged(() => Image);
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                if (IsSelected)
                {
                    Image = "button_selected.png";
                }
                else
                {
                    Image = "button_unselected.png";
                }
                RaisePropertyChanged(() => IsSelected);
            }
        }

        public int ID { get; set; }
    }
}
