using System;
using System.Collections.Generic;
using System.Text;
using Core.Helpers;
using Microsoft.Extensions.Logging;

namespace Core.Extensions
{
    public static class ILoggerExtensions
    {
        public static void LogWarning(this ILogger logger, object logData) => logger.LogWarning("{0}",logData.ToJsonString());

        public static void LogError(this ILogger logger, object logData) => logger.LogError("{0}", logData.ToJsonString());

        public static void LogCritical(this ILogger logger, object logData) => logger.LogCritical("{0}", logData.ToJsonString());

    }
}
