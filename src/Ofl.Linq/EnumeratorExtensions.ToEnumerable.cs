using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ofl.Linq
{
    public static partial class EnumeratorExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator) =>
            enumerator.ToEnumerable(false);

        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator, bool yieldCurrentFirst)
        {
            // Validate parameters.
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));

            // Pass to the implementation.
            return enumerator.ToEnumerableImplementation(yieldCurrentFirst);
        }

        private static IEnumerable<T> ToEnumerableImplementation<T>(this IEnumerator<T> enumerator, bool yieldCurrentFirst)
        {
            // Validate parameters.
            Debug.Assert(enumerator != null);

            // If yielding current first, do so here.
            if (yieldCurrentFirst) yield return enumerator.Current;

            // While movenext.
            while (enumerator.MoveNext())
                // Yield.
                yield return enumerator.Current;
        }
    }
}
