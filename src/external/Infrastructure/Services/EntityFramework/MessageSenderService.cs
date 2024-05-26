namespace Infrastructure.Services.EntityFramework;

public class MessageSenderService : IMessageSender
{
    #region Definition & Ctor
    private readonly ISmsProvider _smsProvider;
    private readonly IEmailProvider _emailProvider;
    private readonly ILogger<MessageSenderService> _logger;
    private readonly IMessageService _notif;
    private readonly IMessageLogService _notifLog;
    public MessageSenderService(ISmsProvider smsProvider, IEmailProvider emailProvider, ILogger<MessageSenderService> logger, IMessageService notif, IMessageLogService notifLog)
    {
        _smsProvider = smsProvider;
        _emailProvider = emailProvider;
        _logger = logger;
        _notif = notif;
        _notifLog = notifLog;
    }
    #endregion


    #region Methods

    public async Task<bool> SendMessageAsync(string providerName, Message notif)
    {
        try
        {
            var result = false;
            switch (notif.Type)
            {
                case NotifType.SMS:
                    result = await _smsProvider.SendAsync(providerName, notif);
                    break;
                case NotifType.Email:
                    result = await _emailProvider.SendAsync(providerName, notif);
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
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return false;
        }
    }


    public async Task<bool> ManageMessage(Provider provider, Message notif, CancellationToken ct)
    {
        var notLog = new MessageLog();
        try
        {
            var notifSender = await this.SendMessageAsync(provider.Name, notif);
            notLog = new()
            {
                MessageId = notif.Id,
                ProviderId = provider.Id, // ????????????????????
                Success = notifSender,
                SentAt = DateTime.UtcNow,
            };

            if (notifSender)
            {
                await _notif.MarkMessagesAsReadAsync(notif, ct);
            }
            else
            {
                await _notif.MarkMessageAsFailedAttemp(notif, ct);
            }
            await _notifLog.SaveMessageLogAsync(notLog, ct);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.Message, ex);
            notLog = new()
            {
                MessageId = notif.Id,
                ProviderId = provider.Id, // ????????????????????
                Success = false,
                FailureReason = ex.Message,
                SentAt = DateTime.UtcNow,
            };
            await _notifLog.SaveMessageLogAsync(notLog, ct);
            throw;
        }
    }

    #endregion

}
