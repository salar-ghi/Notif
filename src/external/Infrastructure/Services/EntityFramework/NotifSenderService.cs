namespace Infrastructure.Services.EntityFramework;

public class NotifSenderService : INotifSender
{
    #region Definition & Ctor
    private readonly ISmsProvider _smsProvider;
    private readonly IEmailProvider _emailProvider;
    private readonly ILogger<NotifSenderService> _logger;
    public NotifSenderService(ISmsProvider smsProvider, IEmailProvider emailProvider, ILogger<NotifSenderService> logger)
    {
        _smsProvider = smsProvider;
        _emailProvider = emailProvider;
        _logger = logger;
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
                    var result = await _smsProvider.SendSmsAsync(providerName, notif);
                    break;
                case NotifType.Email:
                    await _emailProvider.SendEmailAsync(notif);
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

    #endregion

}
