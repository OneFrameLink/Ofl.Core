using System;
using System.Collections.Generic;
using System.Linq;
using Ofl.Core.Linq;

namespace Ofl.Core.Collections.Generic
{
    public static class ReadOnlyDictionaryExtensions
    {
        public static IReadOnlyDictionary<TToKey, TToValue> Cast<TFromKey, TFromValue, TToKey, TToValue>(
            this IReadOnlyDictionary<TFromKey, TFromValue> source)
            where TFromKey : TToKey
            where TFromValue : TToValue
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Create the wrapper, return.
            return new CastingReadOnlyDictionaryWrapper<TFromKey, TFromValue, TToKey, TToValue>(source);
        }
        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (key == null) throw new ArgumentNullException(nameof(key));

            // The value.
            TValue value;

            // Get the value, if it exists, return, otherwise, return
            // default.
            return dictionary.TryGetValue(key, out value) ? value : default(TValue);
        }
        public static TValue? TryGetValueCascading<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary,
            TKey key, params IReadOnlyDictionary<TKey, TValue>[] fallbacks)
            where TValue : struct
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (fallbacks == null) throw new ArgumentNullException(nameof(fallbacks));

            // The value.
            TValue value = default(TValue);

            // Cycle through the dictionaries.
            if (EnumerableExtensions.From(dictionary).Concat(fallbacks).Any(d => d.TryGetValue(key, out value)))
            {
                // Return the value.
                return value;
            }

            // Not found.
            return null;
        }

        public static TValue? TryGetValue<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : struct
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            // The value.
            TValue value;

            // If the value is not found, return null.
            if (!dictionary.TryGetValue(key, out value)) return null;

            // Return the value.
            return value;
        }

        public static bool TryGetValueCascading<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary,
            TKey key, out TValue value, params IReadOnlyDictionary<TKey, TValue>[] fallbacks)
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (fallbacks == null) throw new ArgumentNullException(nameof(fallbacks));

            // Set output.
            value = default(TValue);

            // Cycle through the dictionaries.
            foreach (IReadOnlyDictionary<TKey, TValue> d in EnumerableExtensions.From(dictionary).Concat(fallbacks))
            {
                // Try and get the value, if found, return true.
                if (d.TryGetValue(key, out value)) return true;
            }

            // Not found.
            return false;
        }
    }
}
