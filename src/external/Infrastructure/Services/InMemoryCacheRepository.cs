namespace Infrastructure.Services;

public class InMemoryCacheRepository : ICacheMessage
{
    private readonly IMemoryCache _cache;
    private readonly string _cacheKey = "messages_cache";
    public InMemoryCacheRepository(IMemoryCache cache) => _cache = cache;


    public async Task<bool> AddMessage(string? InputKey, NotifRq message, TimeSpan? slidingExpiration = null)
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

    public async Task<bool> AddMessage(IDictionary<string, NotifRq> messages, TimeSpan? slidingExpiration = null)
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

    public async Task<bool> AddMessage(IEnumerable<NotifRq> messages)
    {
        try
        {
            string key = Guid.NewGuid().ToString(); // Generate unique key
            _cache.Set(_cacheKey, messages.ToList());
            return true;
        }
        catch
        {
            return false;
        }
    }



    public Task<Notif> GetMessages()
    {
        var message = _cache.Get<Notif>("__Allkeys__");
        return Task.FromResult(message);
    }


    public Task<IEnumerable<KeyValuePair<string, Notif>>> GetKeyValueMessages()
    {
        var messages = _cache.Get<IEnumerable<KeyValuePair<string, Notif>>>("__AllKeys__") ?? Enumerable.Empty<KeyValuePair<string, Notif>>();
        return Task.FromResult(messages);
    }

    public async Task<IEnumerable<NotifRs>> GetAllMessages()
    {
        var ded = _cache.GetType().GetProperty("key");
        var ttte = _cache.Get(_cacheKey);
        var tttttr = _cache.Get<IEnumerable<object>>(_cacheKey);

        var tets = _cache.GetOrCreate(_cacheKey, entry =>
        {
            //entry.SlidingExpiration = TimeSpan.FromMinutes(30);
            return new List<string>();
        });
        var ttt = _cache.TryGetValue(_cacheKey, out var calue);

        var messages = _cache.GetOrCreate(_cacheKey, entry =>
        {
            //entry.SlidingExpiration = TimeSpan.FromMinutes(30);
            return new List<NotifRs>();
        });
        

        //var messages = _cache.Get<IEnumerable<NotifRs>>("__AllKeys__") ?? Enumerable.Empty<NotifRs>();
        
        //var currentDateTime = DateTime.Now.ToString("HH:mm:ss");
        //Console.WriteLine("Doing GetAllMessages background job at {0}", currentDateTime);

        return messages;
    }
}
