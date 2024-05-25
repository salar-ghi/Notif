namespace Presentation.Extensions;

public static class IServiceCollectionExtensions
{
    public static void ConfigHangfire(this IServiceCollection services, IConfiguration configuration, string appName, IWebHostEnvironment webHostEnvironment)
    {
        var hangfireConnectionString = configuration.GetConnectionString("HangFireConnection");
        var sqlServerOptions = new SqlServerStorageOptions
        {
            PrepareSchemaIfNecessary = true,

            CommandBatchMaxTimeout = TimeSpan.FromSeconds(2),
            SlidingInvisibilityTimeout = TimeSpan.FromSeconds(2),
            QueuePollInterval = TimeSpan.FromSeconds(3),
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true,
            
        };
        GlobalConfiguration.Configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(hangfireConnectionString, sqlServerOptions);

        services.AddHangfire(o => o.UseSqlServerStorage(hangfireConnectionString));

        services.AddHangfireServer();
        //services.AddHangfireServer(x =>
        //{
        //    x.Queues = new[] { appName.ToLower() };
        //    x.StopTimeout = TimeSpan.FromSeconds(2);
        //    x.MaxDegreeOfParallelismForSchedulers = 20;
        //    x.SchedulePollingInterval = TimeSpan.FromSeconds(2);
        //});
        //GlobalJobFilters.Filters.Add(new Delete)
        GlobalJobFilters.Filters.Add(new PreserveOriginalQueueAttribute());
        GlobalJobFilters.Filters.Add(new DeleteConcurrentExecutionAttribute());
        GlobalJobFilters.Filters.Add(new DisableConcurrentExecutionAttribute(5));
    }
}
