namespace Application.Models.Responses;

public record ProviderRs
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string JsonConfig { get; set; }
    public ProviderType Type { get; set; }
}
