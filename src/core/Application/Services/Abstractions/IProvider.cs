namespace Application.Services.Abstractions;

public interface IProvider
{
    Task<bool> SendAsync(string ProviderName, Notif message);
}
