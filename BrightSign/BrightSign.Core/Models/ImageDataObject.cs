using System;
using MvvmCross.ViewModels;

namespace BrightSign.Core.Models
{
    public class ImageDataObject : MvxViewModel
    {
        byte[] _ImageData;
        public byte[] ImageData
        {
            get
            {
                return _ImageData;
            }
            set
            {
                _ImageData = value;
                RaisePropertyChanged("ImageData");
            }
        }


        string _ImageUrl;
        public string ImageUrl
        {
            get
            {
                return _ImageUrl;
            }
            set
            {
                _ImageUrl = value;
                RaisePropertyChanged("ImageUrl");
            }
        }

        string _ImagePath;
        public string ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                _ImagePath = value;
                RaisePropertyChanged("ImagePath");
            }
        }
    }
}
