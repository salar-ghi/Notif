namespace Presentation.HttpClients.ThirdParties.Sms;

public class IdehpardazanClientService : HttpClientService<IdehpardazanClientService>, IIdehpardazanClientService
{
    private readonly IMapper _mapper;
    private readonly ApplicationSettingExtenderModel _configuration;
    public IdehpardazanClientService(HttpClient httpClient,
        ApplicationSettingExtenderModel configuration,
        IMapper mapper)
        : base(httpClient)
    {
        _mapper = mapper;
        _configuration = configuration;
    }

}
