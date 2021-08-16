using System;
using System.Globalization;
using BrightSign.Core.Utility;
using MvvmCross.ViewModels;

namespace BrightSign.Core.Models
{
    public class BSSnapshot : MvxViewModel
    {
        string _ID;
        public string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;

                ImageDataObj.ImageUrl = string.Format("http://{0}:{1}/GetSnapshot?ID={2}", Constants.ActiveDevice.IpAddress, Constants.httpPort, value);
            }
        }

        ImageDataObject _ImageDataObj;
        public ImageDataObject ImageDataObj
        {
            get
            {
                return _ImageDataObj;
            }
            set
            {
                _ImageDataObj = value;
                RaisePropertyChanged("ImageDataObj");

            }
        }

        public bool IsCredentialsRequired
        {
            get
            {
                return Constants.IsCredentialsRequiredforSnapshots;
            }
        }


        float _ImageTransform;
        public float ImageTransform
        {
            get
            {
                return _ImageTransform;
            }
            set
            {
                _ImageTransform = value;
                RaisePropertyChanged("ImageTransform");
            }
        }
        string _TimeStamp;
        public string TimeStamp
        {
            get
            {
                return _TimeStamp;
            }
            set
            {
                _TimeStamp = value;
                SnapshotDate = DateTime.ParseExact(value, Constants.DateFormatStr, CultureInfo.InvariantCulture);
            }
        }

        DateTime _SnapshotDate;
        public DateTime SnapshotDate
        {
            get
            {
                return _SnapshotDate;
            }
            set
            {
                _SnapshotDate = value;
            }
        }

        public BSSnapshot()
        {
            ImageDataObj = new ImageDataObject();
        }
    }
}
