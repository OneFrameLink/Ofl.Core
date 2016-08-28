using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace Ofl.Core.Serialization.Newtonsoft.Json
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2013-01-13</created>
    /// <summary>A <see cref="IContractResolver"/> implementation
    /// that takes Pascal-cased items and translates them
    /// to lowercase with underscores in between.</summary>
    ///
    //////////////////////////////////////////////////
    public class PascalCaseToLowerCaseWithUnderscoreContractResolver : DefaultContractResolver
    {
        #region Static, read-only state.

        /// <summary>The regular expression that will be used to get capital letters.</summary>
        private static readonly Regex CapitalLettersRegex = new Regex("[A-Z]",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        #endregion

        #region Overrides

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-01-13</created>
        /// <summary>Resolves a property name.</summary>
        /// <param name="propertyName">The name of the property
        /// to resolve.</param>
        /// <returns>The resolved property name.</returns>
        ///
        //////////////////////////////////////////////////
        protected override string ResolvePropertyName(string propertyName)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            // Replace the regular expression with lowercase.
            propertyName = CapitalLettersRegex.Replace(propertyName, m => "_" + m.Value.ToLowerInvariant());

            // Remove the first underscore.
            return propertyName.Substring(1);
        }

        #endregion
    }
}
