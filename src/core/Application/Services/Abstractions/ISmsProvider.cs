namespace Application.Services.Abstractions;

public interface ISmsProvider
{
    Task SendSmsAsync(Notif message);
}
