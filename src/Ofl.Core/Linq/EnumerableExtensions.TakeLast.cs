using System;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Core.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, $"The { nameof(count) } parameter must be a non-negative number.");

            // If the count is zero, return empty.
            if (count == 0) return Enumerable.Empty<T>();

            // Start sniffing.
            // Read-only collection.
            var ro = source as IReadOnlyCollection<T>;
            if (ro != null) return ro.ReadOnlyCollectionTakeLast(count);

            // Collection.
            var c = source as ICollection<T>;
            if (c != null) return c.CollectionTakeLast(count);

            // Default.
            return source.EnumerableTakeLast(count);
        }

        private static IEnumerable<T> EnumerableTakeLast<T>(this IEnumerable<T> source, int count)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, $"The { nameof(count) } parameter must be a non-negative number.");

            // Keep a window of the items.
            var window = new Queue<T>(count);

            // Cycle thorugh the items.
            foreach (T item in source)
            {
                // Queue the item.
                window.Enqueue(item);

                // If the count on the window is greater
                // than the count of items to take the last of, then
                // dequeue it.
                if (window.Count > count) window.Dequeue();
            }

            // Return the queue.
            return window;
        }

        private static IEnumerable<T> CollectionTakeLast<T>(this ICollection<T> source, int count)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, $"The { nameof(count) } parameter must be a non-negative number.");

            // Get the count.  It's the min of the count
            // and the source items.
            count = Math.Min(source.Count, count);

            // If the count is zero, return empty.
            if (count == 0) return Enumerable.Empty<T>();

            // If the count is equal to the source count, return itself.
            if (count == source.Count) return source;

            // The number of elements to skip.  Subtract
            // collection count minus the count desired.
            return source.Skip(source.Count - count);
        }
        private static IEnumerable<T> ReadOnlyCollectionTakeLast<T>(this IReadOnlyCollection<T> source, int count)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, $"The { nameof(count) } parameter must be a non-negative number.");

            // Get the count.  It's the min of the count
            // and the source items.
            count = Math.Min(source.Count, count);

            // If the count is zero, return empty.
            if (count == 0) return Enumerable.Empty<T>();

            // If the count is equal to the source count, return itself.
            if (count == source.Count) return source;

            // The number of elements to skip.  Subtract
            // collection count minus the count desired.
            return source.Skip(source.Count - count);
        }
    }
}
