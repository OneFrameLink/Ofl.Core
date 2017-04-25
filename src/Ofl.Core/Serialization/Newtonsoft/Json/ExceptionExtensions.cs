using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Ofl.Core.Reflection;
using Ofl.Core.Serialization.Newtonsoft.Json.Shims;

namespace Ofl.Core.Serialization.Newtonsoft.Json
{
    internal static class ExceptionExtensions
    {
        internal static readonly TypeInfo EnumerableExceptionTypeInfo = typeof(IEnumerable<Exception>).GetTypeInfo();

        internal static readonly TypeInfo ExceptionTypeInfo = typeof(Exception).GetTypeInfo();

        internal static JToken GetExceptionShimJToken(this Exception value, JsonSerializer serializer)
        {
            // Validate parameters.
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            // Create the shim.
            ExceptionShim shim = value.ToExceptionShim();

            // If the shim is null, be done with it.
            if (shim == null) return JValue.CreateNull();

            // The contract resolver and the object contract.
            var contractResolver = serializer.ContractResolver as DefaultContractResolver;
            var objectContract = contractResolver?.ResolveContract(value.GetType()) as JsonObjectContract;

            // Create the token from the shim.
            JToken token = shim.GetJToken(serializer, value, contractResolver);

            // Get the properties.
            IEnumerable<(string PropertyName, TypeInfo TypeInfo, object Value)> properties =
                // Cascade if null to default implementation.
                value.GetProperties(contractResolver, objectContract) ??
                value.GetProperties();

            // Cycle through the properties, add children
            // that expose exceptions.
            foreach (var property in properties)
            {
                // The type info.
                TypeInfo propertyTypeInfo = property.TypeInfo;

                // The value.
                JToken valueToken = null;

                // If this is assignable to an exception, set.
                if (ExceptionTypeInfo.IsAssignableFrom(propertyTypeInfo))
                    // Add the property, get the token.
                    valueToken = (property.Value as Exception).GetExceptionShimJToken(serializer);

                // Is it assignable to IEnumerable<Exception>?
                if (valueToken == null && EnumerableExceptionTypeInfo.IsAssignableFrom(propertyTypeInfo))
                {
                    // Get the value.
                    IEnumerable<Exception> exceptions = (IEnumerable<Exception>) property.Value;

                    // If there are no exceptions, write null, otherwise, write all the exceptions.
                    valueToken = exceptions == null ?
                        (JToken) JValue.CreateNull() :
                        JArray.FromObject(exceptions.Select(e => e.GetExceptionShimJToken(serializer)), serializer);
                }
                
                // TODO: Handle dictionary.

                // If the value token is not null, then
                // add the property and the token.
                if (valueToken != null)
                    // Set the property and value.
                    token[property.PropertyName] = valueToken;
            }

            // Return the token.
            return token;
        }

        private static readonly string MetaPropertyPrefix = "$";
        private static readonly string MetaPropertySuffix = "Meta";

        internal static JToken GetJToken(this ExceptionShim shim, JsonSerializer serializer, Exception value, DefaultContractResolver defaultContractResolver)
        {
            // Validate parameters.
            if (shim == null) throw new ArgumentNullException(nameof(shim));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (value == null) throw new ArgumentNullException(nameof(value));

            // Get the token.
            JToken token = JToken.FromObject(shim, serializer);

            // Construct the meta and get the token.
            JToken meta = JToken.FromObject(new ExceptionMeta {
                    Type = value.GetType().ToTypeShim()
                }, serializer);

            // If there's a resolver, use it, otherwise, use
            // the default.
            JProperty metaProperty = new JProperty(MetaPropertyPrefix + (defaultContractResolver?.GetResolvedPropertyName(MetaPropertySuffix) ?? MetaPropertySuffix), meta);

            // Attach to the token.
            token[metaProperty.Name] = metaProperty.Value;

            // Return the token.
            return token;
        }

        internal static IEnumerable<(string PropertyName, TypeInfo TypeInfo, object Value)>
            GetProperties(this Exception exception, DefaultContractResolver defaultContractResolver, JsonObjectContract serializer)
        {
            // Validate parameters.
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            // If null, return null.
            return serializer?.Properties.Select(p => (defaultContractResolver.GetResolvedPropertyName(p.PropertyName), p.PropertyType.GetTypeInfo(), p.ValueProvider.GetValue(exception)));
        }

        internal static IEnumerable<(string PropertyName, TypeInfo TypeInfo, object Value)>
            GetProperties(this Exception exception)
        {
            // Validate parameters.
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            // Use reflection.
            return exception.GetType().GetPropertiesWithPublicInstanceGetters().Select(p =>
                (p.Name, p.PropertyType.GetTypeInfo(), p.GetValue(exception)));
        }
    }
}
