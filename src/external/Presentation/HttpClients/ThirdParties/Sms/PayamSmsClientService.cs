namespace Presentation.HttpClients.ThirdParties.Sms;

public class PayamSmsClientService : HttpClientService<PayamSmsClientService>, IPayamSmsClientService
{
    private readonly ApplicationSettingExtenderModel _config;
    //private readonly IElasticsearchService _elasticsearchService;
    public PayamSmsClientService(HttpClient httpClient, ApplicationSettingExtenderModel configuration) : base(httpClient)
    {
        _config = configuration;
        //_elasticsearchService = elasticsearchService;
    }



    public async Task<bool> SendPayamSms(Message notif, CancellationToken ct = default(CancellationToken))
    {
        try
        {
            List<MessageDto> messages = new List<MessageDto>();
            foreach (var recip in notif.Recipients)
            {
                var message = new MessageDto
                {
                    sender = _config.Provider.Sms.PayamSms.Sender,
                    recipient = recip.Destination,
                    body = notif.Body,
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

            //var queryBuilder = new StringBuilder();
            //foreach ( var recip in notif.Recipients)
            //{
            //    queryBuilder.Append($"organization={_config.Provider.Sms.PayamSms.organization}&");
            //    queryBuilder.Append($"username={_config.Provider.Sms.PayamSms.username}&");
            //    queryBuilder.Append($"password={_config.Provider.Sms.PayamSms.password}&");
            //    queryBuilder.Append($"method=send&");
            //    queryBuilder.Append($"sender={_config.Provider.Sms.PayamSms.Sender}&recipient={recip.UserId}&body={notif.Message}&customerId={1}");
            //    queryBuilder.Append(Environment.NewLine);
            //}

            //_httpClient.DefaultRequestHeaders.Clear();
            //_httpClient.DefaultRequestHeaders.Remove("X-CLIENT-TOKEN");
            //_httpClient.DefaultRequestHeaders.Add("x-mock-response-code", "200");
            //_httpClient.DefaultRequestHeaders.Add("x-mock-match-request-headers", "true");

            //await _elasticsearchService.SaveLogAsync(model);
            var uri = new Uri(_config.Provider.Sms.PayamSms.Url);
            var response = await _httpClient.PostAsJsonAsync(uri.ToString(), model, ct).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK)
            {

            }
            var resLogData = new
            {
                ResponseHeader = response.Headers,
                ResponseContent = await response.Content.ReadAsStringAsync(),
                ResponseStatusCode = response.StatusCode,
                ResponseReasonPhrase = response.ReasonPhrase,
            };
            //await _elasticsearchService.SaveLogAsync(resLogData);
            _logger.LogInformation("HttpClient Response logged: {0}", resLogData);

            //var uri2 = new Uri(_httpClient.BaseAddress, $"?{queryBuilder.ToString()}");
            //var result = await _httpClient.GetFromJsonAsync<PayamSmsRs>(uri2, ct);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return false;
        }
    }

}
