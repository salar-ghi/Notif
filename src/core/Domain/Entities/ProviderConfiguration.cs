namespace Domain.Entities;

public class ProviderConfiguration
{
    public int Id { get; set; }
    public string Key { get; set; } = default!;
    public string Value { get; set; } = default!;

    public Provider Provider { get; set; } = default!;
    public int ProviderId { get; set; }

}
