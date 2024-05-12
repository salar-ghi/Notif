namespace Domain.Abstractions;

public interface ITrackable
{
    public DateTime CreatedAt { get; set; }
    public long CreatedById { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public long? ModifiedById { get; set; }
}
