using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Create the read only collection.
            // Do not sniff for a list, as the
            // ReadOnlyCollection<T> is only a wrapper, so
            // this needs to be copied in case a reference to the
            // original list is held on to and mutated.
            return source.ToList().WrapInReadOnlyCollection();
        }
    }
}
