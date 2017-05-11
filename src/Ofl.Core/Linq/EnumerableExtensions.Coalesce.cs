using System;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Core.Linq
{
    public static partial class EnumerableExtensions
    {
        public static T Coalesce<T>(params T[] values)
            where T : class => values.Coalesce();

        public static T Coalesce<T>(this IEnumerable<T> source)
            where T : class
        {
            // Validate parameters.
			if (source == null) throw new ArgumentNullException(nameof(source));

			// Get the first where it's not null.
            return source.FirstOrDefault(i => i != null);
        }

        public static T? Coalesce<T>(params T?[] values)
            where T : struct => values.Coalesce();

        public static T? Coalesce<T>(this IEnumerable<T?> source)
            where T : struct
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Get the first where it's not null.
            return source.FirstOrDefault(i => i != null);
        }
    }
}
