using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Core.Helpers;

namespace Core.Extensions
{
    public static class LongExtensions
    {
        public static string ToCommaDelimited(this long number, string separator = ",", int separatedSize = 3) => ((decimal)number).ToCommaDelimited(separator, separatedSize);


    }
}
