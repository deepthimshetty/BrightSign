using System;
using UIKit;

namespace BrightSign.iOS.Utility
{
    public class UIColorUtility
    {
        /// <summary>
        /// Froms the hex.
        /// </summary>
        /// <returns>The hex.</returns>
        /// <param name="hexValueString">Hex value string.</param>
        public static UIColor FromHex(string hexValueString)
        {
            string stringValue = hexValueString;
            stringValue = stringValue.Replace("#", "");
            int hexValue = Convert.ToInt32(stringValue, 16);
            /// <summary>
            /// UIC olor utility.
            /// </summary>
            return UIColor.FromRGB(
                (((float)((hexValue & 0xFF0000) >> 16)) / 255.0f),
                (((float)((hexValue & 0xFF00) >> 8)) / 255.0f),
                (((float)(hexValue & 0xFF)) / 255.0f)
            );
        }
    }
}
