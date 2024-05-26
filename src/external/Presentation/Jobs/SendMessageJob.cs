namespace Presentation.Jobs;

public class SendMessageJob //: ISendNotifJob
{
    private readonly ILogger<SendMessageJob> _logger;
    private readonly IMessageManagementService _notifManagement;

    public SendMessageJob(ILogger<SendMessageJob> logger, IMessageManagementService notifManagement)
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
