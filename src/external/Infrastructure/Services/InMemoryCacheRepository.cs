namespace Infrastructure.Services;

public class InMemoryCacheRepository : ICacheMessage
{
    private readonly IMemoryCache _cache;

    public InMemoryCacheRepository(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task AddMessage(Notif message)
    {
        string key = Guid.NewGuid().ToString(); // Generate unique key
        _cache.Set(key, message);
    }

    public async Task<IEnumerable<Notif>> GetMessages()
    {
        var messages = _cache.TryGetValue().OfType<Notif>();
        return messages;
    }
}
