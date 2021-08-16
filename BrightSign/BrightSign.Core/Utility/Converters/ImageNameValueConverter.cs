using System;
using System.Globalization;
using MvvmCross;
//using MvvmCross.Platform;
using MvvmCross.Converters;
using MvvmCross.Plugin.File;

namespace BrightSign.Core.Utility.Converters
{
    public class ImageNameValueConverter : MvxValueConverter<string, string>
    {
        /// <summary>
        /// Convert the specified value, targetType, parameter and culture.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="culture">Culture.</param>
        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            /// <summary>
            /// Image name value converter.
            /// </summary>
            switch (parameter.ToString())
            {
                case "0"://Droid
                         //return "res:" + value.ToLower();
                    return value.ToLower();


                case "1"://iOS
                    return "res:" + value + ".png";

                case "2":
                    if (!string.IsNullOrEmpty(value))
                    {
                        return Mvx.Resolve<IMvxFileStore>().NativePath(value);
                    }
                    return value;

                default:
                    return "";
            }
        }
    }
}
