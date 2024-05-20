namespace Application.Models;

public record ApplicationSettingExtenderModel : ApplicationSettingModel
{
    public string BaseUrl { get; init; }
    public string Username { get; init; }
    public BackgroundJobSettingModel Jobs { get; set; }
}

public record BackgroundJobSettingModel
{
    public int Attemp { get; set; }
}
