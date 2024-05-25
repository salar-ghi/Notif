using Application.Models;
namespace Infrastructure.Services;

public class InMemoryCacheRepository : ICacheMessage
{
    private readonly IMemoryCache _cache;
    private readonly string _cacheKey = "messages_cache";
    public InMemoryCacheRepository(IMemoryCache cache) => _cache = cache;


    public async Task<bool> AddMessage(string? InputKey, MessageVM message, TimeSpan? slidingExpiration = null)
    {
        try
        {
            string key = InputKey ?? Guid.NewGuid().ToString(); // Generate unique key
            //_cache.Set(key, message, slidingExpiration ?? TimeSpan.FromMinutes(15));
            _cache.Set(key, message);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AddMessage(IDictionary<string, MessageVM> messages, TimeSpan? slidingExpiration = null)
    {
        try
        {
            foreach (var item in messages)
            {
                await AddMessage(item.Key, item.Value, slidingExpiration);
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public async Task<bool> AddMessage(IEnumerable<MessageVM> messages)
    {
        try
        {
            //string key = Guid.NewGuid().ToString(); // Generate unique key
            _cache.Set(_cacheKey, messages.ToList());
            return true;
        }
        catch
        {
            return false;
        }
    }



    public async Task<MessageVM> GetMessages()
    {
        var message = _cache.Get<MessageVM>("__Allkeys__");
        return message;
    }


    public async Task<IEnumerable<KeyValuePair<string, MessageVM>>> GetKeyValueMessages()
    {
        var messages = _cache.Get<IEnumerable<KeyValuePair<string, MessageVM>>>("__AllKeys__") ?? Enumerable.Empty<KeyValuePair<string, MessageVM>>();
        return messages;
    }

     
    public async Task<IEnumerable<MessageVM>> GetAllMessages()
    {
        var cache = _cache.Get<IEnumerable<MessageVM>>(_cacheKey) ?? Enumerable.Empty<MessageVM>();

        return cache;
    }

    public Task RemoveMessage(MessageVM entity)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveMessage(ICollection<MessageVM> entity)
    {
        _cache.Remove(_cacheKey);
        //throw new NotImplementedException();
    }
}
