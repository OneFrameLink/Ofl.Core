using System;
using System.Collections.Generic;

namespace Ofl.Collections.Generic
{
    public static partial class ReadOnlyDictionaryExtensions
    {
        public static TValue GetValueOrDefaultCascading<TKey, TValue>(
            this IEnumerable<IReadOnlyDictionary<TKey, TValue>> dictionaries,
            TKey key, TValue defaultValue)
        {
            // Validate parameters.
            if (dictionaries == null) throw new ArgumentNullException(nameof(dictionaries));

            // Try and get the value cascading, if false, return
            // default.
            return dictionaries.TryGetValueCascading(key, out TValue value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefaultCascading<TKey, TValue>(
            this IEnumerable<IReadOnlyDictionary<TKey, TValue>> dictionaries,
            TKey key) => dictionaries.GetValueOrDefaultCascading(key, default(TValue));
    }
}
