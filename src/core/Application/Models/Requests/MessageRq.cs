﻿namespace Application.Models.Requests;

public record MessageRq
{
    [Required]
    public string Title { get; set; }
    //public MessageType MessageType { get; set; }
    [Required]
    public string Body { get; set; }
    public NotifType Type { get; set; }
    [Required]
    public int SenderId { get; set; }
    public DateTime SendDate { get; set; }
    public long CreatedById { get; set; }

    public string ProviderName { get; set; }


    public ICollection<RecipientRq> Recipients { get; set; } = new List<RecipientRq>();

}
