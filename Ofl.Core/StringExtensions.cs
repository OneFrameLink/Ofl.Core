using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Ofl.Core
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2012-12-01</created>
    /// <summary>Extensions for the <see cref="String"/>
    /// class in .NET.</summary>
    ///
    //////////////////////////////////////////////////
    public static class StringExtensions
    {
        #region Camel-case

        // NOTE: Check to see if globalization has been updated as per
        // https://github.com/dotnet/corefx/issues/10062

        //public static string ToCamelCase(this string value)
        //{
        //    // Call the overload.
        //    return value.ToCamelCase(CultureInfo.CurrentCulture);
        //}


        // NOTE: Check to see if globalization has been updated as per
        // https://github.com/dotnet/corefx/issues/10062

        //public static string ToCamelCaseInvariant(this string value)
        //{
        //    // Call the overload.
        //    return value.ToCamelCase(CultureInfo.InvariantCulture);
        //}

        // NOTE: Check to see if globalization has been updated as per
        // https://github.com/dotnet/corefx/issues/10062
        //public static string ToCamelCase(this string value, CultureInfo culture)
        //{
        //    // Validate parameters.
        //    if (value == null) throw new ArgumentNullException(nameof(value));
        //    if (culture == null) throw new ArgumentNullException(nameof(culture));

        //    // If no length, return empty.
        //    if (value.Length == 0) return value;

        //    // If one character, return that.
        //    if (value.Length == 1) return value.ToLower(culture);

        //    // Return the rest.
        //    return Char.ToLower(value[0], culture) + value.Substring(1);
        //}

        #endregion

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-12-01</created>
        /// <summary>Returns a string that consists of a sequence of
        /// <paramref name="strings"/> with a <paramref name="delimiter"/>
        /// inserted in between elements.</summary>
        /// <param name="strings">The sequence of strings to concatenate.</param>
        /// <param name="delimiter">The delimiter to place between the strings.</param>
        /// <returns>The delimited string.  There is no delimiter at the end.</returns>
        ///
        //////////////////////////////////////////////////
        public static string ToDelimitedString(this IEnumerable<string> strings, string delimiter)
        {
            // Validate parameters.
            if (strings == null) throw new ArgumentNullException(nameof(strings));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));

            // The string builder.
            var builder = new StringBuilder();

            // Create a copy.
            var builderCopy = builder;

            // Get the last or default item.
            builder = strings.
                Select(s => builderCopy.AppendFormat(CultureInfo.CurrentCulture, "{0}{1}", s, delimiter)).
                LastOrDefault();

            // If the builder is null or the length is 0, return an empty string.
            if (builder == null || builder.Length == 0) return string.Empty;

            // Trim the end.
            builder.Length -= delimiter.Length;

            // Return the string.
            return builder.ToString();
        }

        public static IEnumerable<string> SplitOnNewLine(this string value)
        {
            // Validate parameters.
            if (value == null) throw new ArgumentNullException(nameof(value));

            // Pass the implementation.
            return value.SplitOnNewLineImplementation();
        }

        private static IEnumerable<string> SplitOnNewLineImplementation(this string value)
        {
            // Validate parameters.
            Debug.Assert(value != null);

            // The line.
            string line;

            // Create a string reader.
            using (var reader = new StringReader(value))
            // While there is a line.
            while ((line = reader.ReadLine()) != null)
            {
                // Yield the line.
                yield return line;
            }
        }
    }
}
