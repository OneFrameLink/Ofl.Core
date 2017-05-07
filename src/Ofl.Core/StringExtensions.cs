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

        public static string ToCamelCase(this string value)
        {
            // Call the overload.
            return value.ToCamelCase("");
        }

        public static string ToCamelCase(this string value, Func<string, string> camelCaseResolver)
        {
            // Call the overload.
            return value.ToCamelCase("", camelCaseResolver);
        }

        public static string ToCamelCase(this string value, IEnumerable<char> ignoredPrefixCharacters)
        {
            // Call the overload with the default.
            return value.ToCamelCase(ignoredPrefixCharacters as ISet<char> ?? new HashSet<char>(ignoredPrefixCharacters));
        }

        public static string ToCamelCase(this string value, ISet<char> ignoredPrefixCharacters)
        {
            // Call the overload with the default.
            return value.ToCamelCase(ignoredPrefixCharacters, DefaultCamelCaseResolver);
        }

        public static string ToCamelCase(this string value, IEnumerable<char> ignoredPrefixCharacters,
            Func<string, string> camelCaseResolver)
        {
            // Call the overload.
            return value.ToCamelCase(ignoredPrefixCharacters as ISet<char> ?? new HashSet<char>(ignoredPrefixCharacters),
                camelCaseResolver);
        }

        public static string ToCamelCase(this string value, ISet<char> ignoredPrefixCharacters,
            Func<string, string> camelCaseResolver)
        {
            // Validate parameters.
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (ignoredPrefixCharacters == null) throw new ArgumentNullException(nameof(ignoredPrefixCharacters));
            if (camelCaseResolver == null) throw new ArgumentNullException(nameof(camelCaseResolver));

            // Validate parameters.
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            // The index.
            int index = 0;

            // While the first character is in the prefixed characters.
            do
            {
                // If the character is not in the prefix characters, then break.
                if (!ignoredPrefixCharacters.Contains(value[index]))
                    break;
            } while (++index < value.Length);

            // If the index is 0, return the resolved property name.
            // No prefixed characters.
            if (index == 0) return camelCaseResolver(value);

            // If the index is equal to the length, then
            // just return the name.  All characters
            // are in the ignored prefix.
            if (index == value.Length) return value;

            // The string builder.
            var builder = new StringBuilder(value.Length);

            // Append the prefix.
            builder.Append(value.Substring(0, index));

            // Resolve the property name.
            string propertyName = camelCaseResolver(value.Substring(index));

            // Attach to the buffer and return.
            return builder.Append(propertyName).ToString();
        }

        private static string DefaultCamelCaseResolver(string value)
        {
            // Validate parameters.
            if (value == null) throw new ArgumentNullException(nameof(value));

            // If the length is zero, return empty.
            if (value.Length == 0) return value;

            // Camel case.
            char firstCharacter = char.ToLowerInvariant(value[0]);

            // If there is one character, return.
            if (value.Length == 1) return firstCharacter.ToString();

            // Append the rest.
            return firstCharacter + value.Substring(1);
        }

        public static string CoalesceNullOrWhitespace(params string[] strings)
        {
            // Validate parameters.
            if (strings == null) throw new ArgumentNullException(nameof(strings));

            // Call the overload.
            return strings.CoalesceNullOrWhitespace();
        }

        public static string CoalesceNullOrWhitespace(this IEnumerable<string> strings)
        {
            // Validate parameters.
            if (strings == null) throw new ArgumentNullException(nameof(strings));

            // Return the first non null or non whitespace string.
            return strings.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
        }
    }
}
