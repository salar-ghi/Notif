using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Core.Constants;

namespace WebCore.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetCorrelationId(this HttpContext httpContext)
        {
            if (httpContext == null)
                return Activity.Current?.Id;

            var correlationId = httpContext.Request.Headers[GlobalConstants.CorrelationIdKey];
            return correlationId.Count > 0 ? correlationId : httpContext.TraceIdentifier;
        }
    }
}
