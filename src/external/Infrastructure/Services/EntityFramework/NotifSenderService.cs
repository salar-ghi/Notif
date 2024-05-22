namespace Infrastructure.Services.EntityFramework;

public class NotifSenderService : INotifSender
{
    #region Definition & Ctor
    private readonly ISmsProvider _smsProvider;
    private readonly IEmailProvider _emailProvider;
    private readonly ILogger<NotifSenderService> _logger;
    private readonly INotifService _notif;
    private readonly INotifLogService _notifLog;
    public NotifSenderService(ISmsProvider smsProvider, IEmailProvider emailProvider, ILogger<NotifSenderService> logger, INotifService notif, INotifLogService notifLog)
    {
        _smsProvider = smsProvider;
        _emailProvider = emailProvider;
        _logger = logger;
        _notif = notif;
        _notifLog = notifLog;
    }
    #endregion


    #region Methods


    public async Task<bool> SendNotifAsync(string providerName, Notif notif)
    {
        try
        {
            switch (notif.Type)
            {
                case NotifType.SMS:
                    var result = await _smsProvider.SendAsync(providerName, notif);
                    break;
                case NotifType.Email:
                    await _emailProvider.SendAsync(providerName, notif);
                    break;
                case NotifType.Signal:
                    break;
                case NotifType.MessageBrocker:
                    break;
                case NotifType.Telegram:
                    break;
                case NotifType.Whatsapp:
                    break;
                default:
                    break;
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return false;
        }
    }




    public async Task<bool> ManageNotif(Provider provider, Notif notif, CancellationToken ct)
    {
        var notLog = new NotifLog();
        try
        {
            var notifSender = await this.SendNotifAsync(provider.Name, notif);
            notLog = new()
            {
                NotifId = notif.Id,
                ProviderId = provider.Id, // ????????????????????
                Success = notifSender,
                SentAt = DateTime.UtcNow,
            };

            if (notifSender)
            {
                await _notif.MarkNotificationsAsReadAsync(notif, ct);
            }
            else
            {
                await _notif.MarkNotificationAsFailedAttemp(notif, ct);
            }
            await _notifLog.SaveNotifLogAsync(notLog, ct);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.Message, ex);
            notLog = new()
            {
                NotifId = notif.Id,
                ProviderId = provider.Id, // ????????????????????
                Success = false,
                FailureReason = ex.Message,
                SentAt = DateTime.UtcNow,
            };
            await _notifLog.SaveNotifLogAsync(notLog, ct);
            throw;
        }
    }

    #endregion

}
