using Core.Constants;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Extensions;

namespace WebCore.Filters
{
    public class AssignCorrelationId: IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
                                 
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var correlationId = HttpContextExtensions.GetCorrelationId(context.HttpContext);
            if (!string.IsNullOrWhiteSpace(correlationId))
                context.HttpContext.Response.Headers.TryAdd(GlobalConstants.CorrelationIdKey, correlationId );
        }

    }
}
