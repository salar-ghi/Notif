namespace Domain.Entities;

public class MessageLog : EntityBase
{
    public int Id { get; set; }

    public long MessageId { get; set; }
    public virtual Message Message { get; set; }
    
    public DateTime? SentAt { get; set; }
    public bool Success { get; set; }
    //public string ErrorMessage { get; set; }
    public string? FailureReason { get; set; }

    public int ProviderId { get; set; }
    public virtual Provider Provider { get; set; }


    //...?? provider
}
