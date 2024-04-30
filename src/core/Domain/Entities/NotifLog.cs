namespace Domain.Entities;

public class NotifLog : EntityBase
{
    public int Id { get; set; }
    public int NotifId { get; set; }
    public virtual Notif Notif { get; set; }
    
    public DateTime SentAt { get; set; }
    public bool Success { get; set; }
    //public string ErrorMessage { get; set; }
    public string FailureReason { get; set; }



    //...?? provider
}
