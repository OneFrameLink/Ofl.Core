using System;
using System.Collections.Generic;
using System.Text;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static void Drain<T>(this IEnumerable<T> source)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Call Do with an empty action.
            source.Do(t => { });
        }
    }
}
