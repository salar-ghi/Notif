using Application.Models.Responses;

namespace Application.Services.Abstractions;

public interface ICacheMessage
{
    //void AddMessage(string? InputKey, Notif message, TimeSpan? slidingExpiration = null);
    Task<bool> AddMessage(string? InputKey, NotifRq message, TimeSpan? slidingExpiration = null);
    Task<bool> AddMessage(IDictionary<string, NotifRq> messages, TimeSpan? slidingExpiration =null);
    Task<bool> AddMessage(IEnumerable<NotifRq> messages);


    Task<Notif> GetMessages();
    Task<IEnumerable<KeyValuePair<string, Notif>>> GetKeyValueMessages();
    Task<IEnumerable<NotifRs>> GetAllMessages();

}
