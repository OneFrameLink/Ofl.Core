using System;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static int? IndexOf<T>(this IEnumerable<T> source, T item) =>
            source.IndexOf(item, EqualityComparer<T>.Default);

        public static int? IndexOf<T>(this IEnumerable<T> source, T item, IEqualityComparer<T> equalityComparer)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (equalityComparer == null) throw new ArgumentNullException(nameof(equalityComparer));

            // Map to a sequence, then filter, and provide the first sequence
            // where it exists.
            return source.Select((i, index) => new { Item = i, Index = index })
                .FirstOrDefault(p => equalityComparer.Equals(p.Item, item))
                ?.Index;
        }
    }
}
