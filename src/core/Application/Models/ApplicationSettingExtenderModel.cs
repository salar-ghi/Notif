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
    public string Host { get; init; }
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


public record ElasticsearchSettings
{
    public string Url { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public string DefaultIndex { get; init; }
}