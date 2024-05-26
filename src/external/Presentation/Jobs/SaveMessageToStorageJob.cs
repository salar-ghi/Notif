namespace Presentation.Jobs;

public class SaveMessageToStorageJob //: ISaveNotifToStorageJob
{
    private readonly ILogger<SaveMessageToStorageJob> _logger;
    private readonly ApplicationSettingExtenderModel _applicationSettings;
    private readonly IMessageManagementService _notifManagement;

    public SaveMessageToStorageJob(ILogger<SaveMessageToStorageJob> logger,
         ApplicationSettingExtenderModel applicationSettings, IMessageManagementService notifManagement)
    {
        _logger = logger;
        _applicationSettings = applicationSettings;
        _notifManagement = notifManagement;
    }

    //[DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    //[AutomaticRetry(Attempts = 10, DelaysInSeconds = new int[] {  })]
    //public async Task Run()
    //{
    //    try
    //    {
    //        Console.WriteLine($"Running Run Method 'checkCache' at: {DateTime.Now}");
    //        var notif = await _notifManagement.CheckCacheAndSaveToStorage();
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
