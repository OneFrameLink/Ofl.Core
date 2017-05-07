using System;
using NodaTime;
using Xunit;

namespace Ofl.Core.Tests
{
    public static class DateTimeOffsetExtensionsTests
    {
        [Theory]
        [InlineData("2017-05-06 13:00:00+04:00", "Asia/Seoul", "09:00")]
        [InlineData("2017-05-06 22:30:00+04:00", "Asia/Seoul", "09:00")]
        public static void Test_ReplaceOffsetFromDateTimeZone(string valueString, string dateTimeZoneId, string offsetValue)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(valueString)) throw new ArgumentNullException(nameof(valueString));
            if (string.IsNullOrWhiteSpace(dateTimeZoneId)) throw new ArgumentNullException(nameof(dateTimeZoneId));
            if (string.IsNullOrWhiteSpace(offsetValue)) throw new ArgumentNullException(nameof(offsetValue));

            // Get the value.
            DateTimeOffset value = DateTimeOffset.Parse(valueString);

            // Parse the offset.
            TimeSpan offset = TimeSpan.Parse(offsetValue);

            // Get the date time zone.
            DateTimeZone dateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(dateTimeZoneId);

            // Convert.
            DateTimeOffset actual = value.ReplaceOffsetFromDateTimeZone(dateTimeZone);

            // Compare components.
            Assert.Equal(actual.Year, value.Year);
            Assert.Equal(actual.Month, value.Month);
            Assert.Equal(actual.Day, value.Day);
            Assert.Equal(actual.Hour, value.Hour);
            Assert.Equal(actual.Minute, value.Minute);
            Assert.Equal(actual.Second, value.Second);
            Assert.Equal(actual.Millisecond, value.Millisecond);

            // Compare offset.
            Assert.Equal(actual.Offset, offset);
        }
    }
}
