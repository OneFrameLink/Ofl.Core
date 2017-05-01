using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ofl.Core.Serialization.Newtonsoft.Json;
using Xunit;

namespace Ofl.Core.Tests.Serialization.Newtonsoft.Json
{
    public class UnixTimestampDateTimeOffsetJsonConverterTests
    {
        private class TestClass
        {
            [JsonConverter(typeof(UnixTimestampDateTimeOffsetJsonConverter))]
            public DateTimeOffset? Value { get; set; }
        }

        [Theory]
        [InlineData("{\"value\":1493391600 }", "2017-04-28T15:00:00.0000000+00:00")]
        public void Test_ReadJson(string json, string dateTimeOffsetString)
        {
            // Validate parameters.
            if (json == null) throw new ArgumentNullException(nameof(json));

            // Get the date time offset.
            DateTimeOffset? expected = DateTimeOffset.Parse(dateTimeOffsetString);

            // Parse/etc.
            JObject obj = JObject.Parse(json);

            // Map to an object.
            TestClass testClass = obj.ToObject<TestClass>();

            // Equal.
            Assert.Equal(expected, testClass.Value);
        }
    }
}
