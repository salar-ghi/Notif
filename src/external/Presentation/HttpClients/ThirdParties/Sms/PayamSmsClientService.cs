using Application.Models.Responses.ThirdParties;
using Domain.Entities;
using Microsoft.AspNetCore.Http.Extensions;
using Presentation.Dtos;
using System.Text;

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



    public async Task<PayamSmsRs> SendPayamSms(Notif message, CancellationToken ct = default(CancellationToken))
    {
        try
        {
            //var result  = new PayamSmsRs();

            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"organization={_config.payamSms.organization}");

            var model = new PayamSmsDto()
            {
                organization = _config.payamSms.organization,
                username = _config.payamSms.username,
                password = _config.payamSms.password,
                method = "send",               
               
                
            };
            foreach ( var recipient in message.Recipients )
            {
                model.messages = message.Message;
                model.messages


            }
            

            var uri1 = new Uri(_config.payamSms.Url);
            var uri2 = new Uri(_config.payamSms.Url + $"?{queryBuilder.ToString()}");
            var response = await _httpClient.PostAsJsonAsync(uri1.ToString(), model,ct);



            var result = await _httpClient.GetFromJsonAsync<PayamSmsRs>(uri2,ct);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }

}
