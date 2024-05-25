using Microsoft.Extensions.Logging;
using Presentation.Dtos;

namespace Presentation.HttpClients.ThirdParties.Sms;

public class PayamSmsClientService : HttpClientService<PayamSmsClientService>, IPayamSmsClientService
{
    private readonly IMapper _mapper;
    private readonly ApplicationSettingExtenderModel _config;
    public PayamSmsClientService(HttpClient httpClient,
        ApplicationSettingExtenderModel configuration, IMapper mapper) : base(httpClient)
    {
        _mapper = mapper;
        _config = configuration;
    }



    public async Task<PayamSmsRs> SendPayamSms(Notif notif, CancellationToken ct = default(CancellationToken))
    {
        try
        {
            List<Message> messages = new List<Message>();
            foreach (var recip in notif.Recipients)
            {
                var message = new Message
                {
                    sender = _config.Provider.Sms.PayamSms.Sender,
                    recipient = recip.UserId,
                    body = notif.Message,
                    customerId = 1,
                };
                messages.Add(message);
                //model.messages.Add(message);
            }

            var model = new PayamSmsDto
            {
                organization = _config.Provider.Sms.PayamSms.organization,
                username = _config.Provider.Sms.PayamSms.username,
                password = _config.Provider.Sms.PayamSms.password,
                method = "send",
                messages = messages
            };

            var queryBuilder = new StringBuilder();

            queryBuilder.Append($"organization={_config.Provider.Sms.PayamSms.organization}");
            queryBuilder.Append($"username={_config.Provider.Sms.PayamSms.username}");
            queryBuilder.Append($"password={_config.Provider.Sms.PayamSms.password}");
            queryBuilder.Append($"method=send");
            queryBuilder.Append($"messages={messages}");

            //_httpClient.DefaultRequestHeaders.Clear();
            //_httpClient.DefaultRequestHeaders.Remove("X-CLIENT-TOKEN");
            //_httpClient.DefaultRequestHeaders.Add("x-mock-response-code", "200");
            //_httpClient.DefaultRequestHeaders.Add("x-mock-match-request-headers", "true");

            var uri1 = new Uri(_config.Provider.Sms.PayamSms.Url);
            var response = await _httpClient.PostAsJsonAsync(uri1.ToString(), model, ct).ConfigureAwait(false);

            var uri2 = new Uri(_httpClient.BaseAddress, $"?{queryBuilder.ToString()}");
            var result = await _httpClient.GetFromJsonAsync<PayamSmsRs>(uri2, ct);

            _logger.LogInformation("payam sms provider", queryBuilder.ToString());
            _logger.LogInformation("payam sms provider", model);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }

}
