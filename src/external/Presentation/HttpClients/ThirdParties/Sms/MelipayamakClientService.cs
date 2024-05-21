namespace Presentation.HttpClients.ThirdParties.Sms;

public class MelipayamakClientService : HttpClientService<MelipayamakClientService>, IMelipayamakClientService
{
    private readonly IMapper _mapper;
    private readonly ApplicationSettingExtenderModel _configuration;
    public MelipayamakClientService(HttpClient httpClient,
        ApplicationSettingExtenderModel configuration,
        IMapper mapper)
        : base(httpClient)
    {
        _mapper = mapper;
        _configuration = configuration;
    }
}
