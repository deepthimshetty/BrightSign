using System;
using CoreGraphics;
using MvvmCross.Platform.Converters;

namespace BrightSign.iOS.Utility
{
    public class ImageTransformConverter : MvxValueConverter<float, CGAffineTransform>
    {
        //public ImageTransformConverter()
        //{
        //}
        /// <summary>
        /// Convert the specified value, targetType, parameter and culture.
        /// </summary>
        /// <param name="value">If set to <c>true</c> value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="culture">Culture.</param>
        protected override CGAffineTransform Convert(float value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CGAffineTransform transform = CGAffineTransform.MakeRotation(value);//CGAffineTransform.MakeRotation((float)(Math.PI / 2));
            return transform;
        }
    }
}
