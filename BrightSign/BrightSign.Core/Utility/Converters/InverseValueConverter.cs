using System;
using MvvmCross.Converters;

namespace BrightSign.Core.Utility.Converters
{
    public class InverseValueConverter : MvxValueConverter<bool, bool>
    {
        /// <summary>
        /// Convert the specified value, targetType, parameter and culture.
        /// </summary>
        /// <param name="value">If set to <c>true</c> value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="culture">Culture.</param>
        protected override bool Convert(bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !value;
        }
    }
}
