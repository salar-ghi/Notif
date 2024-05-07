
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;

namespace Infrastructure.Servicesl;

public class RedisCacheRepository : ICacheMessage
{
    private readonly IDatabase _redis; // Inject connection to Redis database

    public RedisCacheRepository(IDatabase redis)
    {
        _redis = redis;
    }

    public async Task AddMessage(Notif message)
    {
        string key = Guid.NewGuid().ToString(); // Generate unique key
        await _redis.StringSetAsync(key, JsonSerializer.Serialize(message)); // Use JSON serialization
    }

    public async Task<IEnumerable<Notif>> GetMessages()
    {
        var keys = await _redis.StringGetAsync(null); // Get all keys
        var messages = new List<Notif>();
        foreach (var key in keys)
        {
            var messageString = await _redis.StringGetAsync(key);
            messages.Add(JsonSerializer.Deserialize<Notif>(messageString)); // Deserialize JSON
        }
        return messages;
    }
}
