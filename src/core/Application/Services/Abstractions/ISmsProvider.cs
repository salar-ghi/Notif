namespace Application.Services.Abstractions;

public interface ISmsProvider
{
    Task<bool> SendSmsAsync(string ProviderName, Notif message);

}
