namespace Presentation.Dtos;

public record PayamSmsDto
{
    public string Url { get; init; }
    public string organization { get; init; }
    public string username { get; init; }
    public string password { get; init; }
    public string method { get; set; }
    public List<MessageDto> messages { get; set; }
}

public record MessageDto
{
    public string sender { get; set; }
    public string recipient { get; set; }
    public string body { get; set; }
    public int customerId { get; set; }
}

