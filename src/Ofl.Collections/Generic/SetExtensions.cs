using System;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Collections.Generic
{
    public static class SetExtensions
    {
        public static IReadOnlyCollection<SetAddRangeResult<T>> AddRange<T>(this ISet<T> set, IEnumerable<T> items)
        {
            // Validate parameters.
            if (set == null) throw new ArgumentNullException(nameof(set));
            if (items == null) throw new ArgumentNullException(nameof(items));

            // Type sniff.
            var collection = items as ICollection<T>;

            // The list to store the results.
            var added = new List<SetAddRangeResult<T>>(collection != null ? collection.Count : 1);

            // Add the items.
            added.AddRange(items.Select(i => new SetAddRangeResult<T>(i, set.Add(i))));

            // Return the result.  Don't use ToReadOnlyCollection as it will create a new list.
            return added.WrapInReadOnlyCollection();
        }
    }
}
