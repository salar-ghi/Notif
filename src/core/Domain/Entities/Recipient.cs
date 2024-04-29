namespace Domain.Entities;

public class Recipient()
{
    public string UserId { get; set; } = default!;
    //public string? Device { get; set; }

    public virtual ICollection<Notif> Notifs { get; set; } //???

}
