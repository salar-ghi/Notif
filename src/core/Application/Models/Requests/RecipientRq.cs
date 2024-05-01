namespace Application.Models.Requests;

public record RecipientRq
{
    public string UserId { get; set; }
    public string nameofDevice { get; set; }
    //public long NotifId { get; set; }
}
