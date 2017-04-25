using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ofl.Core.Linq;

namespace Ofl.Core.Reflection
{
    public static class TypeExtensions
    {
        public static IReadOnlyCollection<PropertyInfo> GetPropertiesWithPublicInstanceGetters(this Type type)
        {
            // Validate parameters.
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Get the properties.
            // NOTE: Used to be
            //return type.GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public).
            return type.GetRuntimeProperties().Where(p => !p.GetMethod.IsStatic && p.GetMethod.IsPublic).
                ToReadOnlyCollection();
        }

        public static IReadOnlyCollection<PropertyInfo> GetPropertiesWithPublicInstanceGetters<T>()
        {
            // Call the overload with T.
            return typeof(T).GetPropertiesWithPublicInstanceGetters();
        }

        public static bool IsAssignableFrom(this Type to, Type from)
        {
            // Validate parameters.
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));

            // Get the type infos.
            TypeInfo fromTypeInfo = from.GetTypeInfo();
            TypeInfo toTypeInfo = to.GetTypeInfo();

            // Call.
            return toTypeInfo.IsAssignableFrom(fromTypeInfo);
        }
    }
}
