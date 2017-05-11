using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Ofl.Core.Collections.Generic;

namespace Ofl.Core
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2012-07-28</created>
    /// <summary>Extenson methods for working with
    /// <see cref="Enum"/> instances.</summary>
    ///
    //////////////////////////////////////////////////
    public static class EnumExtensions
    {
        #region Lookup functions.

        /// <summary>The lookup (thread-safe) that will get implementations based on
        /// the type of the enumeration and whether or not it is case-sensitive.</summary>
        private static readonly ConcurrentDictionary<Tuple<Type, bool>, object> TryGetByEnumCodeMap =
            new ConcurrentDictionary<Tuple<Type, bool>, object>();

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-07-28</created>
        /// <summary>Tries to look up a value from the enumeration
        /// <typeparamref name="T"/> given a <paramref name="code"/>
        /// that is applied.</summary>
        /// <typeparam name="T">The type of the enumeration to
        /// try and perform the lookup on.</typeparam>
        /// <param name="code">The code to look up.</param>
        /// <returns>The value of <typeparamref name="T"/>
        /// that has the <paramref name="code"/> applied to
        /// it through an <see cref="EnumCodeAttribute"/>, or
        /// null if the value does not exist.</returns>
        ///
        //////////////////////////////////////////////////
        public static T? TryGetByEnumCode<T>(string code) where T : struct
        {
            // Call the overload.
            return TryGetByEnumCode<T>(code, false);
        }


        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-10-22</created>
        /// <summary>Gets the <see cref="EnumCodeAttribute"/>
        /// value given a value from an enumeration.</summary>
        /// <typeparam name="T">The type of the enumeration
        /// to get the <see cref="EnumCodeAttribute"/>
        /// from.</typeparam>
        /// <param name="value">The value from the enumeration type
        /// <typeparamref name="T"/> to get the attribute for.</param>
        /// <returns>The <see cref="EnumCodeAttribute"/> or null
        /// if there is none.</returns>
        /// 
        //////////////////////////////////////////////////
        public static EnumCodeAttribute GetEnumCodeAttribute<T>(T value) where T : struct
        {
            // Validate the value.
            ValidateParameter(value, "value");

            // Get the FieldInfo
            FieldInfo fieldInfo = GetEnumFieldsDictionary<T>()[value];

            // Get the attribute.
            return fieldInfo.GetCustomAttributes<EnumCodeAttribute>(true).SingleOrDefault();
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-10-22</created>
        /// <summary>Gets the <see cref="EnumCodeAttribute.Code"/>
        /// property given a value from an enumeration.</summary>
        /// <typeparam name="T">The type of the enumeration
        /// to get the <see cref="EnumCodeAttribute"/>
        /// from.</typeparam>
        /// <param name="value">The value from the enumeration type
        /// <typeparamref name="T"/> to get the attribute for.</param>
        /// <returns>The <see cref="EnumCodeAttribute.Code"/> or null
        /// if there is none.</returns>
        /// 
        //////////////////////////////////////////////////
        public static string GetEnumCodeAttributeCode<T>(T value) where T : struct
        {
            // Get the attribute.
            EnumCodeAttribute attribute = GetEnumCodeAttribute(value);

            // If it is null, return null, otherwise,
            // return the code.
            return attribute?.Code;
        }


        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-07-28</created>
        /// <summary>Tries to look up a value from the enumeration
        /// <typeparamref name="T"/> given a <paramref name="code"/>
        /// that is applied.</summary>
        /// <typeparam name="T">The type of the enumeration to
        /// try and perform the lookup on.</typeparam>
        /// <param name="code">The code to look up.</param>
        /// <param name="ignoreCase">If true, then the lookup
        /// is case insensitive.</param>
        /// <returns>The value of <typeparamref name="T"/>
        /// that has the <paramref name="code"/> applied to
        /// it through an <see cref="EnumCodeAttribute"/>, or
        /// null if the value does not exist.</returns>
        ///
        //////////////////////////////////////////////////
        public static T? TryGetByEnumCode<T>(string code, bool ignoreCase) where T : struct
        {
            // The type of T.
            Type t = typeof(T);

            // Validate parameters.
            if (!typeof(T).GetTypeInfo().IsEnum) throw new InvalidOperationException("The type parameter T must be an enumeration.");
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));

            // The key.
            var key = new Tuple<Type, bool>(t, ignoreCase);

            // Get the map.
            var map = (IDictionary<string, T>) TryGetByEnumCodeMap.
                GetOrAdd(key, k => SetupTryGetByEnumCodeMap<T>(ignoreCase));

            // Try and lookup.
            return map.TryGetValue(code);
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-07-28</created>
        /// <summary>Sets up the <see cref="IDictionary{TKey,TValue}"/>
        /// that maps the code to the <typeparamref name="T"/>.</summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <returns>The <see cref="IDictionary{TKey,TValue}"/>
        /// that is the map between the code and the
        /// enumeration value.</returns>
        ///
        //////////////////////////////////////////////////
        private static IDictionary<string, T> SetupTryGetByEnumCodeMap<T>(bool ignoreCase) where T : struct
        {
            // Get the type.
            Type t = typeof(T);

            // The type is an enumeration.
            Debug.Assert(t.GetTypeInfo().IsEnum);

            // Create the lookup.
            IDictionary<string, T> lookup = new Dictionary<string, T>(ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

            // Cycle through all of the fields.
            foreach (KeyValuePair<T, FieldInfo> pair in GetEnumFieldsDictionary<T>())
            {
                // Get the field info.
                FieldInfo fi = pair.Value;

                // The declaring type is not null.
                Debug.Assert(fi.DeclaringType != null);

                // If the attribute does not exist on the type, then throw
                // an exception.
                EnumCodeAttribute attribute = pair.Value.GetCustomAttributes<EnumCodeAttribute>(true).SingleOrDefault();

                // If the attribute is null, throw an exception.
                if (attribute == null) throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "The {0} value in the {1} enumeration does not have the EnumCodeAttribute applied to it.",
                        fi.Name, fi.DeclaringType.FullName));

                // The looked up value.
                T value;

                // Try and look up, it should not succeed.
                if (lookup.TryGetValue(attribute.Code, out value))
                {
                    // Throw an exception.
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.InvariantCulture, "The code \"{0}\" is already applied to another value in the {1} enumeration.",
                            attribute.Code, fi.DeclaringType.FullName));
                }

                // Add the value.
                lookup.Add(attribute.Code, pair.Key);
            }

            // Return the lookup.
            return lookup;
        }

        #endregion

        #region General helpers.

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-07-28</created>
        /// <summary>Gets the <see cref="FieldInfo"/>
        /// instances representing the fields of an enum.</summary>
        /// <typeparam name="T">The enumeration type
        /// to get the <see cref="FieldInfo"/> instances
        /// from.</typeparam>
        /// <returns>The sequence of <see cref="FieldInfo"/>
        /// instances that correspond to the fields on the
        /// enum type.</returns>
        ///
        //////////////////////////////////////////////////
        public static IEnumerable<FieldInfo> GetEnumFields<T>() where T : struct
        {
            // Call the overload.
            return GetEnumFields(typeof(T));
        }


        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-07-28</created>
        /// <summary>Gets the <see cref="FieldInfo"/>
        /// instances representing the fields of an enum.</summary>
        /// <param name="type">The <see cref="Type"/> to get the
        /// <see cref="FieldInfo"/> sequence from.</param>
        /// <returns>The sequence of <see cref="FieldInfo"/>
        /// instances that correspond to the fields on the
        /// enum type.</returns>
        ///
        //////////////////////////////////////////////////
        public static IEnumerable<FieldInfo> GetEnumFields(Type type)
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

        public static IEnumerable<object> GetEnumValues(Type type)
        {
            // Validate parameters.
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Get the fields, get the value for each field.
            return GetEnumFields(type).Select(f => f.GetValue(null));
        }

        public static IEnumerable<T> GetEnumValues<T>() where T : struct
        {
            // Call the overload, cast values to
            // T.
            return GetEnumValues(typeof(T)).Cast<T>();
        }


        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-07-28</created>
        /// <summary>Gets the <see cref="FieldInfo"/>
        /// instances representing the fields of an enum.</summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        ///
        //////////////////////////////////////////////////
        public static IDictionary<T, FieldInfo> GetEnumFieldsDictionary<T>() where T : struct
        {
            // Get the fields.
            IEnumerable<FieldInfo> fields = GetEnumFields<T>();

            // The type of T.
            Type t = typeof(T);

            // Now return a dictionary with the fields mapped to the values.
            return fields.ToDictionary(fi => (T) Enum.Parse(t, fi.Name));
        }

        #endregion

        #region Parameter validation.

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-07-28</created>
        /// <summary>Validates that
        /// <paramref name="value"/> is a valid
        /// value in the enumeration <typeparamref name="T"/>.</summary>
        /// <param name="value">The value of <typeparamref name="T"/>
        /// to check.</param>
        /// <param name="parameter">The name of the parameter that is being
        /// checked.</param>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        ///
        //////////////////////////////////////////////////
        public static void ValidateParameter<T>(T value, string parameter) where T : struct
        {
            // The type of the enumeration.
            Type t = typeof (T);

            // Validate parameters.
            if (!t.GetTypeInfo().IsEnum) throw new InvalidOperationException("The type parameter T must be an enumeration.");
            if (string.IsNullOrWhiteSpace(parameter)) throw new ArgumentNullException(nameof(parameter));

            // See if the value is in the enumeration, if it isn't then throw an exception.
            if (Enum.IsDefined(t, value)) return;

            // Throw an exception.
            // TODO: Figure out what to do with CultureInfo (need IFormatter implementation).
            throw new ArgumentException($"The value {value} is not defined in the enumeration {t.FullName}.",
                nameof(parameter));
        }

        #endregion
    }
}
