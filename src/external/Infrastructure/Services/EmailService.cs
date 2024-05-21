namespace Infrastructure.Services;

public class EmailService : IEmailProvider
{
    #region Definition & CTor
    
    private readonly ISendGridEmail _sendGridEmail;
    public EmailService(ISendGridEmail sendGridEmail)
    {
        _sendGridEmail = sendGridEmail;
    }



    #endregion


    #region Methods

    public Task BuyAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendAsync(string ProviderName, Notif message)
    {
        throw new NotImplementedException();
    }

    #endregion
}
