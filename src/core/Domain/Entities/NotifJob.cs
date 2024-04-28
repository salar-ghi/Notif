namespace Domain.Entities;

public class NotifJob
{
    public int Id { get; set; }
    public string HangfireJobId { get; set; }
    
    
    //public Notif Notif { get; set; }
    //public int NotifId { get; set; }
    public bool IsSent { get; set; }
    public DateTime NextTry { get; set; }
}
