using System.Net.Mail;

namespace Presentation.Extensions;

public static class FluentEmailExtensions
{
    public static void AddFluentEmail(this IServiceCollection services, IConfiguration config)
    {
        var emailSettings = config.GetSection("Provider").GetSection("Email");
        var defaultFromEmail = emailSettings["DefaultFromEmail"];
        //var host = emailSettings["SMTPSetting:Host"];
        var host = emailSettings.GetSection("SMTPSetting")["Host"];
        var port = emailSettings.GetSection("SMTPSetting").GetValue<int>("Port");
        var userName = emailSettings["SMTPSetting:UserName"];
        var password = emailSettings["SMTPSetting:Password"];

        //var client = new SmtpClient(host, port)
        //{
        //    EnableSsl = true,
        //    Credentials = new NetworkCredential(userName, password)
        //};
        //client.SendMailAsync(
        //    new MailMessage(from:defaultFromEmail,
            
        //    ))

        services.AddFluentEmail(defaultFromEmail)
            //.addRazorRenderer()
            .AddSmtpSender(host, port, userName, password);
    }
}
