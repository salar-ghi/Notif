namespace Presentation.Jobs;

public class SaveNotifToStorageJob : ISaveNotifToStorageJob
{
    private readonly ILogger<SaveNotifToStorageJob> _logger;
    private readonly ApplicationSettingExtenderModel _applicationSettings;
    //private readonly INotifManagementService _notifManagement;

    public SaveNotifToStorageJob(ILogger<SaveNotifToStorageJob> logger,
         ApplicationSettingExtenderModel applicationSettings)
    {
        _logger = logger;
        _applicationSettings = applicationSettings;
        //_cache = cache;
        //_notifManagement = notifManagement;
    }

    public async Task Run()
    {
        try
        {
            //var notif  = await _notifManagement.CheckCacheAndSaveToStorage();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }
}
