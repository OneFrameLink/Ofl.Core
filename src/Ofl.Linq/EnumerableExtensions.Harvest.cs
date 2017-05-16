using System;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Harvest<T>(this IEnumerable<T> source, ICollection<T> harvester)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (harvester == null) throw new ArgumentNullException(nameof(harvester));

            // Add a select in between which calls Add on the harester.
            return source.Select(i => { harvester.Add(i); return i; });
        }
    }
}
