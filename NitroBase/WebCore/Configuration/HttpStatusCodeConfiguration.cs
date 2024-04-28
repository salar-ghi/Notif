using Core.Constants;
using WebCore.Dtos;
using Core.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
using System.Net;
using WebCore.Extensions;

namespace WebCore.Configuration
{
    public static class StatusCodeContextExtensions
    {
        public static async Task UseHttpStatusCodePagesAsync(this StatusCodeContext context,
                                                     IApplicationBuilder app,
                                                     ILoggerFactory loggerFactory,
                                                     IWebHostEnvironment environment)
        {
            app.Use(async (context, next) =>
            {
                var responseStatusCode = context.Response.StatusCode;

                var correlationId = HttpContextExtensions.GetCorrelationId(context);

                var logger = loggerFactory.CreateLogger<ILogger>();

                var hasValidResponse = await StreamHelper.TryGetString(context.Response.Body, out string responseBody);
                if (hasValidResponse && ResponseContentHasValidContent(responseBody)) return;

                if (responseStatusCode < 200 || responseStatusCode > 299)
                    await Log(responseStatusCode, logger, context, environment, correlationId);

                if (responseStatusCode >= 401 && responseStatusCode < 404)
                    await HandleUnauthorisedAccessAsync(context, app, loggerFactory, environment, correlationId);
                if (responseStatusCode == 404)
                    await HandleNotFoundAsync(context, app, loggerFactory, environment, correlationId);
                else if (responseStatusCode >= 500 && responseStatusCode < 600)
                    await HandleInternalServerErrorsAsync(context, app, loggerFactory, environment, correlationId);

                await next(context);
            });
        }



        //public static async Task HandleHttpStatusCodePagesAsync(this StatusCodeContext context,
        //                                             IApplicationBuilder app,
        //                                             ILoggerFactory loggerFactory,
        //                                             IWebHostEnvironment environment)
        //{
        //    var responseStatusCode = context.HttpContext.Response.StatusCode;

        //    var correlationId = HttpContextHelper.GetCorrelationId(context.HttpContext);

        //    var logger = loggerFactory.CreateLogger<ILogger>();

        //    if (await StreamHelper.TryGetString(context.HttpContext.Response.Body, out string responseBody) &&
        //        ResponseContentHasValidContent(responseBody)) return;

        //        if (responseStatusCode < 200 || responseStatusCode > 299)
        //        await Log(responseStatusCode, logger, context.HttpContext, environment, correlationId);

        //    if (responseStatusCode >= 401 && responseStatusCode < 404)
        //        await HandleUnauthorisedAccessAsync(context, app, loggerFactory, environment, correlationId);
        //    if (responseStatusCode == 404)
        //        await HandleNotFoundAsync(context, app, loggerFactory, environment, correlationId);
        //    else if (responseStatusCode >= 500 && responseStatusCode < 600)
        //        await HandleInternalServerErrorsAsync(context, app, loggerFactory, environment, correlationId);

        //}

        public static async Task HandleUnauthorisedAccessAsync(HttpContext context,
                                                               IApplicationBuilder app,
                                                               ILoggerFactory loggerFactory,
                                                               IWebHostEnvironment environment,
                                                               string correlationId)
        {
            var responseStatusCode = context.Response.StatusCode;
            if (responseStatusCode != StatusCodes.Status401Unauthorized &&
                responseStatusCode != StatusCodes.Status403Forbidden)
                return;

            context.Response.ContentType = "application/json";
            var responseContent = new NotOkResultDto(GlobalConstants.Message_AccessDenied_Message);
            await context.Response.WriteAsync(responseContent.ToJsonString());
        }

        public static async Task HandleNotFoundAsync(HttpContext context,
                                               IApplicationBuilder app,
                                               ILoggerFactory loggerFactory,
                                               IWebHostEnvironment environment,
                                               string correlationId)
        {
            var responseStatusCode = context.Response.StatusCode;
            if (responseStatusCode < 400 || responseStatusCode > 499)
                return;

            context.Response.ContentType = "application/json";
            var responseContent = new NotOkResultDto(GlobalConstants.Message_InternalServerError);
            await context.Response.WriteAsync(responseContent.ToJsonString());
        }

        public static async Task HandleInternalServerErrorsAsync(HttpContext context,
                                                           IApplicationBuilder app,
                                                           ILoggerFactory loggerFactory,
                                                           IWebHostEnvironment environment,
                                                           string correlationId)
        {
            var responseStatusCode = context.Response.StatusCode;
            if (responseStatusCode < 500 || responseStatusCode > 599)
                return;

            context.Response.ContentType = "application/json";
            var responseContent = new NotOkResultDto(GlobalConstants.Message_InternalServerError);
            await context.Response.WriteAsync(responseContent.ToJsonString());
        }

        private static async Task Log(int statusCode, ILogger logger, HttpContext context, IWebHostEnvironment environment, string correlationId)
        {
            await StreamHelper.TryGetString(context.Request.Body, out string RequestBody);

            var logData = new
            {
                correlationId,
                Application = $"{environment.ApplicationName}.{environment.EnvironmentName}",
                RequestBody = RequestBody,
                RequestHeaders = context.Request.Headers,
                RequestQuerystring = context.Request.QueryString,
                RequestPath = context.Request.Path
            };
            logger.LogInformation($"{statusCode}: {logData.ToJsonString()}");
        }

        private static bool ResponseContentHasValidContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return false;
            return StringHelper.TryGetObject<NotOkResultDto>(content, out _);
        }
    }

}
