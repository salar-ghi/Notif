using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using Serilog.Core;
using Microsoft.Extensions.Configuration;
using Serilog.Exceptions.Core;
using Serilog.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace WebCore.Loggers.Serilog.Configuration
{
    public static class SerilogConfiguration
    {
        public static Logger CreateLogger(IConfiguration configuration)
        {
            SelfLog.Enable(e => Console.WriteLine(e));
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
                     .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                     .Filter.ByExcluding(Matching.FromSource("Serilog"))
                     .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore"))
                     .Filter.ByExcluding(Matching.FromSource("System.Net"))
                    .WriteTo.Console(
                        LogEventLevel.Verbose,
                        "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}")
                    .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                .WithDefaultDestructurers()
                //.WithDestructurers(new[] { new DbUpdateExceptionDestructurer() })
                )
                    .Enrich.WithCorrelationId()
                    //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearch:Host"])))
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearch:Host"]))
                    {
                        FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                           EmitEventFailureHandling.RaiseCallback,
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                        //IndexFormat = configuration["ElasticSearch:IndexFormat"],
                        IndexFormat = $"{configuration["ElasticSearch:AppName"]}-{DateTime.UtcNow:yyyy-MM}",
                        CustomFormatter = new ExceptionAsObjectJsonFormatter(),

                    })
                    //.WriteTo.File(new RenderedCompactJsonFormatter(), "logs/log.log")
                    .CreateLogger();
            return logger;
        }
    }

}
