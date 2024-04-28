using Core.Constants;
using Core.DomainValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using Core.Extensions;
using Core.Helpers;
using Core.Exceptions;
using Core;
using System.Threading;
using WebCore.Extensions;

namespace WebCore.Helpers
{
    public sealed class GlobalErrorHandler : IExceptionHandler
    {
        private readonly IHostEnvironment _environment;
        private readonly ILogger<GlobalErrorHandler> _logger;

        public GlobalErrorHandler(IHostEnvironment environment, ILogger<GlobalErrorHandler> logger)
        {
            _environment = environment;
            _logger = logger;

        }

        public async Task WriteDevelopmentResponse(HttpContext httpContext)
            => await WriteResponse(httpContext, includeDetails: true);

        public async Task WriteProductionResponse(HttpContext httpContext)
            => await WriteResponse(httpContext, includeDetails: false);

        private async Task WriteResponse(HttpContext httpContext, bool includeDetails)
        {
            // Try and retrieve the error from the ExceptionHandler middleware
            var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = exceptionDetails?.Error;

            // Should always exist, but best to be safe!
            if (ex == null) await Task.CompletedTask;

            // ProblemDetails has it's own content type
            httpContext.Response.ContentType = "application/problem+json";

            // Get the details to display, depending on whether we want to expose the raw exception
            var title = includeDetails ? "An error occured: " + ex.Message : "An error occured";
            var details = includeDetails ? ex.ToString() : string.Empty;

            var otherParameters = new Dictionary<string, string>();
            // This is often very handy information for tracing the specific request
            var correlationId = Extensions.HttpContextExtensions.GetCorrelationId(httpContext);

            if (!string.IsNullOrWhiteSpace(details))
                otherParameters.Add("Details", details);

            if (!string.IsNullOrWhiteSpace(correlationId))
                otherParameters.Add(GlobalConstants.CorrelationIdKey, correlationId);

            if (ex.Data != null)
                otherParameters.Add("Data", ex.Data.ToJsonString());

            var problem = APIResultHelper.RestResultBody($"{title}", otherParameters);

            _logger.LogError($"{title}", problem);

            switch (ex)
            {
                case DomainValidationException:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                case ForbiddenException:
                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    break;
                case UnauthorizedAccessException:
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    break;

            }

            await httpContext.Response.WriteAsJsonAsync(problem);

            //Serialize the problem details object to the Response as JSON (using System.Text.Json)
            //await System.Text.Json.JsonSerializer.SerializeAsync(httpContext.Response.Body, problem);
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            try
            {
                if (_environment.IsProduction())
                    await WriteProductionResponse(httpContext);
                else
                    await WriteDevelopmentResponse(httpContext);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, ex, "Global Error handler encountere with an error");

                return false;
            }
        }
    }
}
