namespace Application.Services.Abstractions.HttpClients.ThirdParties;

public interface IPayamSmsClientService : IHttpClientService
{
    Task<bool> SendPayamSms(Message message, CancellationToken ct = default(CancellationToken));
}
