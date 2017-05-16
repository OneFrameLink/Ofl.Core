using System;
using System.Collections.Generic;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<TResult> ZipChecked<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            // Validate parameters.
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            // Call the implementation.
            return first.ZipCheckedImplementation(second, resultSelector);
        }

        private static IEnumerable<TResult> ZipCheckedImplementation<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            // Validate parameters.
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            // Get the enumerators.
            using (IEnumerator<TFirst> firstEnumerator = first.GetEnumerator())
            using (IEnumerator<TSecond> secondEnumerator = second.GetEnumerator())
                // Will short circuit later.
                while (true)
                {
                    // Advance each.
                    bool firstMoveNext = firstEnumerator.MoveNext();
                    bool secondMoveNext = secondEnumerator.MoveNext();

                    // If both not true, or both not false, throw an exception.
                    if (firstMoveNext ^ secondMoveNext)
                        throw new InvalidOperationException($"The { (firstMoveNext ? nameof(first) : nameof(second)) } sequence yielded more elements than the { (firstMoveNext ? nameof(second) : nameof(first)) } sequence.");

                    // If there is an item, yield.  If not, break.
                    if (firstMoveNext)
                        yield return resultSelector(firstEnumerator.Current, secondEnumerator.Current);
                    else
                        // Break, no more elements.
                        yield break;
                }
        }
    }
}
