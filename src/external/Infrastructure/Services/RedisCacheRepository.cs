using StackExchange.Redis;

namespace Infrastructure.Services;

public class RedisCacheRepository //: ICacheMessage
{
    ////private readonly IDatabase _redis; // Inject connection to Redis database
    //ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("localhost");
    ////private readonly IDistributedCache _cache;

    //public RedisCacheRepository()
    //{
        
    //}
    ////public RedisCacheRepository(IDistributedCache cache)
    ////{
    ////    var _cache = cache;
    ////}

    //public async Task AddMessage(Notif message)
    //{
    //    string key = Guid.NewGuid().ToString(); // Generate unique key
    //    //await _cache.SetAsync(key, JsonSerializer.Serialize(message)); // Use JSON serialization
    //    //await _redis.StringSetAsync(key, JsonSerializer.Serialize(message)); // Use JSON serialization
    //}

    //public async Task<IEnumerable<Notif>> GetMessages()
    //{
    //    var keys = await _redis.StringGetAsync(null); // Get all keys
    //    var messages = new List<Notif>();
    //    foreach (var key in keys)
    //    {
    //        var messageString = await _redis.StringGetAsync(key);
    //        messages.Add(JsonSerializer.Deserialize<Notif>(messageString)); // Deserialize JSON
    //    }
    //    return messages;
    //}    ////private readonly IDatabase _redis; // Inject connection to Redis database
    //ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("localhost");
    ////private readonly IDistributedCache _cache;

    //public RedisCacheRepository()
    //{
        
    //}
    ////public RedisCacheRepository(IDistributedCache cache)
    ////{
    ////    var _cache = cache;
    ////}

    //public async Task AddMessage(Notif message)
    //{
    //    string key = Guid.NewGuid().ToString(); // Generate unique key
    //    //await _cache.SetAsync(key, JsonSerializer.Serialize(message)); // Use JSON serialization
    //    //await _redis.StringSetAsync(key, JsonSerializer.Serialize(message)); // Use JSON serialization
    //}

    //public async Task<IEnumerable<Notif>> GetMessages()
    //{
    //    var keys = await _redis.StringGetAsync(null); // Get all keys
    //    var messages = new List<Notif>();
    //    foreach (var key in keys)
    //    {
    //        var messageString = await _redis.StringGetAsync(key);
    //        messages.Add(JsonSerializer.Deserialize<Notif>(messageString)); // Deserialize JSON
    //    }
    //    return messages;
    //}
}
