//using System;
//using Newtonsoft.Json.Serialization;

//namespace Ofl.Serialization.Json.Newtonsoft
//{
//    //////////////////////////////////////////////////
//    ///
//    /// <author>Nicholas Paldino</author>
//    /// <created>2013-01-13</created>
//    /// <summary>A <see cref="IContractResolver"/> implementation
//    /// that uses case-insensitive property names.</summary>
//    ///
//    //////////////////////////////////////////////////
//    public class CaseInsensitiveContractResolver : DefaultContractResolver
//    {
//        #region Overrides

//        //////////////////////////////////////////////////
//        ///
//        /// <author>Nicholas Paldino</author>
//        /// <created>2013-01-13</created>
//        /// <summary>Resolves a property name.</summary>
//        /// <param name="propertyName">The name of the property
//        /// to resolve.</param>
//        /// <returns>The resolved property name.</returns>
//        ///
//        //////////////////////////////////////////////////
//        protected override string ResolvePropertyName(string propertyName)
//        {
//            // Validate parameters.
//            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

//            // Convert to lower case invariant.
//            return propertyName.ToUpperInvariant();
//        }

//        #endregion
//    }
//}
