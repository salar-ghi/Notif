using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Core.Helpers;

namespace Core.Extensions
{
    public static class DecimalExtensions
    {

        public static string ToCommaDelimited(this decimal number, string separator = ",", int separatedSize = 3)
        {
            if (number is default(decimal)) return default(decimal).ToString(CultureInfo.CurrentCulture);
            var isInteger = number % 1 == 0; // So number doesn't have decimal part
            return isInteger ? number.ToString("N0", new NumberFormatInfo()
            {
                NumberGroupSizes = new[] { separatedSize },
                NumberGroupSeparator = separator
            }).Split(".")[0] :
                number.ToString("N0", new NumberFormatInfo()
                {
                    NumberGroupSizes = new[] { separatedSize },
                    NumberGroupSeparator = separator
                });
        }


    }
}
