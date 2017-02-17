using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Core.IO;
using Newtonsoft.Json;

namespace Ofl.Core.Net.Http
{
    public static class HttpClientExtensions
    {
        public static Task<Stream> GetStreamAsync(this HttpClient httpClient,
            string requestUri, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(requestUri)) throw new ArgumentNullException(nameof(requestUri));

            // Call the overload, dispose of the client.
            return httpClient.GetStreamAsync(new Uri(requestUri), true, cancellationToken);
        }

        public static Task<Stream> GetStreamAsync(this HttpClient httpClient,
            Uri requestUri, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));

            // Call the overload, dispose of the client.
            return httpClient.GetStreamAsync(requestUri, true, cancellationToken);
        }

        public static Task<Stream> GetStreamAsync(this HttpClient httpClient,
            string requestUri, bool disposeHttpClient, CancellationToken cancellationToken)
        {

            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(requestUri)) throw new ArgumentNullException(nameof(requestUri));

            // Call the overload, dispose of the client.
            return httpClient.GetStreamAsync(new Uri(requestUri), disposeHttpClient, cancellationToken);
        }

        public static async Task<Stream> GetStreamAsync(this HttpClient httpClient,
            Uri requestUri, bool disposeHttpClient, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));

            // Create a disposable.
            var compositeDisposable = new CompositeDisposable();

            // The copy.
            CompositeDisposable compositeDisposableCopy = compositeDisposable;
            
            // Wrap in a try/finally.
            try
            {
                // Add the client if disposing of it.
                if (disposeHttpClient) compositeDisposable.Add(httpClient);

                // Set the response message.
                HttpResponseMessage httpResponseMessage;
                compositeDisposable.Add(httpResponseMessage = 
                    await httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false));

                // Get the stream.  Wrap.
                Stream stream;
                compositeDisposable.Add(stream =
                    await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false));

                // Set the copy to null.
                compositeDisposableCopy = null;

                // Return the stream.
                return new StreamWithState<object>(stream, compositeDisposable);
            }
            catch
            {
                // Dispose.
                using (compositeDisposableCopy)                
                    // Throw.
                    throw;
            }
        }

        public static Task<T> GetJsonAsync<T>(this HttpClient httpClient, string uri,
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));

            // Call the overload.
            return httpClient.GetJsonAsync<T>(uri, JsonSerializer.CreateDefault(), cancellationToken);
        }

        public static Task<T> GetJsonAsync<T>(this HttpClient httpClient, string uri,
            JsonSerializerSettings jsonSerializerSettings, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializerSettings == null) throw new ArgumentNullException(nameof(jsonSerializerSettings));

            // Call the overload.
            return httpClient.GetJsonAsync<T>(uri, JsonSerializer.Create(jsonSerializerSettings),
                cancellationToken);
        }

        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient,
            string uri, JsonSerializer jsonSerializer, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializer == null) throw new ArgumentNullException(nameof(jsonSerializer));

            // Get the message.
            using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false))
            // Get the stream.
            using (Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
            // Text reader on top of stream.
            using (var streamReader = new StreamReader(stream))
            // Create a JsonReader.
            using (JsonReader jsonReader = new JsonTextReader(streamReader))
                // Deserialize and return.
                return jsonSerializer.Deserialize<T>(jsonReader);
        }

        public static Task<TResult> PostJsonAsync<TRequest, TResult>(this HttpClient httpClient, string uri,
            TRequest request, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Call the overload.
            return httpClient.PostJsonAsync<TRequest, TResult>(uri, JsonSerializer.CreateDefault(), request, cancellationToken);
        }

        public static Task<TResponse> PostJsonAsync<TRequest, TResponse>(this HttpClient httpClient, string uri,
            JsonSerializerSettings jsonSerializerSettings, TRequest request, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializerSettings == null) throw new ArgumentNullException(nameof(jsonSerializerSettings));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Call the overload.
            return httpClient.PostJsonAsync<TRequest, TResponse>(uri, JsonSerializer.Create(jsonSerializerSettings),
                request, cancellationToken);
        }

        public static async Task<TResponse> PostJsonAsync<TRequest, TResponse>(this HttpClient httpClient,
            string uri, JsonSerializer jsonSerializer, TRequest request, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializer == null) throw new ArgumentNullException(nameof(jsonSerializer));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // The json.
            string json;

            // Create a StringWriter.
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            // Now a json writer.
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                // Serialize.
                jsonSerializer.Serialize(jsonWriter, request);

                // Flush the writer.
                jsonWriter.Flush();

                // Write the json.
                json = stringWriter.ToString();
            }

            // Create the content.
            using (HttpContent content = new StringContent(json, Encoding.UTF8, "application/json"))
            // Get the message.
            using (HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(uri, content, cancellationToken).ConfigureAwait(false))
            // Get the stream.
            using (Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
            // Text reader on top of stream.
            using (var streamReader = new StreamReader(stream))
            // Create a JsonReader.
            using (JsonReader jsonReader = new JsonTextReader(streamReader))
                // Deserialize and return.
                return jsonSerializer.Deserialize<TResponse>(jsonReader);
        }

        public static Task<TResult> DeleteJsonAsync<TResult>(this HttpClient httpClient, string uri,
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));

            // Call the overload.
            return httpClient.DeleteJsonAsync<TResult>(uri, JsonSerializer.CreateDefault(), cancellationToken);
        }

        public static Task<TResponse> DeleteJsonAsync<TResponse>(this HttpClient httpClient, string uri,
            JsonSerializerSettings jsonSerializerSettings, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializerSettings == null) throw new ArgumentNullException(nameof(jsonSerializerSettings));

            // Call the overload.
            return httpClient.DeleteJsonAsync<TResponse>(uri, JsonSerializer.Create(jsonSerializerSettings),
                cancellationToken);
        }

        public static async Task<TResponse> DeleteJsonAsync<TResponse>(this HttpClient httpClient,
            string uri, JsonSerializer jsonSerializer, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializer == null) throw new ArgumentNullException(nameof(jsonSerializer));

            // Get the message.
            using (HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(uri, cancellationToken).ConfigureAwait(false))
            // Get the stream.
            using (Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
            // Text reader on top of stream.
            using (var streamReader = new StreamReader(stream))
            // Create a JsonReader.
            using (JsonReader jsonReader = new JsonTextReader(streamReader))
                // Deserialize and return.
                return jsonSerializer.Deserialize<TResponse>(jsonReader);
        }
    }
}
