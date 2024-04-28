using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace WebCore.Configuration
{
    public abstract class HealthCheck : IHealthCheck
    {
        private readonly ILogger _logger;

        public HealthCheck(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException($"{nameof(ILogger)} is null");

        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var healthCheckResultHealthy = true;

            try
            {
                CheckHealthAsync();
            }
            catch (Exception ex)
            {
                var msg = "Application is unhealthy.";
                _logger.LogError(ex, msg);
                return Task.FromResult(
                    new HealthCheckResult(context.Registration.FailureStatus, msg, ex));
            }

            if (healthCheckResultHealthy)
                return Task.FromResult(
                    HealthCheckResult.Healthy());
            else
                return Task.FromResult(
                        new HealthCheckResult(context.Registration.FailureStatus));
        }

        public abstract Task CheckHealthAsync();
    }

}
