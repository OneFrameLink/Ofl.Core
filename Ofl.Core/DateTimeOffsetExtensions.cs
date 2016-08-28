using System;
using System.Globalization;

namespace Ofl.Core
{
    //////////////////////////////////////////////////
    /// 
    /// <author>Nicholas Paldino</author>
    /// <created>2013-10-22</created>
    /// <summary>Utilities for the <see cref="DateTimeOffset"/>
    /// structure.</summary>
    /// 
    //////////////////////////////////////////////////
    public static class DateTimeOffsetExtensions
    {
        #region Read-only state.

        #region To Delete when moving to .NET 4.6, as DateTimeOffset remomves need for this.

        // Number of days in a non-leap year
        private const int DaysPerYear = 365;
        // Number of days in 4 years
        private const int DaysPer4Years = DaysPerYear * 4 + 1;       // 1461
        // Number of days in 100 years
        private const int DaysPer100Years = DaysPer4Years * 25 - 1;  // 36524
        // Number of days in 400 years
        private const int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097

        // The number of unix epoch ticks there are.
        private const int DaysTo1970 = DaysPer400Years * 4 + DaysPer100Years * 3 + DaysPer4Years * 17 + DaysPerYear; // 719,162

        // Number of days from 1/1/0001 to 12/31/9999
        private const int DaysTo10000 = DaysPer400Years * 25 - 366;  // 3652059

        private const long UnixEpochTicks = TimeSpan.TicksPerDay * DaysTo1970;

        // Number of 100ns ticks per time unit
        private const long TicksPerMillisecond = 10000;
        private const long TicksPerSecond = TicksPerMillisecond * 1000;
        private const long TicksPerMinute = TicksPerSecond * 60;
        private const long TicksPerHour = TicksPerMinute * 60;
        private const long TicksPerDay = TicksPerHour * 24;
        private const long MinTicks = 0;
        private const long MaxTicks = DaysTo10000 * TicksPerDay - 1;

        private const long UnixEpochSeconds = UnixEpochTicks / TimeSpan.TicksPerSecond; // 62,135,596,800        
        private const long UnixMinSeconds = MinTicks / TimeSpan.TicksPerSecond - UnixEpochSeconds;
        private const long UnixMaxSeconds = MaxTicks / TimeSpan.TicksPerSecond - UnixEpochSeconds;

        #endregion

        /// <summary>The Unix epoch time.</summary>
        public static readonly DateTimeOffset UnixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        #endregion

        #region Extension methods.

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2013-10-22</created>
        /// <summary>Converts a <see cref="DateTimeOffset"/>
        /// to a Unix timestamp.</summary>
        /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/>
        /// that is to be converted.</param>
        /// <returns>The Unix timestamp.</returns>
        /// 
        //////////////////////////////////////////////////
        // TODO: Replace with DateTimeOffset.ToUnixTimeSeconds in .NET 4.6
        public static long ToUnixTimeSeconds(this DateTimeOffset dateTimeOffset)
        {
            // Truncate sub-second precision before offsetting by the Unix Epoch to avoid
            // the last digit being off by one for dates that result in negative Unix times.
            //
            // For example, consider the DateTimeOffset 12/31/1969 12:59:59.001 +0
            //   ticks            = 621355967990010000
            //   ticksFromEpoch   = ticks - UnixEpochTicks                   = -9990000
            //   secondsFromEpoch = ticksFromEpoch / TimeSpan.TicksPerSecond = 0
            //
            // Notice that secondsFromEpoch is rounded *up* by the truncation induced by integer division,
            // whereas we actually always want to round *down* when converting to Unix time. This happens
            // automatically for positive Unix time values. Now the example becomes:
            //   seconds          = ticks / TimeSpan.TicksPerSecond = 62135596799
            //   secondsFromEpoch = seconds - UnixEpochSeconds      = -1
            //
            // In other words, we want to consistently round toward the time 1/1/0001 00:00:00,
            // rather than toward the Unix Epoch (1/1/1970 00:00:00).
            long seconds = dateTimeOffset.UtcDateTime.Ticks / TimeSpan.TicksPerSecond;
            return seconds - UnixEpochSeconds;
        }

        // TODO: Replace with DateTimeOffset.FromUnixTimeSeconds in .NET 4.6
        public static DateTimeOffset FromUnixTimeSeconds(long seconds)
        {
            // Validate parameters.
            if (seconds < UnixMinSeconds || seconds > UnixMaxSeconds)
                throw new ArgumentOutOfRangeException(nameof(seconds), seconds,
                    string.Format(CultureInfo.CurrentCulture, "The seconds parameter must be a value between {0} and {1}.",
                        UnixMinSeconds, UnixMaxSeconds));

            // Calculate.
            long ticks = seconds * TimeSpan.TicksPerSecond + UnixEpochTicks;
            return new DateTimeOffset(ticks, TimeSpan.Zero);
        }


        #endregion
    }
}
