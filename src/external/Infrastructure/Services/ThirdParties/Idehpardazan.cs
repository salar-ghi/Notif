namespace Infrastructure.Services.ThirdParties;

public class Idehpardazan : IIdehpardazan
{
    #region Definition & Ctor
    public Idehpardazan()
    {
        
    }
    #endregion


    #region Methods

    public Task SendIdehpardazSmsAsync(Message message)
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
