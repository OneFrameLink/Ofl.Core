using System;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace Ofl.Core.Serialization.Newtonsoft.Json
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
            // To camel case.
            return name.ToCamelCase(_ignoredPrefixCharacters, base.ResolvePropertyName);
        }

        #endregion

    }
}
