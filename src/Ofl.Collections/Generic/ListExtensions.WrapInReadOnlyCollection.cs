using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ofl.Collections.Generic
{
    public static partial class ListExtensions
    {
        public static ReadOnlyCollection<T> WrapInReadOnlyCollection<T>(this IList<T> source)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Wrap.
            return new ReadOnlyCollection<T>(source);
        }
    }
}
