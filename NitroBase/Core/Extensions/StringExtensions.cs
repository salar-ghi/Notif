using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Core.Exceptions;
using Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static T FromJsonString<T>(this string jsonString) =>
            JsonHelper.FromJsonString<T>(jsonString);

        public static string Remove(this string text, string textToRemove) =>
            text.Replace(textToRemove, "");

        public static bool TryGetSqlInjectionFreeText(this string orderByText, out string sqlInjectionFreeText)
        {
            sqlInjectionFreeText = string.Empty;

            if (string.IsNullOrWhiteSpace(orderByText)) return false;

            var match = Regex.Match(orderByText, @"^([a-zA-Z0-9\[\]_\.]){2,}(\s+(\b(asc|desc)\b)+){0,1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!match.Success) return false;

            sqlInjectionFreeText = match.Value;

            return true;
        }

        public static bool TryGetSqlInjectionFreeText(this string orderByText, out (string ColumnName, bool IsAscending) sqlInjectionFreeText)
        {
            sqlInjectionFreeText = (string.Empty, false);
            if (string.IsNullOrWhiteSpace(orderByText)) return false;

            var isValid = TryGetSqlInjectionFreeText(orderByText, out string sqlInjectionFreeTextString);
            if (!isValid)
                return false;

            var splittedText = sqlInjectionFreeTextString.Split(' ');
            var isDescending = false;

            if (splittedText.Length > 1)
                isDescending = splittedText[1].Equals("desc", StringComparison.InvariantCultureIgnoreCase);

            sqlInjectionFreeText = (splittedText[0], !isDescending);

            return isValid;
        }

        public static Type ToType(this string typeName)
        {
                if (string.IsNullOrWhiteSpace(typeName))
                    throw new NitroException(Enums.ExceptionCodeEnum.InvalidInput, $"{nameof(typeName)} is not valid .");

            return Type.GetType(typeName, true, true);
        }

        public static DateTime GetDate(string persianDate)
        {
            DateTime dateTime;
            persianDate = persianDate.Replace("/", "");

            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            dateTime = pc.ToDateTime(
                int.Parse(persianDate.Substring(0, 4)),
                int.Parse(persianDate.Substring(4, 2)),
                int.Parse(persianDate.Substring(6, 2)),
                1, 1, 1, 1);

            return dateTime;
        }

        ///// <summary>
        ///// Instantiates a type from type name
        ///// </summary>
        ///// <param name="typeName">Class type name to instantiate</param>
        ///// <returns>The instantiated object</returns>
        //public static object CreateInstance(this string typeName)
        //{
        //    if (string.IsNullOrWhiteSpace(typeName))
        //        throw new NitroException(Enums.ExceptionCodeEnum.InvalidInput, $"{nameof(typeName)} is not valid to instantiate.");


        //    return Activator.CreateInstance(Type.GetType(typeName));
        //}
        //public static bool TryCreateInstance<T>(this string typeName, out T instance) where T : class
        //{
        //    try
        //    {
        //        //var objInstance = ServiceLocator.GetService(typeName);
        //        var objInstance = typeName.CreateInstance();

        //        instance = (T)objInstance;
        //    }
        //    catch (Exception ex)
        //    {
        //        instance = null;
        //        return false;
        //    }

        //    return true;
        //}

    }
}
