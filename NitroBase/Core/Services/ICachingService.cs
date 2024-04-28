using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface ICachingService
    {
        Task<bool> SetAsync<T>(string key, T item);

        Task<bool> SetAsync<T>(string key, T item, TimeSpan ttl);

        Task<bool> TryGet<T>(string key, out T value);

        Task<bool> RemoveAsync(string key);

    }
}
