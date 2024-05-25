namespace Application.Models.Requests;

public record RecipientRq
{
    public long UserId { get; set; }
    public string Destination { get; set; }
    //public long NotifId { get; set; }
}
