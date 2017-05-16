using System;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static bool In<T>(this T item, params T[] items) => item.In((IEnumerable<T>) items);

        public static bool In<T>(this T item, IEqualityComparer<T> equalityComparer, params T[] items) => item.In(items, equalityComparer);

        public static bool In<T>(this T item, IEnumerable<T> items) => item.In(items, EqualityComparer<T>.Default);

        public static bool In<T>(this T item, IEnumerable<T> items, IEqualityComparer<T> equalityComparer)
        {
            // Validate parameters.
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (equalityComparer == null) throw new ArgumentNullException(nameof(equalityComparer));

            // Just a call to contains.
            return items.Contains(item, equalityComparer);
        }
    }
}
