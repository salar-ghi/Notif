namespace Domain.Entities;

public class Recipient : EntityBase
{
    public long Id { get; set; }
    public string UserId { get; set; } = default!;
    //public string? Device { get; set; }

    public long NotifId { get; set; }
    public virtual Notif Notif { get; set; } //???

}
