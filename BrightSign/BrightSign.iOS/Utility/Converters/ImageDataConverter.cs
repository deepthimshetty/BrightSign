using System;
using BrightSign.Core.Models;
using CoreGraphics;
using Foundation;
using MvvmCross.Platform.Converters;
using UIKit;

namespace BrightSign.iOS.Utility.Converters
{
    public class ImageDataConverter : MvxValueConverter<ImageDataObject, UIImage>
    {
        protected override UIImage Convert(ImageDataObject value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            UIImage uiimage = null;

            if (value.ImageData != null)
            {
                byte[] imageByteArray = value.ImageData;

                if (value == null)
                    return null;

                var data = NSData.FromArray(imageByteArray);
                uiimage = UIImage.LoadFromData(data);
            }
            else
            {
                using (var url = new NSUrl(value.ImageUrl))
                using (var data = NSData.FromUrl(url))
                    return UIImage.LoadFromData(data);
            }

            return uiimage;
        }

    }

}
