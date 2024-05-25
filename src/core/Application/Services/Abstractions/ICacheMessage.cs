namespace Application.Services.Abstractions;

public interface ICacheMessage
{
    Task<bool> AddMessage(string? InputKey, MessageVM message, TimeSpan? slidingExpiration = null);
    Task<bool> AddMessage(IDictionary<string, MessageVM> messages, TimeSpan? slidingExpiration =null);
    Task<bool> AddMessage(IEnumerable<MessageVM> messages);


    Task<MessageVM> GetMessages();
    Task<IEnumerable<KeyValuePair<string, MessageVM>>> GetKeyValueMessages();
    Task<IEnumerable<MessageVM>> GetAllMessages();


    Task RemoveMessage(MessageVM entity);
    Task RemoveMessage(ICollection<MessageVM> entity);
}
