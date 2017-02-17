using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ofl.Core.Net.Http
{
    public abstract class JsonApiClient : ApiClient
    {
        #region Constructor

        protected JsonApiClient(Func<HttpMessageHandler, bool, CancellationToken, Task<HttpClient>> httpClientFactory) : 
            base(httpClientFactory)
        { }

        #endregion

        #region Overrides.

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings {
            // Camel case.
            ContractResolver = new CamelCasePropertyNamesContractResolver(),

            // Don't send null values.
            NullValueHandling = NullValueHandling.Ignore
        };

        protected virtual Task<JsonSerializerSettings> CreateJsonSerializerSettingsAsync(CancellationToken cancellationToken)
        {
            // Camel cased by default.
            return Task.FromResult(JsonSerializerSettings);
        }

        protected override async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            // Format the url.
            url = await FormatUrlAsync(url, cancellationToken).ConfigureAwait(false);

            // Create the JsonSerializer.
            var settings = await CreateJsonSerializerSettingsAsync(cancellationToken).ConfigureAwait(false);

            // Create the HttpClient.
            using (var client = await CreateHttpClientAsync(cancellationToken).ConfigureAwait(false))
                // Get JSON now.
                return await client.GetJsonAsync<T>(url, settings, cancellationToken).
                    ConfigureAwait(false);
        }

        protected override async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request,
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Format the url.
            url = await FormatUrlAsync(url, cancellationToken).ConfigureAwait(false);

            // Create the serializer.
            var settings = await CreateJsonSerializerSettingsAsync(cancellationToken).ConfigureAwait(false);

            // Create the HttpClient.
            using (var client = await CreateHttpClientAsync(cancellationToken).ConfigureAwait(false))
                // Get JSON now.
                return await client.PostJsonAsync<TRequest, TResponse>(url, settings, request, cancellationToken).
                    ConfigureAwait(false);
        }

        protected override async Task<TResponse> DeleteAsync<TResponse>(string url, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            // Format the url.
            url = await FormatUrlAsync(url, cancellationToken).ConfigureAwait(false);

            // Create the serializer.
            var settings = await CreateJsonSerializerSettingsAsync(cancellationToken).ConfigureAwait(false);

            // Create the HttpClient.
            using (var client = await CreateHttpClientAsync(cancellationToken).ConfigureAwait(false))
                // Get JSON now.
                return await client.DeleteJsonAsync<TResponse>(url, settings, cancellationToken).
                    ConfigureAwait(false);
        }

        #endregion
    }
}
