namespace Presentation.HttpClients.ThirdParties.Sms;

public class PayamSmsClientService : HttpClientService<PayamSmsClientService>, IPayamSmsClientService
{
    private readonly IMapper _mapper;
    private readonly ApplicationSettingExtenderModel _configuration;
    public PayamSmsClientService(HttpClient httpClient,
        ApplicationSettingExtenderModel configuration, IMapper mapper) : base(httpClient)
    {
        _mapper = mapper;
        _configuration = configuration;
    }





}
