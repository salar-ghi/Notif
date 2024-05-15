namespace Infrastructure.Services.EntityFramework;

public class SmsNotifSender : ISmsProvider, INotifSender
{
    private readonly NotifContext _context;
    private readonly IMapper _mapper;

    public SmsNotifSender(NotifContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<IEnumerable<ProviderRs>> GetSmsProviders()
    {
        // ************############## Create Provider Response Model  ????????????????????????????????????
        var provider = await _context.Provider
            .Where(z => z.Type == ProviderType.Mobile && z.IsEnabled == true)
            .Select(x => new ProviderRs 
            { 
                Name = x.Name, 
                Type = x.Type,
                JsonConfig = x.JsonConfig,
                Description = x.Description,
            })
            .AsNoTracking()
            .ToListAsync();

        return provider;
    }

    //public async Task<ProviderRs> GetRandomSmsProvider()
    //{
    //    var providerNum = await _context.Providers
    //        .Where(z => z.Type == ProviderType.Mobile && z.IsEnabled == true)
    //        .AsNoTracking()
    //        .CountAsync();

    //    var randomNum = Random.Shared.Next(0, providerNum);

    //    var randomProvider = await _context.Providers
    //        .Where(z => z.Type == ProviderType.Mobile && z.IsEnabled == true)
    //        .OrderBy(x => x.Priority)
    //        //.Skip(randomNum)
    //        //.Take(1)
    //        .Select(x => new
    //        {
    //            x.Name,
    //            x.JsonConfig
    //        })
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync();

    //    return randomProvider;
    //}



    public async Task<ProviderRs> GetRandomSmsProvider()
    {
        var provider = await _context.Provider
            .Where(z => z.Type == ProviderType.Mobile && z.IsEnabled == true)
            .OrderBy(x => x.Priority)
            .Select(x => new ProviderRs
            {
                Name = x.Name,
                Type = x.Type,
                JsonConfig = x.JsonConfig
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return provider;
    }

    public async Task<ProviderRs> GetSmsProvider(string name)
    {
        // ************############## Create Provider Response Model  ????????????????????????????????????
        //var provider = await _context.Providers
        //    .Where(z => z.Type == ProviderType.Mobile && z.IsEnabled == true && z.Name == name)
        //    .Select(x => new ProviderRs
        //    {
        //        Name = x.Name,
        //        Type = x.Type,
        //        JsonConfig = x.JsonConfig,
        //    })
        //    .AsNoTracking().SingleOrDefaultAsync().ConfigureAwait(false);

        var provider =  await _context.Provider.Where(z => z.Name == name && z.IsEnabled == true).FirstOrDefaultAsync();

        ProviderRs prv = new ProviderRs()
        {
            Name = provider.Name,
            Type = provider.Type,
            JsonConfig = provider.JsonConfig,
        };

        return prv;
    }

    public async Task<ProviderRs> SendNotificationAsync(Notif notif, string providerName)
    {
        var provider = new ProviderRs();
        if (!string.IsNullOrEmpty(providerName))
        {
            // enter name of sms provider
            provider = await GetSmsProvider(providerName);
        }
        else
        {
            provider = await GetRandomSmsProvider();
        }

        Console.WriteLine($"Sending SMS notification: {notif.Message}");
        return provider;
    }


}
