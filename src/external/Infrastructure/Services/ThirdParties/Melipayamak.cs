namespace Infrastructure.Services.ThirdParties;

public class Melipayamak : IMelipayamak
{
    #region Definition & Ctor
    public Melipayamak()
    {

    }
    #endregion


    #region Methods


    public Task SendMelipayamakSmsAsync(Message message)
    {
        Console.WriteLine($"Sending Sms notification: {message}");
        return Task.CompletedTask;
    }

    public Task<bool> SendAsync(string ProviderName, Message message)
    {
        throw new NotImplementedException();
    }
    #endregion
}
