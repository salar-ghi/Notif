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
    public Task SendEmailAsync(Notif message)
    {
        throw new NotImplementedException();
    }

    #endregion
}
