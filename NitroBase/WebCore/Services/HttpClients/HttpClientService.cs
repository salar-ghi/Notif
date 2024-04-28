using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Core;
using Core.Extensions;
using WebCore.Helpers;
using Core.Abstractions;
using Microsoft.Extensions.Options;
using WebCore.Extensions;
using Core.Services;

namespace WebCore.Services.HttpClients
{
    public abstract class HttpClientService<T> : IHttpClientService
    {
        protected readonly HttpClient _httpClient;
        protected readonly ILogger<T> _logger;
        protected readonly IGuard _guard;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _logger = ServiceLocator.GetService<ILogger<T>>();
            _guard = ServiceLocator.GetService<IGuard>();
        }

        public async Task<TResult> GetAsync<TResult>(string url, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default) where TResult : class
        {
            SetHeaderParams(requestHeader);

            var result = await SendAsync(url, HttpMethod.Get, cancellationToken);

            return string.IsNullOrWhiteSpace(result) ? null : result.FromJsonString<TResult>();
        }

        public Task DeleteAsync(string url, object body = null, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default)
        {
            SetHeaderParams(requestHeader);

            return SendAsync(url, HttpMethod.Delete, cancellationToken);
        }

        public async Task<TResult> DeleteAsync<TResult>(string url, object body = null, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default) where TResult : class
        {
            SetHeaderParams(requestHeader);

            string result;

            if (body is not null)
                result = await SendAsync(url, body, HttpMethod.Delete, cancellationToken);
            else
                result = await SendAsync(url, HttpMethod.Delete, cancellationToken);

            return string.IsNullOrWhiteSpace(result) ? null : result.FromJsonString<TResult>();
        }

        //public async Task<TResult> PostAsync<TResult>(string url, HttpContent content = null, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default) where TResult : class
        //{
        //    SetHeaderParams(requestHeader);

        //    var result = await SendAsync(url, content, HttpMethod.Post, cancellationToken: cancellationToken);

        //    return string.IsNullOrWhiteSpace(result) ? null : result.FromJsonString<TResult>();
        //}

        public async Task<TResult> PostAsync<TValue, TResult>(string url, TValue content, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default) where TResult : class
        {
            ArgumentNullException.ThrowIfNull(nameof(content));

            SetHeaderParams(requestHeader);

            var result = await SendAsync(url, content, HttpMethod.Post, cancellationToken: cancellationToken);

            return string.IsNullOrWhiteSpace(result) ? null : result.FromJsonString<TResult>();
        }

        public async Task PutAsync(string url, object body = null, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default)
        {
            SetHeaderParams(requestHeader);

            await SendAsync(url, body, HttpMethod.Put, cancellationToken);
        }

        public async Task<TResult> PutAsync<TResult>(string url, object body, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default) where TResult : class
        {
            SetHeaderParams(requestHeader);

            var result = await SendAsync(url, body, HttpMethod.Put, cancellationToken);

            return string.IsNullOrWhiteSpace(result) ? null : result.FromJsonString<TResult>();
        }

        public async Task PatchAsync(string url, object body = null, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default)
        {
            SetHeaderParams(requestHeader);

            await SendAsync(url, body, HttpMethod.Patch, cancellationToken);
        }

        public async Task<TResult> PatchAsync<TResult>(string url, object body, Dictionary<string, string> requestHeader = null, CancellationToken cancellationToken = default) where TResult : class
        {
            SetHeaderParams(requestHeader);

            var result = await SendAsync(url, body, HttpMethod.Patch, cancellationToken);

            return string.IsNullOrWhiteSpace(result) ? null : result.FromJsonString<TResult>();
        }

        private void SetHeaderParams(Dictionary<string, string> requestHeader)
        {
            _httpClient.DefaultRequestHeaders.Clear();

            if (requestHeader is null) return;

            foreach (var (key, value) in requestHeader)
                _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        private async Task<string> SendAsync(string url, HttpMethod httpMethod, CancellationToken cancellationToken) =>
            await SendAsync(url, null, httpMethod, cancellationToken);

        private async Task<string> SendAsync(string url, object content, HttpMethod httpMethod, CancellationToken cancellationToken)
        {
            var jsonContent = content is null ? null : content.ToHttpContent();

            return await SendAsync(url, jsonContent, httpMethod, cancellationToken);
        }

        private async Task<string> SendAsync(string url, HttpContent content, HttpMethod httpMethod, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage()
            {
                Content = content,
                Method = httpMethod,
                RequestUri = new Uri(url)
            };

            var result = await _httpClient.SendAsync(request, cancellationToken);

            if (result.IsSuccessStatusCode is false)
                throw new Exception(APIResultHelper.CreateErrorMessage(url, result.StatusCode));

            return await result.Content.ReadAsStringAsync(cancellationToken);
        }

        private HttpRequestMessage GetHttpRequestMessage(HttpMethod httpMethod, Uri uri)
        {
            return new HttpRequestMessage(httpMethod, uri)
            {
                Content = null,
                Version = _httpClient.DefaultRequestVersion,
                VersionPolicy = _httpClient.DefaultVersionPolicy
            };
        }
    }
}
