namespace Application.Services.Abstractions.HttpClients.ThirdParties;

public interface IPayamSmsClientService : IHttpClientService
{
    Task<PayamSmsRs> SendPayamSms(Notif message, CancellationToken ct = default(CancellationToken));
}
