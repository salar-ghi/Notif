namespace Infrastructure.Services;

public class InMemoryCacheRepository : ICacheMessage
{
    private readonly IMemoryCache _cache;

    public InMemoryCacheRepository(IMemoryCache cache) => _cache = cache;


    public void AddMessage(string? InputKey, Notif message, TimeSpan? slidingExpiration = null)
    {
        string key = InputKey ?? Guid.NewGuid().ToString(); // Generate unique key
        _cache.Set(key, message, slidingExpiration ?? TimeSpan.FromMinutes(15));
    }

    public void AddMessage(IDictionary<string, Notif> messages, TimeSpan? slidingExpiration = null)
    {
        foreach (var item in messages)
        {
            AddMessage(item.Key, item.Value, slidingExpiration);
        }
    }

    public void AddMessage(IEnumerable<Notif> messages, TimeSpan? slidingExpiration = null)
    {
        string key = Guid.NewGuid().ToString(); // Generate unique key
        _cache.Set(key, messages.ToList(), slidingExpiration ?? TimeSpan.FromMinutes(15));
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

    public Task<IEnumerable<Notif>> GetAllMessages()
    {
        var currentDateTime = DateTime.Now.ToString("HH:mm:ss");
        //string formattedDateTime = currentDateTime.ToString("HH:mm:ss")
        var messages = _cache.Get<IEnumerable<Notif>>("__AllKeys__") ?? Enumerable.Empty<Notif>();
        Console.WriteLine("Doing GetAllMessages background job at {0}", currentDateTime);
        return Task.FromResult(messages);
    }
}
