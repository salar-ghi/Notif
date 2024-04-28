using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Extensions;

namespace Core.Helpers
{
    public static partial class StringHelper
    {
        public static bool TryGetObject<T>(string jsonString, out T result)
        {
            result = default;

            if (string.IsNullOrWhiteSpace(jsonString))
                return false;

            try
            {
                result = jsonString.FromJsonString<T>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string UrlCombine(params string[] items)
        {
            if (!items.Any())
                return string.Empty;

            return string.Join("/", items.Where(u => !string.IsNullOrWhiteSpace(u)).Select(u => u.Trim('/', '\\')));
        }

        public static string GetSqlInjectionFreeText(this string orderByText)
        {
            if (string.IsNullOrWhiteSpace(orderByText)) return string.Empty;

            var match = Regex.Match(orderByText, @"^([a-zA-Z0-9\[\]_\.]){2,}\s+(\b(asc|desc)\b){1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!match.Success)
                throw new InvalidOperationException("درخواست شما با خطا مواجه شد.");

            return match.Value;
        }
    }
}
