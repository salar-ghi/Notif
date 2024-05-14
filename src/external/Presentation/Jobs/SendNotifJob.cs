namespace Presentation.Jobs;

public class SendNotifJob : ISaveNotifToStorageJob
{
    private readonly ILogger<SendNotifJob> _logger;
    private readonly ApplicationSettingExtenderModel _applicationSettings;
    private readonly INotifManagementService _notifManagement;

    public SendNotifJob(ILogger<SendNotifJob> logger,
         ApplicationSettingExtenderModel applicationSettings, INotifManagementService notifManagement)
    {
        _logger = logger;
        _applicationSettings = applicationSettings;
        //_cache = cache;
        _notifManagement = notifManagement;
    }

    public async Task Run()
    {
        try
        {

            var notif = await _notifManagement.CheckCacheAndSaveToStorage();


        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }


}
