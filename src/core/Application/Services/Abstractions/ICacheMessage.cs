namespace Application.Services.Abstractions;

public interface ICacheMessage
{
    Task<bool> AddMessage(string? InputKey, NotifVM message, TimeSpan? slidingExpiration = null);
    Task<bool> AddMessage(IDictionary<string, NotifVM> messages, TimeSpan? slidingExpiration =null);
    Task<bool> AddMessage(IEnumerable<NotifVM> messages);


    Task<NotifVM> GetMessages();
    Task<IEnumerable<KeyValuePair<string, NotifVM>>> GetKeyValueMessages();
    Task<IEnumerable<NotifVM>> GetAllMessages();


    Task RemoveMessage(NotifVM entity);
    Task RemoveMessage(ICollection<NotifVM> entity);
}
