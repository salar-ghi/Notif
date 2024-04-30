namespace Domain.Entities;

public class Recipient : EntityBase
{
    public string UserId { get; set; } = default!;
    //public string? Device { get; set; }

    public virtual ICollection<Notif> Notifs { get; set; } //???

}
