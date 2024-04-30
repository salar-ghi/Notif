using Hangfire;
using Hangfire.SqlServer;
using Presentation.Scheduler;

namespace Presentation.Extensions;

public static class IServiceCollectionExtensions
{
    public static void ConfigHangfire(this IServiceCollection services, IConfiguration configuration, string appName, IWebHostEnvironment webHostEnvironment)
    {
        var hangfireConnectionString = configuration.GetConnectionString("HangFireConnection");
        var sqlServerOptions = new SqlServerStorageOptions
        {
            PrepareSchemaIfNecessary = true,
            SlidingInvisibilityTimeout = TimeSpan.FromSeconds(30),
            QueuePollInterval = TimeSpan.Zero,
        };
        GlobalConfiguration.Configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(hangfireConnectionString, sqlServerOptions);

        services.AddHangfire(o => o.UseSqlServerStorage(hangfireConnectionString));

        services.AddHangfireServer(x =>
        {
            x.Queues = new[] { appName.ToLower() };
            x.StopTimeout = TimeSpan.FromSeconds(30);
            x.MaxDegreeOfParallelismForSchedulers = 10;
            x.SchedulePollingInterval = TimeSpan.FromMinutes(3);
        });
        //GlobalJobFilters.Filters.Add(new Delete)
        GlobalJobFilters.Filters.Add(new PreserveOriginalQueueAttribute());
        GlobalJobFilters.Filters.Add(new DeleteConcurrentExecutionAttribute());
        GlobalJobFilters.Filters.Add(new DisableConcurrentExecutionAttribute(15));

    }
}
