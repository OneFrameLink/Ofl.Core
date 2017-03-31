using System;
using Newtonsoft.Json;

namespace Ofl.Core.Serialization.Newtonsoft.Json
{
    public abstract class ReadOnlyJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;

        public override bool CanRead => true;
    }
}
