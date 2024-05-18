namespace Infrastructure.Services.EntityFramework;

public class NotifSenderService : INotifSender
{
    #region Definition & Ctor
    private readonly ISmsProvider _smsProvider;
    private readonly IEmailProvider _emailProvider;
    public NotifSenderService(ISmsProvider smsProvider, IEmailProvider emailProvider)
    {
        _smsProvider = smsProvider;
        _emailProvider = emailProvider;
    }
    #endregion


    #region Methods


    public async Task SendNotif(Notif notif)
    {
        await _smsProvider.SendSmsAsync(notif);
    }

    public Task SendNotifAsync(Notif notif)
    {
        throw new NotImplementedException();
    }

    #endregion

}
