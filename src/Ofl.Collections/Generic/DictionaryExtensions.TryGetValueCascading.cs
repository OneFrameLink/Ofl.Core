using System;
using System.Collections.Generic;
using System.Linq;
using Ofl.Linq;

namespace Ofl.Collections.Generic
{
    public static partial class DictionaryExtensions
    {
        public static bool TryGetValueCascading<TKey, TValue>(
            this IEnumerable<IDictionary<TKey, TValue>> dictionaries,
            TKey key, out TValue value)
        {
            // Validate parameters.
            if (dictionaries == null) throw new ArgumentNullException(nameof(dictionaries));

            // Set to the default.
            value = default(TValue);

            // Cycle through the dictionaries, first one
            // that can produce a result wins.
            foreach (IReadOnlyDictionary<TKey, TValue> dictionary in dictionaries)
                // If found, return.
                if (dictionary.TryGetValue(key, out value)) return true;

            // Return false.
            return false;
        }

        public static TValue? TryGetValueCascading<TKey, TValue>(
            this IEnumerable<IDictionary<TKey, TValue>> dictionaries, TKey key)
            where TValue : struct => dictionaries.TryGetValueCascading(key, out TValue value) ? value : (TValue?) null;

        public static TValue? TryGetValueCascading<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, IEnumerable<IDictionary<TKey, TValue>> fallbacks)
            where TValue : struct
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (fallbacks == null) throw new ArgumentNullException(nameof(fallbacks));

            // Call the overload.
            return EnumerableExtensions.From(dictionary).Concat(fallbacks).TryGetValueCascading(key);
        }

        public static TValue? TryGetValueCascading<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, params IDictionary<TKey, TValue>[] fallbacks)
            where TValue : struct => dictionary.TryGetValueCascading(key, (IEnumerable<IDictionary<TKey, TValue>>) fallbacks);

        public static bool TryGetValueCascading<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, out TValue value, IEnumerable<IDictionary<TKey, TValue>> fallbacks)
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (fallbacks == null) throw new ArgumentNullException(nameof(fallbacks));

            // Call the overload.
            return EnumerableExtensions.From(dictionary).Concat(fallbacks).TryGetValueCascading(key, out value);
        }
        public static bool TryGetValueCascading<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, out TValue value, params IReadOnlyDictionary<TKey, TValue>[] fallbacks) =>
                dictionary.TryGetValueCascading(key, out value, (IEnumerable<IDictionary<TKey, TValue>>) fallbacks);
    }
}
