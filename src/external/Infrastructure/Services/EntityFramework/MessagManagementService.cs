using Domain.Entities;
using Hangfire.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.EntityFramework;

public class MessagManagementService : IMessageManagementService
{
    #region Definition & Ctor
    private readonly ILogger<MessagManagementService> _logger;
    private readonly IMapper _mapper;

    private readonly IMessageService _notif;
    private readonly IProviderService _provider;
    private readonly IMessageLogService _notifLog;
    private readonly ICacheMessage _cache;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageSender _notifSender;

    public MessagManagementService(ILogger<MessagManagementService> logger, IMapper mapper, IProviderService provider,
        IMessageService notif, IMessageLogService notifLog, ICacheMessage cache, IServiceProvider serviceProvider,
        IMessageSender notifSender)
    {
        _logger = logger;
        _mapper = mapper;
        _notif = notif;
        _provider = provider;
        _notifLog = notifLog;
        _cache = cache;
        _serviceProvider = serviceProvider;
        _notifSender = notifSender;
    }
    #endregion

    #region Methods

    public async Task<bool> CheckCacheAndSaveToStorage(CancellationToken ct = default(CancellationToken))
    {
        try
        {
            var entities = await _cache.GetAllMessages();
            if (!entities.IsNullOrEmpty())
            {
                await _notif.SaveNotifAsync(entities, ct);
                await _cache.RemoveMessage(entities.ToList());
            }

            //ICollection<NotifLog> notifLogCol = new HashSet<NotifLog>();
            //foreach (var entity in entities)
            //{
            //    var provider = !string.IsNullOrEmpty(entity.ProviderName) ? await _provider.GetSpecificProvider(entity.ProviderName) : await _provider.GetRandomProvider(entity.ProviderName, entity.Type);
            //    provider = provider ?? await _provider.GetRandomProvider(entity.ProviderName, entity.Type);

            //    var notif = await _notif.SaveNotifAsync(entity, ct);


            //    var notLog = new NotifLog
            //    {
            //        NotifId = notif.Id,
            //        ProviderId = provider.Id,
            //    };
            //    notifLogCol.Add(notLog);
            //};
            //await _notifLog.SaveNotifLogAsync(notifLogCol, ct);



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
                "PayamSms" => _serviceProvider.GetRequiredService<PayamSms>(),
                _ => throw new KeyNotFoundException("Provider not found.")
            };
        }
        catch (Exception ex)
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
            Provider provider = new Provider();
            foreach (var item in notifs)
            {
                if (item.ProviderID > 0)
                {
                    provider = await _provider.GetSpecificProvider(item.ProviderID, item.Type);
                }
                else
                {
                    provider = await _provider.GetRandomProvider(item.Type);
                }

                //var providerName = provider.Name;
                //var NotifSender = await _notifSender.SendNotifAsync(providerName, item);
                var NotifSender = await _notifSender.ManageNotif(provider, item, ct);

                //var notLog = new NotifLog
                //{
                //    NotifId = item.Id,
                //    ProviderId = provider.Id,
                //    Success = NotifSender,
                //    SentAt = DateTime.UtcNow,
                //};
                //if (NotifSender)
                //{
                //    await _notif.MarkNotificationsAsReadAsync(item, ct);
                //}
                //else
                //{
                //    await _notif.MarkNotificationAsFailedAttemp(item, ct);
                //}
                //await _notifLog.SaveNotifLogAsync(notLog, ct);
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
        finally
        {

        }

    }

    #endregion
}
