using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Ofl.Core.Serialization.Newtonsoft.Json
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2013-09-07</created>
    /// <summary>Handles the conversion of a Unix 
    /// timestamp to a <see cref="DateTimeOffset"/>.</summary>
    ///
    //////////////////////////////////////////////////
    public class UnixTimestampDateTimeOffsetJsonConverter : JsonConverter
    {
        #region Overrides of JsonConverter

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-09-07</created>
        /// <summary>Reads the JSON representation of the object.</summary>
        /// <remarks>This is not implemented, as this is
        /// read-only.</remarks>
        /// <param name="writer">The <see cref="JsonWriter"/> to read from.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="serializer">The calling serializer.</param>
        ///
        //////////////////////////////////////////////////
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Validate parameters.
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            // Get the date time offset.
            var dateTimeOffset = (DateTimeOffset?)value;

            // If the value is null, then write null.
            if (dateTimeOffset == null)
                // Write null.
                writer.WriteNull();
            else
                // Write the value.
                writer.WriteValue(dateTimeOffset.Value.ToUnixTimeSeconds());
        }

        public override bool CanWrite => true;

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-09-07</created>
        /// <summary>Reads the JSON representation of the object.</summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        ///
        //////////////////////////////////////////////////
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Validate parameters.
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (objectType == null) throw new ArgumentNullException(nameof(objectType));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            // Read as a string or boolean.
            object value = reader.Value;

            // If the value is null, return null.
            if (value == null) return null;

            // The value is convertable to a long.
            var seconds = Convert.ToInt64(value, CultureInfo.InvariantCulture);

            // Return the date time offset.
            DateTimeOffset returnValue = DateTimeOffset.FromUnixTimeSeconds(seconds);

            // If the type is nullable, return nullable.
            if (objectType == typeof (DateTimeOffset?)) return (DateTimeOffset?) returnValue;

            // Return the return value.
            return returnValue;
        }

        public override bool CanRead => true;

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-09-07</created>
        /// <summary>Determines whether this instance can convert the specified object type.</summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        ///
        //////////////////////////////////////////////////
        public override bool CanConvert(Type objectType)
        {
            // Validate parameters.
            if (objectType == null) throw new ArgumentNullException(nameof(objectType));

            // Is it a timespan, or a nullable timespan?
            return objectType == typeof(DateTimeOffset?) || objectType == typeof(DateTimeOffset);
        }

        #endregion
    }
}
