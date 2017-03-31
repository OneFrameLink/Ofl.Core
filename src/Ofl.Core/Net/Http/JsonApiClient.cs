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

        protected JsonApiClient(IHttpClientFactory httpClientFactory) : 
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

        protected override async Task<HttpResponseMessage> ProcessHttpResponseMessageAsync(HttpResponseMessage httpResponseMessage, 
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));

            // Get the serializer settings.
            JsonSerializerSettings settings = await CreateJsonSerializerSettingsAsync(cancellationToken).ConfigureAwait(false);

            // Call the overload.
            return await ProcessHttpResponseMessageAsync(httpResponseMessage, settings, cancellationToken).
                ConfigureAwait(false);
        }

        protected virtual Task<HttpResponseMessage> ProcessHttpResponseMessageAsync(HttpResponseMessage httpResponseMessage, 
            JsonSerializerSettings jsonSerializerSettings, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));
            if (jsonSerializerSettings == null) throw new ArgumentNullException(nameof(jsonSerializerSettings));

            // Ensure the status code.
            httpResponseMessage.EnsureSuccessStatusCode();

            // Return the response.
            return Task.FromResult(httpResponseMessage);
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
            // Get the response.
            using (HttpResponseMessage response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false))
                // Process the response.
                return await ProcessResponseAsync<T>(response, settings, cancellationToken).ConfigureAwait(false);
        }

        protected override async Task PostAsync<TRequest>(string url, TRequest request,
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
            // Get the response.
            using (HttpResponseMessage response = await client.PostJsonForHttpResponseMessageAsync(
                url, settings, request, cancellationToken).ConfigureAwait(false))
                // Process the response.
                await ProcessResponseAsync(response, settings, cancellationToken).ConfigureAwait(false);
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
            // Get the response.
            using (HttpResponseMessage response = await client.PostJsonForHttpResponseMessageAsync(
                url, settings, request, cancellationToken).ConfigureAwait(false))
                // Process the response.
                return await ProcessResponseAsync<TResponse>(response, settings, cancellationToken).ConfigureAwait(false);
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
            // Get the response.
            using (HttpResponseMessage response = await client.DeleteAsync(url, cancellationToken).ConfigureAwait(false))
                // Process the response.
                return await ProcessResponseAsync<TResponse>(response, settings, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Helpers

        private async Task ProcessResponseAsync(HttpResponseMessage httpResponseMessage,
            JsonSerializerSettings jsonSerializerSettings, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));
            if (jsonSerializerSettings == null) throw new ArgumentNullException(nameof(jsonSerializerSettings));

            // Process the response.
            using (await ProcessHttpResponseMessageAsync(httpResponseMessage, jsonSerializerSettings, cancellationToken).
                ConfigureAwait(false))
            // Do nothing.
            { }
        }

        private async Task<TResponse> ProcessResponseAsync<TResponse>(HttpResponseMessage httpResponseMessage,
            JsonSerializerSettings jsonSerializerSettings, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));
            if (jsonSerializerSettings == null) throw new ArgumentNullException(nameof(jsonSerializerSettings));

            // Process the response.
            using (HttpResponseMessage response = await ProcessHttpResponseMessageAsync(httpResponseMessage, jsonSerializerSettings, cancellationToken).
                ConfigureAwait(false))
                // Deserialize.
                return await response.ToObjectAsync<TResponse>(jsonSerializerSettings, cancellationToken).ConfigureAwait(false);
        }

        #endregion
    }
}
