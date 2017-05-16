using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ofl.Collections.Generic
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2013-03-24</created>
    /// <summary>Exposes extension methods for the <see cref="IList{T}"/>
    /// interface.</summary>
    ///
    //////////////////////////////////////////////////
    public static partial class ListExtensions
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

        public static int BinarySearch<T>(this IList<T> list, T value)
        {
            // Call the overload.
            return list.BinarySearch(t => t, value);
        }

        public static int BinarySearch<TSource, TValue>(this IList<TSource> list, Func<TSource, TValue> map, TValue value)
        {
            // Call the overload.
            return list.BinarySearch(map, value, Comparer<TValue>.Default);
        }

        public static int BinarySearch<T>(this IList<T> list, int index, int length, T value)
        {
            // Call the overload.
            return list.BinarySearch(index, length, t => t, value);
        }

        public static int BinarySearch<TSource, TValue>(this IList<TSource> list, int index, int length, Func<TSource, TValue> map, TValue value)
        {
            // Call the overload.
            return list.BinarySearch(index, length, map, value, Comparer<TValue>.Default);
        }


        public static int BinarySearch<T>(this IList<T> list, T value, IComparer<T> comparer)
        {
            // Call the overload.
            return list.BinarySearch(0, list.Count, t => t, value, comparer);
        }

        public static int BinarySearch<TSource, TValue>(this IList<TSource> list, Func<TSource, TValue> map, TValue value, IComparer<TValue> comparer)
        {
            // Call the overload.
            return list.BinarySearch(0, list.Count, map, value, comparer);
        }


        public static int BinarySearch<T>(this IList<T> list, int index, int length, T value, IComparer<T> comparer)
        {
            // Call the overload.
            return list.BinarySearch(index, length, t => t, value, comparer);
        }

        public static int BinarySearch<TSource, TValue>(this IList<TSource> list, int index, int length, Func<TSource, TValue> map, TValue value,
            IComparer<TValue> comparer)
        {
            // Validate parameters.
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (map == null) throw new ArgumentNullException(nameof(map));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), index, $"The { nameof(index) } parameter must be a non-negative value.");
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), length, $"The {nameof(length)} parmeter must be a non-negative value.");
            if (index + length > list.Count) throw new InvalidOperationException($"The value of {nameof(index)} plus {nameof(length)} must be less than or equal to the value of the number of items in the {nameof(list)}.");

            // Do work.
            // Taken from https://github.com/dotnet/coreclr/blob/cdff8b0babe5d82737058ccdae8b14d8ae90160d/src/mscorlib/src/System/Collections/Generic/ArraySortHelper.cs#L156
            // The lo and high bounds.
            int low = index;
            int high = index + length - 1;

            // While low < high.
            while (low <= high)
            {
                // The midpoint.
                int midpoint = low + ((high - low) >> 1);

                // Compare.
                int order = comparer.Compare(map(list[midpoint]), value);

                // If they are equal, return.
                if (order == 0) return midpoint;

                // If less than zero, reset low, otherwise, reset high.
                if (order < 0)
                    low = midpoint + 1;
                else
                    high = midpoint - 1;
            }

            // Nothing matched, return inverse of the low bound.
            return ~low;
        }
    }
}
