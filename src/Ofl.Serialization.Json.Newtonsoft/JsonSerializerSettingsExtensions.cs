using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ofl.Serialization.Json.Newtonsoft
{
    public static class JsonSerializerSettingsExtensions
    {
        public static JsonSerializer ToSerializer(this JsonSerializerSettings jsonSerializerSettings)
        {
            // Validate parameters.
            if (jsonSerializerSettings == null) throw new ArgumentNullException(nameof(jsonSerializerSettings));

            // Create the serializer.
            return JsonSerializer.Create(jsonSerializerSettings);
        }

        public static Task<MemoryStream> SerializeToMemoryStreamAsync<TRequest>(this JsonSerializerSettings jsonSerializerSettings, TRequest request,
            CancellationToken cancellationToken) => jsonSerializerSettings.ToSerializer().SerializeToMemoryStreamAsync(request, cancellationToken);

        public static string SerializeToString<TRequest>(this JsonSerializerSettings jsonSerializerSettings, TRequest request) =>
            jsonSerializerSettings.ToSerializer().SerializeToString(request);
    }
}
