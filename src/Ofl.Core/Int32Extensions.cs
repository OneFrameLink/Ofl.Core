using System;
using System.Globalization;

namespace Ofl.Core
{
    public static class Int32Extensions
    {
        public static int? TryParse(string s, NumberStyles style, IFormatProvider provider)
        {
            // The value.
            int value;

            // Try and parse, if not successful, return null, otherwise, value.
            if (int.TryParse(s, style, provider, out value)) return value;

            // Return null.
            return null;
        }

        public static int? TryParse(string s)
        {
            // The value.
            int value;

            // Try and parse, if not successful, return null, otherwise, value.
            if (int.TryParse(s, out value)) return value;

            // Return null.
            return null;
        }
    }
}
