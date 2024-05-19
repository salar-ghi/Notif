
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


    public async Task<bool> SendSmsAsync(string providerName, Notif message)
    {
        try
        {
            switch (providerName)
            {
                case "MeliPayamak":
                    await _melipayamak.SendMelipayamakSmsAsync(message);
                    break;
                case "idehpardazan":
                    await _idehpardazan.SendIdehpardazSmsAsync(message);
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
