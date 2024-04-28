namespace Domain.Entities;

public class Recipient()
{
    public long Id { get; set; }
    public string UserId { get; set; }
    public string? Device { get; set; }

    //public string? mobile { get; set; }


    public virtual ICollection<Notif> Notifs { get; set; } //???

}
