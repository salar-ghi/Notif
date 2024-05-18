
using Castle.Core.Logging;
using Infrastructure.Services.EntityFramework;

namespace Infrastructure.Services;

public class SmsService : ISmsProvider
{
    #region Definition & CTor
    private readonly IIdehpardazan _idehpardazan;
    private readonly IMelipayamak _melipayamak;
    private readonly ILogger<SmsService> _logger;
    private readonly IServiceProvider _serviceProvider;
    public SmsService(IIdehpardazan idehpardazan, IMelipayamak melipayamak, ILogger<SmsService> logger, IServiceProvider serviceProvider)
    {
        _idehpardazan = idehpardazan;
        _melipayamak = melipayamak;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    #endregion

    #region Methods

    public async Task<bool> SendSmsAsync(string providerName, Notif message)
    {
        try
        {
            var providerServie = GetService(providerName);
            //var provider = (dynamic)null;
            switch (providerName)
            {
                case "MeliPayamak":
                    var provider = providerServie as IMelipayamak;
                    await provider.SendMelipayamakSmsAsync(message);
                    //providersss = _serviceProvider.GetRequiredService<Melipayamak>() as IMelipayamak;
                    
                    break;
                case "idehpardazan":

                    break;
            }
            //var providerType = GetService(providerName);            
            //await providerType.SendSmsAsync(providerName, message);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.Message, ex);
            return false;
        }
    }

    public ISmsProvider GetService(string ProviderName)
    {
        try
        {
            return ProviderName switch
            {
                "MeliPayamak" => _serviceProvider.GetRequiredService<Melipayamak>(),
                "Idehpardazan" => _serviceProvider.GetRequiredService<Idehpardazan>(),
                _ => throw new KeyNotFoundException("Provider not found.")
            };
        }        
        catch (Exception ex)
        {
            _logger?.LogError(ex.Message, ex);
            throw;
        }
    }


    #endregion
}
