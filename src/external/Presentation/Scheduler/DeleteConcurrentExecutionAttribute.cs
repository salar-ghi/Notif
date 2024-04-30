using Hangfire.Common;
using Hangfire.Server;

namespace Presentation.Scheduler;

public class DeleteConcurrentExecutionAttribute : JobFilterAttribute, IServerFilter
{
    public void OnPerforming(PerformingContext filterContext)
    {
        var processingJobs = JobStorage.Current.GetMonitoringApi().ProcessingJobs(0, int.MaxValue);
        var scheduledJobs = JobStorage.Current.GetMonitoringApi().ScheduledJobs(0, int.MaxValue);

        //if (processingJobs.Count(x => x.Value?.Job?.ToString() == filterContext.BackgroundJob.Job.ToString()) > 1)
        //{
        //    filterContext.CancellationToken.ThrowIfCancellationRequested();
        //    //filterContext.CancellationToken.ShutdownToken.ThrowIfCancellationRequested();
        //    filterContext.Canceled = true;
        //    BackgroundJob.Delete(filterContext.BackgroundJob.Id);
        //}

        //if (scheduledJobs.Count(x => x.Value?.Job?.ToString() == filterContext.BackgroundJob.Job.ToString()) > 0)
        //{
        //    foreach (var job in scheduledJobs.Where(x => x.Value?.Job?.ToString() == filterContext.BackgroundJob.Job.ToString()))
        //    {
        //        BackgroundJob.Delete(job.Key);
        //    }
        //}
    }

    public void OnPerformed(PerformedContext filterContext)
    {
    }
}
