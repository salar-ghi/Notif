using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions
{
    public interface IHttpClientService
    {
        //Task GetAsync(string url, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default);

        Task<TResult> GetAsync<TResult>(string url, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default) where TResult : class;

        Task DeleteAsync(string url, object body = null, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default);

        Task<TResult> DeleteAsync<TResult>(string url, object body = null, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default) where TResult : class;

        //Task<TResult> PostAsync<TResult>(string url, HttpContent content = null, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default) where TResult : class;

        Task<TResult> PostAsync<TValue, TResult>(string url, TValue content, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default) where TResult : class;

        Task PutAsync(string url, object body = null, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default);

        Task<TResult> PutAsync<TResult>(string url, object body, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default) where TResult : class;

        Task PatchAsync(string url, object body = null, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default);

        Task<TResult> PatchAsync<TResult>(string url, object body, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default) where TResult : class;
    }
}
