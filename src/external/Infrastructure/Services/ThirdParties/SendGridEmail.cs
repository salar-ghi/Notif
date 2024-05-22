namespace Infrastructure.Services.ThirdParties;

public class SendGridEmail : ISendGridEmail
{
    #region Ctor & DEfinition

    public SendGridEmail()
    {
        
    }
    #endregion

    #region Decleration

    public async Task SendEmail(string to, string subject, string body)
    {
        // SendGrid email implementation
        Console.WriteLine($"Sending email to {to}: {subject}");
    }

    #endregion
}
