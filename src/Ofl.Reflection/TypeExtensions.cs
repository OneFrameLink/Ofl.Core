using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ofl.Reflection
{
    public static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetPropertiesWithPublicInstanceGetters(this Type type)
        {
            // Validate parameters.
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Get the properties.
            // NOTE: Used to be
            //return type.GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public).
            return type.GetRuntimeProperties().Where(p => !p.GetMethod.IsStatic && p.GetMethod.IsPublic);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithPublicInstanceGetters<T>() =>
            typeof(T).GetPropertiesWithPublicInstanceGetters();

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
