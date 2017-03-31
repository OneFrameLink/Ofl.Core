using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Ofl.Core
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2013-10-19</created>
    /// <summary>Extensions for the timespan structure.</summary>
    ///
    //////////////////////////////////////////////////
    public static class TimeSpanExtensions
    {
        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-10-19</created>
        /// <summary>Converts a timespan to
        /// a string with days, minutes, and hours.</summary>
        /// <remarks>This string is in English, of the format
        /// "1 day, 10 hours, 3 minutes".</remarks>
        /// <param name="timeSpan">The <see cref="TimeSpan"/>
        /// to convert to a string.</param>
        /// <returns>The <paramref name="timeSpan"/>
        /// converted to a string.</returns>
        ///
        //////////////////////////////////////////////////
        public static string ToEnglishDaysMinutesHoursString(this TimeSpan timeSpan)
        {
            // The string builder.
            var builder = new StringBuilder();

            // Days?
            if (timeSpan.Days > 0)
            {
                // Append.
                builder.AppendFormat(CultureInfo.CurrentCulture, "{0} day{1}, ",
                    timeSpan.ToString("%d", CultureInfo.CurrentCulture), timeSpan.Days == 1 ? "" : "s");
            }

            // Hours.
            if (timeSpan.Hours > 0)
            {
                // Append.
                builder.AppendFormat(CultureInfo.CurrentCulture, "{0} hour{1}, ",
                    timeSpan.ToString("%h", CultureInfo.CurrentCulture), timeSpan.Hours == 1 ? "" : "s");
            }

            // Minutes.
            if (timeSpan.Minutes > 0)
            {
                // Append.
                builder.AppendFormat(CultureInfo.CurrentCulture, "{0} minute{1}, ",
                    timeSpan.ToString("%m", CultureInfo.CurrentCulture), timeSpan.Minutes == 1 ? "" : "s");
            }

            // If the total seconds is less than 60 (a minute), then include the
            // seconds.
            if (timeSpan.TotalSeconds < 60)
            {
                // The builder has nothing in it.
                Debug.Assert(builder.Length == 0);

                // Append.
                builder.AppendFormat(CultureInfo.CurrentCulture, "{0} second{1}, ",
                    timeSpan.ToString("%s", CultureInfo.CurrentCulture), timeSpan.Seconds == 1 ? "" : "s");
            }

            // If there is length, remove the last two characters.
            if (builder.Length > 0) builder.Length -= 2;

            // Return the string.
            return builder.ToString();
        }
    }
}
