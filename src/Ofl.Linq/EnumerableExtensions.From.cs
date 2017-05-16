using System;
using System.Collections.Generic;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> From<T>(params T[] items)
        {
            // Validate parameters.
            if (items == null) throw new ArgumentNullException(nameof(items));

            // Just return the array.
            return items;
        }
    }
}
