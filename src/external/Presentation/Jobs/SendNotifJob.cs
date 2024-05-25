namespace Presentation.Jobs;

public class SendNotifJob //: ISendNotifJob
{
    private readonly ILogger<SendNotifJob> _logger;
    private readonly INotifManagementService _notifManagement;

    public SendNotifJob(ILogger<SendNotifJob> logger, INotifManagementService notifManagement)
    {
        _logger = logger;
        _notifManagement = notifManagement;
    }


    //[DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    //[AutomaticRetry(Attempts = 10, DelaysInSeconds = new int[] { 2 })]
    //public async Task Run()
    //{
    //    try
    //    {
    //        Console.WriteLine($"Running Run Method() 'Send' at: {DateTime.Now}");
    //        CancellationToken ct = default(CancellationToken);
    //        var notif = await _notifManagement.SendNotif(ct);
    //        RecurringJob.AddOrUpdate("Save data to cache", () => this.Run(), "*/2 * * * *");
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex.Message, ex);
    //        throw;
    //    }
    //    finally
    //    {
    //        RecurringJob.AddOrUpdate("Save data to cache", () => this.Run(), "*/2 * * * *");
    //    }
    //}


}
