
using Castle.Core.Smtp;

namespace Infrastructure.Services.EntityFramework;

public class NotifSenderService : INotifSender
{
    #region Definition & Ctor
    private readonly ISmsProvider _smsProvider;
    private readonly IEmailSender _emailSender;
    public NotifSenderService(ISmsProvider smsProvider, IEmailSender emailSender)
    {
        _smsProvider = smsProvider;
        _emailSender = emailSender;
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
