namespace Application.Models;

public record ApplicationSettingExtenderModel : ApplicationSettingModel
{
    public string BaseUrl { get; init; }
    public string Username { get; init; }
}
