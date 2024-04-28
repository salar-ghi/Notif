using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Core.Helpers;

namespace Core.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum value) =>
            value
                .GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description ?? Enum.GetName(value.GetType(), value);
        
    }
}
