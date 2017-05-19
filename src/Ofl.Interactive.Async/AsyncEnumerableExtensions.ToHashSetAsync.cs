using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Interactive.Async
{
    public static partial class AsyncEnumerableExtensions
    {
        public static Task<HashSet<T>> ToHashSetAsync<T>(this IAsyncEnumerable<T> source,
            CancellationToken cancellationToken) => source.ToHashSetAsync(EqualityComparer<T>.Default, cancellationToken);

        public static async Task<HashSet<T>> ToHashSetAsync<T>(this IAsyncEnumerable<T> source,
            IEqualityComparer<T> comparer, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // The hash set.
            var hashSet = new HashSet<T>(comparer);

            // Dispose when done.
            using (IAsyncEnumerator<T> enumerator = source.GetEnumerator())
                // Iterate.
                while (await enumerator.MoveNext(cancellationToken).ConfigureAwait(false))
                    // Add to the hashset.
                    hashSet.Add(enumerator.Current);

            // Return the hashset.
            return hashSet;
        }
    }
}
