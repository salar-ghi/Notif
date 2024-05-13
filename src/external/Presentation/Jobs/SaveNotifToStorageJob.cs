namespace Presentation.Jobs;

public class SaveNotifToStorageJob : ISaveNotifToStorageJob
{
    private readonly ILogger<SaveNotifToStorageJob> _logger;
    private readonly ApplicationSettingExtenderModel _applicationSettings;
    private readonly ICacheMessage _cache;

    public SaveNotifToStorageJob(ILogger<SaveNotifToStorageJob> logger,
         ApplicationSettingExtenderModel applicationSettings, ICacheMessage cache)
    {
        _logger = logger;
        _applicationSettings = applicationSettings;
        _cache = cache;
    }

    public async Task Run()
    {
        try
        {
            var notif = await _cache.GetAllMessages();
            //if (notif != null)



        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }
}
