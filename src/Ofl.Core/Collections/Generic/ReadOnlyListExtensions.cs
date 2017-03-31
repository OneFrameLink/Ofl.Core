using System;
using System.Collections.Generic;

namespace Ofl.Core.Collections.Generic
{
    public static class ReadOnlyListExtensions
    {
        public static int BinarySearch<T>(this IReadOnlyList<T> list, T value)
        {
            // Call the overload.
            return list.BinarySearch(value, Comparer<T>.Default);
        }

        public static int BinarySearch<T>(this IReadOnlyList<T> list, int index, int length, T value)
        {
            // Call the overload.
            return list.BinarySearch(index, length, value, Comparer<T>.Default);
        }

        public static int BinarySearch<T>(this IReadOnlyList<T> list, T value, IComparer<T> comparer)
        {
            // Call the overload.
            return list.BinarySearch(0, list.Count, value, comparer);
        }

        public static int BinarySearch<T>(this IReadOnlyList<T> list, int index, int length, T value, IComparer<T> comparer)
        {
            // Validate parameters.
            if (list == null) throw new ArgumentNullException(nameof(list));
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
                int order = comparer.Compare(list[midpoint], value);

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
