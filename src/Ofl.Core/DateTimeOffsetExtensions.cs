using System;
using NodaTime;

namespace Ofl.Core
{
    public static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset ReplaceOffsetFromDateTimeZone(this DateTimeOffset dateTimeOffset, DateTimeZone dateTimeZone) =>
            dateTimeOffset.DateTime.ApplyDateTimeZone(dateTimeZone);
    }
}
