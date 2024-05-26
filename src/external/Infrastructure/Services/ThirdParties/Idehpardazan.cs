namespace Infrastructure.Services.ThirdParties;

public class Idehpardazan : IIdehpardazan
{
    #region Definition & Ctor
    private readonly ILogger<Idehpardazan> _logger;
    public Idehpardazan(ILogger<Idehpardazan> logger)
    {
        _logger = logger;
    }
    #endregion


    #region Methods

    public Task SendIdehpardazSmsAsync(Message message)
    {
        Console.WriteLine($"Sending Sms notification: {message}");
        return Task.CompletedTask;
    }

    public Task<bool> SendAsync(string ProviderName, Message message)
    {
        try
        {
            Console.WriteLine($"Sending Sms notification: {message}");
            _logger.LogInformation(ProviderName, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }

        throw new NotImplementedException();
    }
    #endregion

}
