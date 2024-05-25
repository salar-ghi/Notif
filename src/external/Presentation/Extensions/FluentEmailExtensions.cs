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

        var client = new SmtpClient(host, port)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(userName, password),
            Host = host,
            Port = 587,
            DeliveryMethod = SmtpDeliveryMethod.Network,
        };

        //using (SmtpClient client = new SmtpClient())
        //{
        //    client.EnableSsl = true;
        //    client.UseDefaultCredentials = false;
        //    client.Credentials = new NetworkCredential(userName, password);
        //    client.Host = host;
        //    client.Port = 587;
        //    client.DeliveryMethod = SmtpDeliveryMethod.Network;

        //    client.SendAsync()
        //}


        services.AddFluentEmail(defaultFromEmail)
            .AddSmtpSender(client);
        //.AddSmtpSender(host, port, userName, password);
    }
}
