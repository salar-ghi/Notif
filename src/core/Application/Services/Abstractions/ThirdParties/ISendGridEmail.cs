namespace Application.Services.Abstractions.ThirdParties;

public interface ISendGridEmail
{
    Task SendEmail(string to, string subject, string body);
}
