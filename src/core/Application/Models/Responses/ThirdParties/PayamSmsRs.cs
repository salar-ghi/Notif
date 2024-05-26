namespace Application.Models.Responses.ThirdParties;

public record PayamSmsRs
{
    public int code { get; set; }
    public string message { get; set; }
    public List<PayamSmsPerMessageRs> data { get; set; }
}


public record PayamSmsPerMessageRs
{
    public string serverId { get; set; }
    public string customerId { get; set; }
    public string Mobile { get; set; }
}
