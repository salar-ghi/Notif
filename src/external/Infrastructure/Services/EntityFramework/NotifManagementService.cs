using Infrastructure.Services.ThirdParties;
using Microsoft.Extensions.DependencyInjection;

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
    private readonly IServiceProvider _serviceProvider;

    public NotifManagementService(ILogger<NotifManagementService> logger, IMapper mapper,
        INotifService notif, IProviderService provider, INotifLogService notifLog, ICacheMessage cache, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _mapper = mapper;

        _notif = notif;
        _provider = provider;
        _notifLog = notifLog;

        _cache = cache;
        _serviceProvider = serviceProvider;

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


    public ISmsProvider GetService(string ProviderName)
    {
        try
        {
            return ProviderName switch
            {
                "MeliPayamak" => _serviceProvider.GetRequiredService<Melipayamak>(),
                "Idehpardazan" => _serviceProvider.GetRequiredService<Idehpardazan>(),
                _ => throw new KeyNotFoundException("Provider not found.")
            };
        }
        catch(Exception ex)
        {
            _logger?.LogError(ex.Message, ex);
            throw;
        }
    }

    public async Task<bool> SendNotif(CancellationToken ct = default(CancellationToken))
    {
        try
        {
            var notifs = await _notif.GetUnDeliveredAsync();
            // call provider 
            foreach (var item in notifs)
            {
                var notifId = item.Id;
                
                var notifLog = await _notifLog.GetNotifLog(notifId);

                var provider = await _provider.GetSpecificProvider(notifLog.ProviderId);

                //var notiflog = 

                //get Notif log and provider
                //???????

                var smsService = GetService(provider.Name);
                var result = smsService.SendSmsAsync(item);

                //var providerService = _provider.GetSpecificProvider(notifType.ToString()) as INotifSender;
                //await providerService.SendNotifAsync(item);
                await _notif.MarkNotificationsAsReadAsync(notifs.ToList(), ct); //???????






            }
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
