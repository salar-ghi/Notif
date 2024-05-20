namespace Application.Services.Abstractions;

public interface ISmsProvider: IProvider
{

}

public interface IProvider 
{
    Task<bool> SendAsync(string ProviderName, Notif message);

}
