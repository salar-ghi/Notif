
namespace Infrastructure.Services.ThirdParties;

public class Melipayamak : ISmsProvider
{
    #region Definition & Ctor
    public Melipayamak()
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
