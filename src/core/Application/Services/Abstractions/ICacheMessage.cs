namespace Application.Services.Abstractions;

public interface ICacheMessage
{
    Task AddMessage(Notif message);
    Task<IEnumerable<Notif>> GetMessages();
}
