
namespace Infrastructure.Services.ThirdParties;

public class Melipayamak : IMelipayamak
{
    #region Definition & Ctor
    public Melipayamak()
    {

    }
    #endregion


    #region Methods


    public Task SendMelipayamakSmsAsync(Notif message)
    {
        Console.WriteLine($"Sending Sms notification: {message}");
        return Task.CompletedTask;
    }

    public Task<bool> SendSmsAsync(string ProviderName, Notif message)
    {
        throw new NotImplementedException();
    }
    #endregion
}
