namespace Domain.Entities;

public abstract class Provider
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsEnabled { get; set; }

    //public NotifType Type { get; set; } // e.g., SMS, Email, RabbitMQ

    //.... Another Properties and Configurations
    public List<ProviderConfiguration> config { get; set; } = new List<ProviderConfiguration>();

    public abstract Task<bool> SendNotifAsync(Notif notif);

}
