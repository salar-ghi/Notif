using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Core;
using Microsoft.AspNetCore.Http;
using WebCore.Extensions;
using Core.Constants;

namespace WebCore.Services.HttpClients.DelegatingHandlers
{
    public class LoggingDelegateHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggingDelegateHandler(ILogger<LoggingDelegateHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var correlationId = httpContext.GetCorrelationId();
                if (!string.IsNullOrWhiteSpace(correlationId))
                    request.Headers.TryAddWithoutValidation(GlobalConstants.CorrelationIdKey, correlationId);

                var reqLogData = new
                {
                    RequestHeader = request.Headers,
                    RequestContent = request.Content is not null ? await request.Content.ReadAsStringAsync(): null,
                    request.RequestUri,
                    RequestOptions = request.Options,
                };
                _logger.LogInformation("HttpClient Request logged: {0}", reqLogData);

                var response = await base.SendAsync(request, cancellationToken);

                var resLogData = new
                {
                    ResponseHeader = response.Headers,
                    ResponseContent = request.Content is not null ? await response.Content.ReadAsStringAsync() : null,
                    ResponseStatusCode = response.StatusCode,
                    ResponseReasonPhrase = response.ReasonPhrase,
                };
                _logger.LogInformation("HttpClient Response logged: {0}", resLogData);

                return response;
            }
            catch (Exception ex)
            {
                var logData = new
                {
                    Request = request,
                    CorrelationId = _httpContextAccessor.HttpContext.GetCorrelationId(),
                };
                _logger.LogError(ex, "HttpClient call encountered with an error: {0}", logData);
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }
    }

}
