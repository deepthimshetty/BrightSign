using System;
using MvvmCross.ViewModels;

namespace BrightSign.Core.Models
{
    public class SnapshotConfigModel : MvxViewModel
    {
        int _Count;
        public int Count
        {
            get
            {
                return _Count;
            }
            set
            {
                _Count = value;
                RaisePropertyChanged(() => Count);
            }
        }


        bool _CanConfigure;
        public bool CanConfigure
        {
            get
            {
                return _CanConfigure;
            }
            set
            {
                _CanConfigure = value;
                RaisePropertyChanged(() => CanConfigure);
            }
        }


        int _Interval;
        public int Interval
        {
            get
            {
                return _Interval;
            }
            set
            {
                _Interval = value;
                RaisePropertyChanged(() => Interval);
            }
        }


        bool _DisplayPortraitMode;
        public bool DisplayPortraitMode
        {
            get
            {
                return _DisplayPortraitMode;
            }
            set
            {
                _DisplayPortraitMode = value;
                RaisePropertyChanged(() => DisplayPortraitMode);
            }
        }


        int _MaxImages;
        public int MaxImages
        {
            get
            {
                return _MaxImages;
            }
            set
            {
                _MaxImages = value;
                RaisePropertyChanged(() => MaxImages);
            }
        }

        int _Quality;
        public int Quality
        {
            get
            {
                return _Quality;
            }
            set
            {
                _Quality = value;
                RaisePropertyChanged(() => Quality);
            }
        }


        double _ResY;
        public double ResY
        {
            get
            {
                return _ResY;
            }
            set
            {
                _ResY = value;
                RaisePropertyChanged(() => ResY);
            }
        }


        double _ResX;
        public double ResX
        {
            get
            {
                return _ResX;
            }
            set
            {
                _ResX = value;
                RaisePropertyChanged(() => ResX);
            }
        }

        bool _Enabled;
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                _Enabled = value;
                RaisePropertyChanged(() => Enabled);
            }
        }



    }
}
