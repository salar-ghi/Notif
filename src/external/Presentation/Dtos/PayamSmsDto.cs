namespace Presentation.Dtos;

public record PayamSmsDto
{
    public string Url { get; set; }
    public string organization { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string method { get; set; }
    public ICollection<Message> messages { get; set; }
}

public record Message
{
    public string sender { get; set; }
    public string recipient { get; set; }
    public string body { get; set; }
    public int customerId { get; set; }
}

