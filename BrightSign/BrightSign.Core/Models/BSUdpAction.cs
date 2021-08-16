using System;
using System.ComponentModel;
using System.Windows.Input;
using MvvmCross.ViewModels;
using SQLite;

namespace BrightSign.Core.Models
{
    public class BSUdpAction : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }

        public int Sno { get; set; }

        string _Label;
        public string Label
        {
            get
            {
                return _Label;
            }
            set
            {
                _Label = value;
                NotifyPropertyChanged("Label");
            }
        }

        public string PresentationLabel { get; set; }

        string _DataUDP;
        public string DataUDP
        {
            get
            {
                return _DataUDP;
            }
            set
            {
                _DataUDP = value;
                NotifyPropertyChanged("DataUDP");
            }
        }

        public bool IsUserDefined { get; set; }
        public bool UpdateCheckDone { get; set; }

        /// <summary>
        /// An event to detect the change in the value of a property.
        /// </summary>

        public event PropertyChangedEventHandler PropertyChanged;


        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
