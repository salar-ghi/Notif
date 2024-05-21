
using Castle.Core.Logging;
using Infrastructure.Services.EntityFramework;
using Infrastructure.Services.ThirdParties;

namespace Infrastructure.Services;

//public class SmsService : ProviderRepository, ISmsProvider
public class SmsService : ISmsProvider
{
    #region Definition & CTor
    private readonly IIdehpardazan _idehpardazan;
    private readonly IMelipayamak _melipayamak;
    private readonly IPayamSms _payamSms;
    private readonly ILogger<SmsService> _logger;
    private readonly IServiceProvider _serviceProvider;
    public SmsService(IIdehpardazan idehpardazan, IMelipayamak melipayamak, IPayamSms payamSms,
        ILogger<SmsService> logger, IServiceProvider serviceProvider)
    {
        _idehpardazan = idehpardazan;
        _melipayamak = melipayamak;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _payamSms = payamSms;
    }

    #endregion

    #region Methods

    //public async Task<bool> SendSmsAsync(string providerName, Notif message)
    //{
    //    try
    //    {
    //        var providerServie = GetService(providerName);
    //        switch (providerName)
    //        {
    //            case "MeliPayamak":
    //                var meli = providerServie as IMelipayamak;
    //                await meli.SendMelipayamakSmsAsync(message);
    //                break;
    //            case "idehpardazan":
    //                var ideh = providerServie as IIdehpardazan;
    //                await ideh.SendIdehpardazSmsAsync(message);
    //                break;
    //        }
    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger?.LogError(ex.Message, ex);
    //        return false;
    //    }
    //}


    public async Task<bool> SendAsync(string providerName, Notif message)
    {
        try
        {
            var provider = GetService(providerName);
            switch (providerName)
            {
                case "MeliPayamak":
                    await _melipayamak.SendMelipayamakSmsAsync(message);
                    break;
                case "Idehpardazan":
                    await _idehpardazan.SendIdehpardazSmsAsync(message);
                    break;
                case "PayamSms":
                    //await _payamSms.SendAsync(providerName, message);
                    await provider.SendAsync(providerName, message);
                    break;
            }
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
                "PayamSms" => _serviceProvider.GetRequiredService<PayamSms>(),
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
