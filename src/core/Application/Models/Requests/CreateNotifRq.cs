namespace Application.Models.Requests;

public record CreateNotifRq
{
    [Required]
    public string Title { get; set; }
    //public MessageType MessageType { get; set; }
    [Required]
    public string Message { get; set; }
    public NotifType Type { get; set; }
    [Required]
    public int SenderId { get; set; }
    public DateTime SendDate { get; set; }
    public long CreatedById { get; set; }
    public string HangfireJobId { get; set; } = default!;


    public ICollection<RecipientRq> Recipients { get; set; } = new List<RecipientRq>();

}
