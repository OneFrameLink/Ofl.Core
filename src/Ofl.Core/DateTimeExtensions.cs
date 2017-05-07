using System;
using NodaTime;
using NodaTime.Extensions;

namespace Ofl.Core
{
    public static class DateTimeExtensions
    {
        public static DateTimeOffset SpecifyDateTimeZone(this DateTime dateTime, DateTimeZone dateTimeZone)
        {
            // Validate parameters.
            if (dateTimeZone == null) throw new ArgumentNullException(nameof(dateTimeZone));

            // Create the instance, then apply the date time zone.
            LocalDateTime local = dateTime.ToLocalDateTime();

            // Apply the time zone now.
            return dateTimeZone.AtStrictly(local).ToDateTimeOffset();
        }
    }
}
