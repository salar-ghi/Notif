using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Core.Helpers;

namespace Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJsonString(this object obj) =>
            JsonHelper.ToJsonString(obj);

        public static HttpContent ToHttpContent(this object obj, string contentType = "application/json")
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));

            return new StringContent(obj.ToJsonString(), Encoding.UTF8, contentType);
        }

    }
}
