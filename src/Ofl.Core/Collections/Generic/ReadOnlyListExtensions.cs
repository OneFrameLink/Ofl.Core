using System;
using System.Collections.Generic;

namespace Ofl.Core.Collections.Generic
{
    public static class ReadOnlyListExtensions
    {
        public static int BinarySearch<T>(this IReadOnlyList<T> list, T value)
        {
            // Call the overload.
            return list.BinarySearch(t => t, value);
        }

        public static int BinarySearch<TSource, TValue>(this IReadOnlyList<TSource> list, Func<TSource, TValue> map, TValue value)
        {
            // Call the overload.
            return list.BinarySearch(map, value, Comparer<TValue>.Default);
        }

        public static int BinarySearch<T>(this IReadOnlyList<T> list, int index, int length, T value)
        {
            // Call the overload.
            return list.BinarySearch(index, length, t => t, value);
        }

        public static int BinarySearch<TSource, TValue>(this IReadOnlyList<TSource> list, int index, int length, Func<TSource, TValue> map, TValue value)
        {
            // Call the overload.
            return list.BinarySearch(index, length, map, value, Comparer<TValue>.Default);
        }


        public static int BinarySearch<T>(this IReadOnlyList<T> list, T value, IComparer<T> comparer)
        {
            // Call the overload.
            return list.BinarySearch(0, list.Count, t => t, value, comparer);
        }

        public static int BinarySearch<TSource, TValue>(this IReadOnlyList<TSource> list, Func<TSource, TValue> map, TValue value, IComparer<TValue> comparer)
        {
            // Call the overload.
            return list.BinarySearch(0, list.Count, map, value, comparer);
        }


        public static int BinarySearch<T>(this IReadOnlyList<T> list, int index, int length, T value, IComparer<T> comparer)
        {
            // Call the overload.
            return list.BinarySearch(index, length, t => t, value, comparer);
        }

        public static int BinarySearch<TSource, TValue>(this IReadOnlyList<TSource> list, int index, int length, Func<TSource, TValue> map, TValue value,
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
