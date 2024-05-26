namespace Application.Models;

public record ApplicationSettingExtenderModel : ApplicationSettingModel
{
    public string BaseUrl { get; init; }
    public string Username { get; init; }
    public BackgroundJobSettingModel Jobs { get; init; }
    public ProviderSettingModel Provider { get; init; }
}

public record BackgroundJobSettingModel
{
    public int Attemp { get; init; }
}


public record ProviderSettingModel
{
    public SmsProviderConfiguration Sms { get; init; }
    public EmailProviderConfiguration Email { get; set; }
}

public record EmailProviderConfiguration
{
    public string DefaultFromEmail { get; init; }
    public SmtpSettings SMTP { get; set; }
    public Pop3Settings POP3 { get; set; }
}

public record Pop3Settings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public record SmtpSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool EnableSsl { get; set; }
}

public record SmsProviderConfiguration
{
    public MelipayamakSettingModel Melipayamak { get; init; }
    public PayamSmsSettingModel PayamSms { get; set; }
}

public record MelipayamakSettingModel
{
    public string Url { get; init; }
}

public record PayamSmsSettingModel
{
    public string Url { get; init; }
    public string organization { get; init; }
    public string username { get; init; }
    public string password { get; init; }
    public string method { get; set; }
    public string Sender { get; set; }
}