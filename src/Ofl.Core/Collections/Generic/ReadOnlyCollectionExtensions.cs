﻿using System;
using System.Collections.Generic;

namespace Ofl.Core.Collections.Generic
{
    public static class ReadOnlyCollectionExtensions
    {
        public static IReadOnlyCollection<T> Append<T>(this IReadOnlyCollection<T> source, T item)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Return the appended read only collection.
            return new AppendedReadOnlyCollection<T>(source, item);
        }
    }
}
