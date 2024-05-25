namespace Application.Services.Abstractions;

public interface IMessageSender
{
    Task<bool> SendMessageAsync(string providerName, Message notif);
    Task<bool> ManageMessage(Provider provider, Message notif, CancellationToken ct);
}
