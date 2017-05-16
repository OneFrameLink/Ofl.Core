using System;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T?> AsNullableSequence<T>(this IEnumerable<T> source)
            where T : struct
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Map.
            return source.Select(t => (T?) t);
        }
    }
}
