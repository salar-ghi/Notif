using Domain.Entities;

namespace Presentation.Jobs;

public class JobScheduler
{
    public static void ScheduleJobs(IApplicationBuilder app, ApplicationSettingExtenderModel aplicationSettings)
    {
        var appName = "nitro-Notif";

        //BackgroundJob.Enqueue<SaveNotifToStorageJob>(x => x.Run());

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
