using System.Net.Mail;

namespace Presentation.Extensions;

public static class FluentEmailExtensions
{
    public static void AddFluentEmail(this IServiceCollection services, IConfiguration config)
    {
        var emailSettings = config.GetSection("Provider").GetSection("Email");
        var defaultFromEmail = emailSettings["DefaultFromEmail"];
        var host = emailSettings.GetSection("SMTP")["Host"];
        var port = emailSettings.GetSection("SMTP").GetValue<int>("Port");
        var userName = emailSettings["SMTP:UserName"];
        var password = emailSettings["SMTP:Password"];

        //var client = new SmtpClient()
        //{
        //    Credentials = new NetworkCredential(userName, password),
        //    Host = host,
        //    Port = port,
        //};


        services
            .AddFluentEmail(defaultFromEmail)
            .AddSmtpSender(host, port, userName, password);
    }
}
