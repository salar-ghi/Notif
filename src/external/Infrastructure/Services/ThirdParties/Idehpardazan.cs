namespace Infrastructure.Services.ThirdParties;

public class Idehpardazan : IIdehpardazan
{
    #region Definition & Ctor
    public Idehpardazan()
    {
        
    }
    #endregion


    #region Methods

    public Task SendIdehpardazSmsAsync(Notif message)
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
