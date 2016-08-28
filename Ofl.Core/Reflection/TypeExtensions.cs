using System;
using System.Collections.Generic;
using System.Reflection;
using Ofl.Core.Linq;

namespace Ofl.Core.Reflection
{
    public static class TypeExtensions
    {
        public static IReadOnlyCollection<PropertyInfo> GetPublicInstanceProperties(this Type type)
        {
            // Validate parameters.
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Get the properties.
            return type.GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public).
                WrapInReadOnlyCollection();
        }

        public static IReadOnlyCollection<PropertyInfo> GetPublicInstanceProperties<T>()
        {
            // Call the overload with T.
            return typeof(T).GetPublicInstanceProperties();
        }
    }
}
