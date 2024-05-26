namespace Domain.Entities;

public class Provider : EntityBase , IValidate, ITrackable
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsEnabled { get; set; }
    public string JsonConfig { get; set; } = default!;
    public ProviderType Type { get; set; }
    public byte Priority { get; set; }
    //public NotifType Type { get; set; } // e.g., SMS, Email, RabbitMQ
    public virtual ICollection<MessageLog> MessageLog { get; set; }

    public DateTime CreatedAt { get; set; }
    public long CreatedById { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public long? ModifiedById { get; set; }

    public void Validate()
    {
        throw new NotImplementedException();
    }


}
