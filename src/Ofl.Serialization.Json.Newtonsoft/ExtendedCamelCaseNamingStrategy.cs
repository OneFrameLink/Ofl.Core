using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace Ofl.Serialization.Json.Newtonsoft
{
    public class ExtendedCamelCaseNamingStrategy : CamelCaseNamingStrategy
    {
        #region Constructor

        public ExtendedCamelCaseNamingStrategy(IEnumerable<char> ignoredPrefixCharacters)
        {
            // Validate parameters.
            if (ignoredPrefixCharacters == null) throw new ArgumentNullException(nameof(ignoredPrefixCharacters));

            // Put in the set.
            _ignoredPrefixCharacters = new HashSet<char>(ignoredPrefixCharacters);
        }

        #endregion

        #region Instance, read-only state.

        private readonly ISet<char> _ignoredPrefixCharacters;

        #endregion

        #region Overrides of CamelCaseNamingStrategy

        protected override string ResolvePropertyName(string name)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            // The index.
            int index = 0;

            // While the first character is in the prefixed characters.
            do
            {
                // If the character is not in the prefix characters, then break.
                if (!_ignoredPrefixCharacters.Contains(name[index]))
                    break;
            } while (++index < name.Length);

            // If the index is 0, return the resolved property name.
            // No prefixed characters.
            if (index == 0) return base.ResolvePropertyName(name);

            // If the index is equal to the length, then
            // just return the name.  All characters
            // are in the ignored prefix.
            if (index == name.Length) return name;

            // The string builder.
            var builder = new StringBuilder(name.Length);

            // Append the prefix.
            builder.Append(name.Substring(0, index));

            // Resolve the property name.
            string propertyName = base.ResolvePropertyName(name.Substring(index));

            // Attach to the buffer and return.
            return builder.Append(propertyName).ToString();
        }

        #endregion

    }
}
