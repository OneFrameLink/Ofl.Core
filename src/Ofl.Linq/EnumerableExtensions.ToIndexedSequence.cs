using System;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<KeyValuePair<int, T>> ToIndexedSequence<T>(this IEnumerable<T> source)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Return the sequence mapped to the index.
            return source.Select((t, i) => new KeyValuePair<int, T>(i, t));
        }
    }
}
