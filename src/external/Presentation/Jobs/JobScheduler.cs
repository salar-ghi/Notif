using Domain.Entities;

namespace Presentation.Jobs;

public class JobScheduler
{
    public static void ScheduleJobs(IApplicationBuilder app, ApplicationSettingExtenderModel aplicationSettings)
    {
        var appName = "nitro-Notif";

        //BackgroundJob.Enqueue<CheckStorageBackgroundService>(x => x.ExecuteAsync());

        //RecurringJob.AddOrUpdate<ISaveNotifToStorageJob>($"{appName}.{nameof(SaveNotifToStorageJob)}",
        //    j => j.Run(), Cron.Minutely(), new RecurringJobOptions()
        //    {
        //        //QueueName = appName.ToLower(),
        //        TimeZone = GlobalConstants.GetTehranTimeZoneInfo(),
        //        //MisfireHandling = MisfireHandlingMode.Strict,
        //    });


        //RecurringJob.AddOrUpdate<ISendNotifJob>($"{appName}.{nameof(SendNotifJob)}",
        //    j => j.Run(), Cron.Minutely(), new RecurringJobOptions()
        //    {
        //        //QueueName = appName.ToLower(),
        //        TimeZone = GlobalConstants.GetTehranTimeZoneInfo(),
        //        //MisfireHandling = MisfireHandlingMode.Strict,
        //    });


    }
}
