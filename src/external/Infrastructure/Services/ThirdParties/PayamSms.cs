using Application.Services.Abstractions.HttpClients.ThirdParties;

namespace Infrastructure.Services.ThirdParties;

public class PayamSms : IPayamSms
{
    #region Definition & Ctor
    private readonly IPayamSmsClientService _payamClientService;
    private readonly ILogger<PayamSms> _logger;
    public PayamSms(IPayamSmsClientService payamSmsClientService, ILogger<PayamSms> logger)
    {
        _payamClientService = payamSmsClientService;
        _logger = logger;
    }
    #endregion


    #region Methods


    public Task SendPayamSmsAsync(Message message)
    {
        Console.WriteLine($"Sending Sms notification: {message}");
        return Task.CompletedTask;
    }

    public async Task<bool> SendAsync(string ProviderName, Message message)
    {
        try
        {
            Console.WriteLine($"Sending Sms notification from PayamSms: {message}");
            var result = await _payamClientService.SendPayamSms(message);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return false;
        }

        
        
    }
    #endregion

}
