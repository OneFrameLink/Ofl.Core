using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Core.Net.Http
{
    // TODO: Find some way to merge this with IHttpClientFactory.
    // Do not want code split.  Problems are that Ofl.Core does not have interfaces (nor is it
    // supposed to).
    public abstract class ApiClient
    {
        #region Constructor

        protected ApiClient(IHttpClientFactory httpClientFactory httpClientFactory)
        {
            // Validate parameters.
            if (httpClientFactory == null) throw new ArgumentNullException(nameof(httpClientFactory));

            // Assign values.
            _httpClientFactory = httpClientFactory;
        }

        #endregion

        #region Instance, read-only state.

        private readonly Func<HttpMessageHandler, bool, CancellationToken, Task<HttpClient>> _httpClientFactory;

        #endregion

        #region Overrides

        protected virtual Task<HttpClient> CreateHttpClientAsync(CancellationToken cancellationToken)
        {
            // Call the overload.
            return CreateHttpClientAsync(HttpMessageHandlerExtensions.CreateDefaultMessageHandler(), cancellationToken);
        }

        protected virtual Task<HttpClient> CreateHttpClientAsync(HttpMessageHandler httpMessageHandler, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpMessageHandler == null) throw new ArgumentNullException(nameof(httpMessageHandler));

            // Call the func.
            return CreateHttpClientAsync(httpMessageHandler, true, cancellationToken);
        }

        protected virtual Task<HttpClient> CreateHttpClientAsync(HttpMessageHandler httpMessageHandler, bool disposeHandler, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpMessageHandler == null) throw new ArgumentNullException(nameof(httpMessageHandler));

            // Call the func.
            return _httpClientFactory(httpMessageHandler, disposeHandler, cancellationToken);
        }

        protected virtual Task<string> FormatUrlAsync(string url, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            // Just return the URL.
            return Task.FromResult(url);
        }
        protected virtual Task<HttpResponseMessage> ProcessHttpResponseMessageAsync(
            HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));

            // Ensure success.
            httpResponseMessage.EnsureSuccessStatusCode();

            // Return the message.
            return Task.FromResult(httpResponseMessage);
        }

        protected virtual async Task GetAsync(string url, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            // Format the URL.
            url = await FormatUrlAsync(url, cancellationToken).ConfigureAwait(false);

            // Get the http client.
            using (HttpClient client = await CreateHttpClientAsync(cancellationToken).ConfigureAwait(false))
            // Get the response.
            using (HttpResponseMessage originalResponse = await client.GetAsync(url, cancellationToken).ConfigureAwait(false))
            // Process the response message.
            using (await ProcessHttpResponseMessageAsync(originalResponse, cancellationToken).ConfigureAwait(false))
            { }
        }

        protected abstract Task<T> GetAsync<T>(string url, CancellationToken cancellationToken);

        protected abstract Task<TResponse> PostAsync<TRequest, TResponse>(string url, 
            TRequest request, CancellationToken cancellationToken);

        protected abstract Task PostAsync<TRequest>(string url, 
            TRequest request, CancellationToken cancellationToken);

        protected virtual async Task DeleteAsync(string url, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            // Format the URL.
            url = await FormatUrlAsync(url, cancellationToken).ConfigureAwait(false);

            // Get the http client.
            using (HttpClient client = await CreateHttpClientAsync(cancellationToken).ConfigureAwait(false))
            // Get the response.
            using (HttpResponseMessage originalResponse = await client.DeleteAsync(url, cancellationToken).ConfigureAwait(false))
            // Process the response message.
            using (await ProcessHttpResponseMessageAsync(originalResponse, cancellationToken).ConfigureAwait(false))
            { }
        }

        protected abstract Task<TResponse> DeleteAsync<TResponse>(string url, CancellationToken cancellationToken);

        #endregion
    }
}
