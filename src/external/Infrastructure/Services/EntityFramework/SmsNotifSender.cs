using Application.Models.Responses;
using AutoMapper;
using Domain.Entities;
using System.Data.Common;

namespace Infrastructure.Services.EntityFramework;

public class SmsNotifSender : ISmsProvider, INotifSender
{
    private readonly NotifContext _context;
    private readonly IMapper _mapper;

    public SmsNotifSender()
    {
        _context = new NotifContext();
        //_mapper = mapper;
    }


    public async Task<IEnumerable<Provider>> GetSmsProviders()
    {
        // ************############## Create Provider Response Model  ????????????????????????????????????
        IEnumerable<ProviderRs> ProviderRs = new List<ProviderRs>();
        var provider = await _context.Providers
            .Where(z => z.Type == ProviderType.Mobile && z.IsEnabled == true)
            .Select(x => new { 
                x.Name, 
                x.Description,
                x.JsonConfig, 
                x.Type})
            .AsNoTracking()
            .ToListAsync();


        return provider;
    }

    public async Task<Provider> GetRandomSmsProvider()
    {
        var providerNum = await _context.Providers
            .Where(z => z.Type == ProviderType.Mobile && z.IsEnabled == true)
            .AsNoTracking()
            .CountAsync();
        
        var randomNum = Random.Shared.Next(0, providerNum);

        var randomProvider = await _context.Providers
            .Where(z => z.Type == ProviderType.Mobile && z.IsEnabled == true)
            .OrderBy(x => x.Priority)
            //.Skip(randomNum)
            //.Take(1)
            .Select (x => new
            {
                x.Name,
                x.JsonConfig
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return randomProvider;
    }


    public async Task<Provider> GetSmsProvider(string name)
    {
        // ************############## Create Provider Response Model  ????????????????????????????????????
        var provider = await _context.Providers
            .Where(z => z.Type == ProviderType.Mobile && z.IsEnabled == true && z.Name.Equals(name))
            .Select(x => new
            {
                x.Name,
                x.JsonConfig,
                x.Type
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return provider;
    }

    public async Task SendNotificationAsync(Notif notif, string providerName)
    {

        if (!string.IsNullOrEmpty(providerName))
        {
            // enter name of sms provider
            var provider = await GetSmsProvider(providerName);
        }
        else
        {
            var provider = await GetRandomSmsProvider();
        }

        Console.WriteLine($"Sending SMS notification: {notif.Message}");
        await Task.CompletedTask;
    }


}
