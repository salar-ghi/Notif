using FluentEmail.Smtp; 
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using FluentEmail.Core.Models;

namespace Infrastructure.Services;

public class EmailService : IEmailProvider
{
    #region Definition & CTor

    private readonly IFluentEmail _fluentEmail;
    private readonly ILogger<EmailService> _logger;
    private readonly IMapper _mapper;
    private readonly ApplicationSettingExtenderModel _settingModel;
    private readonly SmtpSettings _smtpSettings;

    public EmailService(ILogger<EmailService> logger, IMapper mapper,  
        ApplicationSettingExtenderModel settingModel, IOptions<SmtpSettings> smtpSettings, IFluentEmail fluentEmail)
    {
        _logger = logger;
        _mapper = mapper;
        _settingModel = settingModel;
        _smtpSettings = smtpSettings.Value;
        _fluentEmail = fluentEmail;
    }

    #endregion

    #region Methods

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var sender = new SmtpSender(() => new SmtpClient(_settingModel.Provider.Email.SMTP.Host)
        {
            //Port = _smtpSettings.Port,
            Port = _settingModel.Provider.Email.SMTP.Port,
            Credentials = new NetworkCredential(
                _settingModel.Provider.Email.SMTP.Username,
            _settingModel.Provider.Email.SMTP.Password),
            EnableSsl = _settingModel.Provider.Email.SMTP.EnableSsl
        });

        Email.DefaultSender = sender;
        var email = await Email
            .From(_smtpSettings.Username)
            .To(to)
            .Subject(subject)
            .Body(body)
            .SendAsync();
    }

    public async Task<bool> SendAsync(string ProviderName, Message message)
    {
        try
        {
            foreach (var recipient in message.Recipients)
            {
                //var sender = new SmtpSender(() => new SmtpClient("mail.nitrogenco.com")
                //{
                //    //Port = _settingModel.Provider.Email.SMTP.Port,
                //    //Credentials = new NetworkCredential(
                //    //    _settingModel.Provider.Email.SMTP.Username,
                //    //    _settingModel.Provider.Email.SMTP.Password),
                //    //EnableSsl = _settingModel.Provider.Email.SMTP.EnableSsl

                //    Port = 465,
                //    Credentials = new NetworkCredential("s.ghahremani@nitrogenco.com", "Nitro@123456"),
                //    EnableSsl = false
                //});
                //Email.DefaultSender = sender;
                //var email = await Email
                //    .From("s.ghahremani@nitrogenco.com")
                //    .To(recipient.Destination)
                //    .Subject(message.Title)
                //    .Body(message.Body)
                //    .SendAsync();
                //if (!email.Successful)
                //{
                //    Console.WriteLine(email.ErrorMessages);
                //}

                var item = await _fluentEmail
                   .To(recipient.Destination)
                   .Subject(message.Title)
                   .Body(message.Body)
                   .SendAsync();
                if (!item.Successful)
                {
                    Console.WriteLine(item.ErrorMessages);
                }
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
