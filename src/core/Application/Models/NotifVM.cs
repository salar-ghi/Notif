namespace Application.Models;

public record NotifVM
{
    [Required]
    public string Title { get; set; } = default!;
    
    [Required]
    public string Body { get; set; } = default!;
    public NotifType Type { get; set; }

    [Required]
    public int SenderId { get; set; }
    public DateTime SendDate { get; set; }
    public long CreatedById { get; set; }
    public int Provider { get; set; }

    
    public DateTime CreateDate { get; init; } = DateTime.UtcNow;

    public ICollection<RecipientVM> Recipients { get; set; } = new List<RecipientVM>();
}


public record RecipientVM
{
    public string UserId { get; set; }
    public string nameofDevice { get; set; }
}
