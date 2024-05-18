namespace Application.Services.Abstractions;

public interface IEmailProvider
{
    Task SendEmailAsync(Notif message);
}
