using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Ofl.Core.Serialization.Newtonsoft.Json
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2014-04-07</created>
    /// <summary>Formats a <see cref="DateTime"/> instance with
    /// a particular format.</summary>
    ///
    //////////////////////////////////////////////////
    // TODO: Write tests.
    public class FormattedDateTimeJsonConverter : JsonConverter
    {
        #region Constructor

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2014-04-07</created>
        /// <summary>Creates a new instance of the
        /// <see cref="FormattedDateTimeJsonConverter"/>
        /// class.</summary>
        /// <param name="format">The format for the
        /// <see cref="DateTime"/>.</param>
        ///
        //////////////////////////////////////////////////
        public FormattedDateTimeJsonConverter(string format)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException(nameof(format));

            // Assign values.
            Format = format;
        }

        #endregion

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2014-04-07</created>
        /// <summary>Gets the format that is used to convert
        /// <see cref="DateTime"/> instances.</summary>
        /// <value>The format that is used to convert
        /// <see cref="DateTime"/> instances.</value>
        ///
        //////////////////////////////////////////////////
        public string Format { get; }

        #region Overrides of JsonConverter

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2014-04-07</created>
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
            var dateTime = (DateTime?) value;

            // If the value is null, then write null.
            if (dateTime == null)
            {
                // Write null.
                writer.WriteNull();
            }
            else
            {
                // Format the value.
                writer.WriteValue(dateTime.Value.ToString(Format, CultureInfo.InvariantCulture));
            }
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2014-04-07</created>
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

            // Parse.
            return DateTime.ParseExact(Convert.ToString(value, CultureInfo.InvariantCulture),
                Format, CultureInfo.InvariantCulture);
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2014-04-07</created>
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
            return objectType == typeof(DateTime?) || objectType == typeof(DateTime);
        }

        #endregion
    }
}
