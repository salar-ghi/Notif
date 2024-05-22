using Application.Models;
namespace Infrastructure.Services;

public class InMemoryCacheRepository : ICacheMessage
{
    private readonly IMemoryCache _cache;
    private readonly string _cacheKey = "messages_cache";
    public InMemoryCacheRepository(IMemoryCache cache) => _cache = cache;


    public async Task<bool> AddMessage(string? InputKey, NotifVM message, TimeSpan? slidingExpiration = null)
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

    public async Task<bool> AddMessage(IDictionary<string, NotifVM> messages, TimeSpan? slidingExpiration = null)
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

    public async Task<bool> AddMessage(IEnumerable<NotifVM> messages)
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



    public async Task<NotifVM> GetMessages()
    {
        var message = _cache.Get<NotifVM>("__Allkeys__");
        return message;
    }


    public async Task<IEnumerable<KeyValuePair<string, NotifVM>>> GetKeyValueMessages()
    {
        var messages = _cache.Get<IEnumerable<KeyValuePair<string, NotifVM>>>("__AllKeys__") ?? Enumerable.Empty<KeyValuePair<string, NotifVM>>();
        return messages;
    }

     
    public async Task<IEnumerable<NotifVM>> GetAllMessages()
    {
        var cache = _cache.Get<IEnumerable<NotifVM>>(_cacheKey) ?? Enumerable.Empty<NotifVM>();
        
        //???????????????????
        //var tttttr = _cache.Get<IEnumerable<object>>(_cacheKey).AsParallel();

        return cache;
    }

    public Task RemoveMessage(NotifVM entity)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveMessage(ICollection<NotifVM> entity)
    {
        _cache.Remove(_cacheKey);
        //throw new NotImplementedException();
    }
}
