using Application.Services.Abstractions.HttpClients.ThirdParties;

namespace Infrastructure.Services.ThirdParties;

public class PayamSms : IPayamSms
{
    #region Definition & Ctor
    private readonly IPayamSmsClientService _payamSmsClientService;
    public PayamSms(IPayamSmsClientService payamSmsClientService)
    {
        _payamSmsClientService = payamSmsClientService;
    }
    #endregion


    #region Methods


    public Task SendPayamSmsAsync(Notif message)
    {
        Console.WriteLine($"Sending Sms notification: {message}");
        return Task.CompletedTask;
    }

    public async Task<bool> SendAsync(string ProviderName, Notif message)
    {
        Console.WriteLine($"Sending Sms notification from PayamSms: {message}");
        return true;
    }
    #endregion

}
