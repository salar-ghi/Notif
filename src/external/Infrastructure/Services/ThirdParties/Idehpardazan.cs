
namespace Infrastructure.Services.ThirdParties;

public class Idehpardazan : ISmsProvider
{
    #region Definition & Ctor
    public Idehpardazan()
    {
        
    }
    #endregion


    #region Methods

    public Task SendSms(Notif message)
    {
        Console.WriteLine($"Sending Sms notification: {message}");
        return Task.CompletedTask;
    }

    public Task SendSmsAsync(Notif message)
    {
        Console.WriteLine($"Sending Sms notification: {message}");
        return Task.CompletedTask;
    }
    #endregion

}
