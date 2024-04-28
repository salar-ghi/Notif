using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Core.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDateTimeString(this DateTime dateTime, string format = "yyyy-MM-dd", bool convertToShamsi = false) => 
            dateTime.ToString(format, convertToShamsi ? new CultureInfo("fa-IR") : null);

    }
}
