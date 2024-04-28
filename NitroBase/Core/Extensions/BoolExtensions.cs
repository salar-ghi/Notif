using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Core.Helpers;

namespace Core.Extensions
{
    public static class BoolExtensions
    {
        public static string ToYesNo(this bool value) => value ? "بله" : "خیر";

    }
}
