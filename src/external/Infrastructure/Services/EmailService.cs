namespace Infrastructure.Services;

public class EmailService : IEmailProvider
{
    #region Definition & CTor
    
    private readonly IFluentEmail _fluentEmail;
    private readonly ILogger<EmailService> _logger;
    private readonly IMapper _mapper;
    //private readonly IElasticsearchService _elasticsearchService;
    public EmailService(IFluentEmail fluentEmail, ILogger<EmailService> logger, IMapper mapper)
    {
        _fluentEmail = fluentEmail;
        _logger = logger;
        _mapper = mapper;
        //_elasticsearchService = elasticsearchService;
    }

    #endregion


    #region Methods

    public async Task<bool> SendAsync(string ProviderName, Notif message)
    {
        try
        {
            foreach (var recipient in message.Recipients)
            {
                var item = await _fluentEmail
                   .To(recipient.Destination)
                   .Subject(message.Title)
                   .Body(message.Message)
                   .SendAsync();
                if (!item.Successful)
                {

                }
                //await _elasticsearchService.SaveLogAsync(item);
                _logger.LogInformation("HttpClient Response logged: {0}", item);

            }

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
