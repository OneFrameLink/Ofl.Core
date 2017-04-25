using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ofl.Core.Serialization.Newtonsoft.Json
{
    public class ExceptionJsonConverter : JsonConverter
    {
        #region Overrides

        public override bool CanRead => false;

        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Validate parameters.
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            // Get the exception.
            var e = (Exception) value;

            // Get the exception token.
            JToken token = e.GetExceptionShimJToken(serializer);

            // Write to the writer.
            token.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            throw new NotImplementedException();

        public override bool CanConvert(Type objectType)
        {
            // Validate parameters.
            if (objectType == null) throw new ArgumentNullException(nameof(objectType));

            // The type info.
            TypeInfo typeInfo = objectType.GetTypeInfo();

            // If the type info is assignable to any of the can convert type infos then
            // return that.
            return ExceptionExtensions.ExceptionTypeInfo.IsAssignableFrom(typeInfo);
        }

        #endregion
    }
}
