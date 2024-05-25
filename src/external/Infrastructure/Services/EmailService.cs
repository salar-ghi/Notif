using Domain.Entities;
using FluentEmail.Core.Models;
using AutoMapper;
using System.Collections.Generic;

namespace Infrastructure.Services;

public class EmailService : IEmailProvider
{
    #region Definition & CTor
    
    private readonly IFluentEmail _fluentEmail;
    private readonly ILogger<EmailService> _logger;
    private readonly IMapper _mapper;
    public EmailService(IFluentEmail fluentEmail, ILogger<EmailService> logger, IMapper mapper)
    {
        _fluentEmail = fluentEmail;
        _logger = logger;
        _mapper = mapper;
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
                   .To(recipient.UserId)
                   .Subject(message.Title)
                   .Body(message.Message)
                   .SendAsync();
                if (!item.Successful)
                {

                }
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
