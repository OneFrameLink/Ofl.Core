using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ofl.Core.Collections.Generic
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2013-03-24</created>
    /// <summary>Exposes extension methods for the <see cref="IList{T}"/>
    /// interface.</summary>
    ///
    //////////////////////////////////////////////////
    public static class ListExtensions
    {
        public static void SafeRemoveRange<T>(this List<T> source, int index, int count)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // If the index is less than 0 or the count is less than 0, then
            // just get out.
            if (index < 0 || count < 0) return;

            // If the index is greater than or equal the number
            // of elements, get out.
            if (index >= source.Count) return;

            // Take the min of the count of the list minus index
            // and the count minus the index.
            count = Math.Min(count, source.Count) - index;

            // Update.
            source.RemoveRange(index, count);
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-03-24</created>
        /// <summary>Cycles through a list, removing the elements
        /// from the list which match a certain <paramref name="predicate"/>.</summary>
        /// <param name="source">The <see cref="IList{T}"/> to remove
        /// the items from.</param>
        /// <param name="predicate">The <see cref="Func{T, TResult}"/> that will
        /// be used to filter the items in the list.</param>
        /// <returns>A sequence of the items that satisified the conditions
        /// and were removed from the list.</returns>
        /// <typeparam name="T">The type of the list.</typeparam>
        ///
        //////////////////////////////////////////////////
        public static IEnumerable<T> RemoveWhere<T>(this IList<T> source, Func<T, bool> predicate)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            // Call the implementation.
            return source.RemoveWhereImplementation(predicate);
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-03-24</created>
        /// <summary>Cycles through a list, removing the elements
        /// from the list which match a certain <paramref name="predicate"/>.</summary>
        /// <param name="source">The <see cref="IList{T}"/> to remove
        /// the items from.</param>
        /// <param name="predicate">The <see cref="Func{T, TResult}"/> that will
        /// be used to filter the items in the list.</param>
        /// <returns>A sequence of the items that satisified the conditions
        /// and were removed from the list.</returns>
        /// <typeparam name="T">The type of the list.</typeparam>
        ///
        //////////////////////////////////////////////////
        private static IEnumerable<T> RemoveWhereImplementation<T>(this IList<T> source, Func<T, bool> predicate)
        {
            // Validate parameters.
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);

            // Cycle backwards through the source.
            for (int i = source.Count - 1; i >= 0; --i)
            {
                // The item.
                T item = source[i];

                // If the item meets the predicate, then remove it.
                if (!predicate(item)) continue;

                // Remove the item.
                source.RemoveAt(i);

                // Yield the item.
                yield return item;
            }
        }
    }
}
