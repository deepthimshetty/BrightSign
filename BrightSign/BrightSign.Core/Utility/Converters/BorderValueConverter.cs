using System;
using System.Globalization;
using MvvmCross.Converters;

namespace BrightSign.Core.Utility.Converters
{

    public class BorderValueConverter : MvxValueConverter<bool, double>
    {
        /// <summary>
        /// Convert the specified value, targetType, parameter and culture.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="culture">Culture.</param>
        protected override double Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            /// <summary>
            /// Image name value converter.
            /// </summary>
            if (value)
            {
                return 3.0;
            }
            else
            {
                return 0.0;
            }
        }
    }
}
