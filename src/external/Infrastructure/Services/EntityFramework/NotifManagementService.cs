namespace Infrastructure.Services.EntityFramework;

public class NotifManagementService : INotifManagementService
{
    #region Definition & Ctor
    private readonly ILogger<NotifManagementService> _logger;
    private readonly IMapper _mapper;

    private readonly INotifService _notif;
    private readonly IProviderService _provider;
    private readonly INotifLogService _notifLog;
    private readonly ICacheMessage _cache;
    
    public NotifManagementService(ILogger<NotifManagementService> logger, IMapper mapper,
        INotifService notif, IProviderService provider, INotifLogService notifLog, ICacheMessage cache)
    {
        _logger = logger;
        _mapper = mapper;

        _notif = notif;
        _provider = provider;
        _notifLog = notifLog;

        _cache = cache;
    }
    #endregion

    #region Methods

    public async Task<bool> CheckCacheAndSaveToStorage(CancellationToken ct = default(CancellationToken))
    {
        try
        {
            var entities = await _cache.GetAllMessages();

            ICollection<NotifLog> notifLogCol = new HashSet<NotifLog>();
            foreach (var entity in entities)            
            {
                var provider = !string.IsNullOrEmpty(entity.ProviderName) ? await _provider.GetSpecificProvider(entity.ProviderName) : await _provider.GetRandomProvider(entity.ProviderName, entity.Type);
                provider = provider ?? await _provider.GetRandomProvider(entity.ProviderName, entity.Type);
                
                var notif = await _notif.SaveNotifAsync(entity, ct);


                var notLog = new NotifLog
                {
                    NotifId = notif.Id,
                    ProviderId = provider.Id,
                };
                notifLogCol.Add(notLog);
            };
            await _notifLog.SaveNotifLogAsync(notifLogCol, ct);


            // ??????????????????? start to remove saved items to storage from cache ***********
            // ??????????????????? start to remove saved items to storage from cache ***********
            // ??????????????????? start to remove saved items to storage from cache ***********
            await _cache.RemoveMessage(entities.ToList());

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }

    #endregion
}
