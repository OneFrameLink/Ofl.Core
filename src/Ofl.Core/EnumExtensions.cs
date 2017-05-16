using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ofl.Core
{
    public static class EnumExtensions
    {
        #region General helpers.

        public static IEnumerable<FieldInfo> GetFieldInfos<T>() where T : struct =>
            GetFieldInfos(typeof(T));


        public static IEnumerable<FieldInfo> GetFieldInfos(Type type)
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

        public static IEnumerable<KeyValuePair<FieldInfo, object>> GetEnumerator(Type type) =>
            GetFieldInfos(type).Select(f => new KeyValuePair<FieldInfo, object>(f, f.GetValue(null)));

        public static IEnumerable<KeyValuePair<FieldInfo, T>> GetEnumerator<T>() where T : struct =>
            GetEnumerator(typeof(T)).Select(p => new KeyValuePair<FieldInfo, T>(p.Key, (T) p.Value));

        public static IEnumerable<object> GetValues(Type type) =>
            GetEnumerator(type).Select(p => p.Value);

        public static IEnumerable<T> GetValues<T>() where T : struct =>
            GetValues(typeof(T)).Cast<T>();

        #endregion
    }
}
