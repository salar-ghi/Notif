namespace Application.Services.Abstractions.HttpClients.ThirdParties;

public interface IPayamSmsClientService : IHttpClientService
{
    Task<bool> SendPayamSms(Notif message, CancellationToken ct = default(CancellationToken));
}
