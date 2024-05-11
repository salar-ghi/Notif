namespace Application.Services.Abstractions;

public interface ICacheMessage
{
    void AddMessage(string? InputKey, Notif message, TimeSpan? slidingExpiration = null);
    void AddMessage(IDictionary<string, Notif> messages, TimeSpan? slidingExpiration = null);
    void AddMessage(IEnumerable<Notif> messages, TimeSpan? slidingExpiration = null);


    Task<Notif> GetMessages();
    Task<IEnumerable<KeyValuePair<string, Notif>>> GetKeyValueMessages();
    Task<IEnumerable<Notif>> GetAllMessages();

}
