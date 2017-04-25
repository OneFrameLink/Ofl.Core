using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Ofl.Core.Linq;
using Xunit;

namespace Ofl.Core.Tests.Serialization.Newtonsoft.Json
{
    internal static class AssertExtensions
    {
        public static void AssertExceptionsEqual(this IContractResolver contractResolver,
            IEnumerable<Exception> expected, IEnumerable<JToken> actual)
        {
            // Validate parameters.
            if (contractResolver == null) throw new ArgumentNullException(nameof(contractResolver));
            if (expected == null) throw new ArgumentNullException(nameof(expected));
            if (actual == null) throw new ArgumentNullException(nameof(actual));

            // Cycle through the exceptions, compare to actual.  Zip.
            foreach (var pair in expected.ZipChecked(actual, (f, s) => new {Exception = f, JToken = s}))
                // Assert.
                contractResolver.AssertExceptionsEqual(pair.Exception, pair.JToken);
        }

        public static void AssertExceptionsEqual(this IContractResolver contractResolver,
            Exception expected, JToken actual)
        {
            // Validate parameters.
            if (contractResolver == null) throw new ArgumentNullException(nameof(contractResolver));
            if (expected == null) throw new ArgumentNullException(nameof(expected));
            if (actual == null) throw new ArgumentNullException(nameof(actual));

            // Cast, if not a DefaultContractResolver, throw.
            var defaultContractResolver = (DefaultContractResolver) contractResolver;

            // The getters.
            JToken GetToken(JToken token, string key) => token[defaultContractResolver.GetResolvedPropertyName(key)];
            object GetTokenValue(JToken token, string key) => (GetToken(token, key) as JValue).Value;
            object GetActualValue(string key) => GetTokenValue(actual, key);

            // Start comparing.
            Assert.Equal(expected.HResult, GetActualValue(nameof(expected.HResult)));
            Assert.Equal(expected.HelpLink, GetActualValue(nameof(expected.HelpLink)));
            Assert.Equal(expected.Source, GetActualValue(nameof(expected.Source)));
            Assert.Equal(expected.StackTrace, GetActualValue(nameof(expected.StackTrace)));
            Assert.Equal(expected.Message, GetActualValue(nameof(expected.Message)));

            // Look for meta.
            // TODO: Expose this better.
            string metaKey = "$" + defaultContractResolver.GetResolvedPropertyName("Meta");

            // Get the meta token.
            JToken meta = GetToken(actual, metaKey);

            // Get type token and value.
            JToken type = GetToken(meta, "Type");
            object GetTypeValue(string key) => GetTokenValue(type, key);

            // Get the values.
            Assert.Equal(expected.GetType().Namespace, GetTypeValue("Namespace"));
            Assert.Equal(expected.GetType().FullName, GetTypeValue("FullName"));
            Assert.Equal(expected.GetType().AssemblyQualifiedName, GetTypeValue("AssemblyQualifiedName"));
            Assert.Equal(expected.GetType().Name, GetTypeValue("Name"));
        }
    }
}
