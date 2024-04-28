namespace Domain.Entities;

public class BlackList
{
    public int Id { get; set; }

    public string UserId { get; set; } = default!;
    public string Reason { get; set; } = default!;
}
