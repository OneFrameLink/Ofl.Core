using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ofl.Linq
{
    // NOTE: This exists because Ofl.Collections references this assembly,
    // so we can't use the extensions in there.  Easier to
    // just dupe them here for now.
    internal static class WrapperExtensions
    {
        internal static ReadOnlyCollection<T> WrapInReadOnlyCollection<T>(this IList<T> list)
        {
            // Validate parameters.
            if (list == null) throw new ArgumentNullException(nameof(list));

            // Wrap.
            return new ReadOnlyCollection<T>(list);
        }

        internal static ReadOnlyDictionary<TKey, TValue> WrapInReadOnlyDictionary<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary)
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            // Wrap.
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}
