using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ofl.Core
{
    public static class EnumExtensions
    {
        #region General helpers.

        public static IEnumerable<FieldInfo> EnumerateFields<T>() where T : struct =>
            EnumerateFields(typeof(T));


        public static IEnumerable<FieldInfo> EnumerateFields(Type type)
        {
            // Validate parameters.
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Validate parameters.
            if (!type.GetTypeInfo().IsEnum) throw new ArgumentException("The type parameter must be an enumeration.", nameof(type));

            // Filter the fields.
            // Note: Used to be 
            // GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            return type.GetTypeInfo().DeclaredFields.Where(f => f.IsStatic);
        }

        public static IEnumerable<KeyValuePair<FieldInfo, object>> EnumerateFieldsAndValues(Type type) =>
            EnumerateFields(type).Select(f => new KeyValuePair<FieldInfo, object>(f, f.GetValue(null)));

        public static IEnumerable<KeyValuePair<FieldInfo, T>> EnumerateFieldsAndValues<T>() where T : struct =>
            EnumerateFieldsAndValues(typeof(T)).Select(p => new KeyValuePair<FieldInfo, T>(p.Key, (T) p.Value));

        public static IEnumerable<object> EnumerateValues(Type type) =>
            EnumerateFieldsAndValues(type).Select(p => p.Value);

        public static IEnumerable<T> EnumerateValues<T>() where T : struct =>
            EnumerateValues(typeof(T)).Cast<T>();

        #endregion
    }
}
