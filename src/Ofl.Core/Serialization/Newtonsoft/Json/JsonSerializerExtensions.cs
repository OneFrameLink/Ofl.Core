using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

namespace Ofl.Core.Serialization.Newtonsoft.Json
{
    public static class JsonSerializerExtensions
    {
        public static string SerializeToString<TRequest>(this JsonSerializer serializer, TRequest request)
        {
            // Validate parameters.
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Create a StringWriter.
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            // Now a json writer.
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                // Serialize.
                serializer.Serialize(jsonWriter, request);

                // Flush the writer.
                jsonWriter.Flush();

                // Write the json.
                return stringWriter.ToString();
            }
        }
    }
}
