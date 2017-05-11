using System;
using System.Collections.Generic;
using NodaTime;
using Ofl.Core.Linq;
using Xunit;

namespace Ofl.Core.Tests
{
    public static class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData("2017-05-06 13:00:00", "Asia/Seoul", "09:00")]
        [InlineData("2017-05-06 22:30:00", "Asia/Seoul", "09:00")]
        public static void Test_ApplyDateTimeZone(string valueString, string dateTimeZoneId, string offsetValue)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(valueString)) throw new ArgumentNullException(nameof(valueString));
            if (string.IsNullOrWhiteSpace(dateTimeZoneId)) throw new ArgumentNullException(nameof(dateTimeZoneId));
            if (string.IsNullOrWhiteSpace(offsetValue)) throw new ArgumentNullException(nameof(offsetValue));

            // Get the values.
            IReadOnlyCollection<DateTimeKind> kinds = EnumExtensions.GetEnumValues<DateTimeKind>()
                .ToReadOnlyCollection();

            // Convert the value.
            DateTime originalValue = DateTime.Parse(valueString);

            // Parse the offset.
            TimeSpan offset = TimeSpan.Parse(offsetValue);

            // Get the date time zone.
            DateTimeZone dateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(dateTimeZoneId);

            // Cycle through the kinds.
            foreach (DateTimeKind kind in kinds)
            {
                // Specify the kind of the date time offset.
                DateTime value = DateTime.SpecifyKind(originalValue, kind);

                // Make sure value and original value are all the same.
                Assert.Equal(value.Ticks, originalValue.Ticks);

                // Convert.
                DateTimeOffset actual = value.ApplyDateTimeZone(dateTimeZone);

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
}
