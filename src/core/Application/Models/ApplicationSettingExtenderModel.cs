namespace Application.Models;

public record ApplicationSettingExtenderModel : ApplicationSettingModel
{
    public string BaseUrl { get; init; }
    public string Username { get; init; }
    public BackgroundJobSettingModel Jobs { get; set; }
    public PayamSmsSettingModel payamSms { get; set; }
}

public record BackgroundJobSettingModel
{
    public int Attemp { get; set; }
}


public record PayamSmsSettingModel
{
    public string Url { get; set; }
    public string organization { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string method { get; set; }
    public string Sender { get; set; }
}
