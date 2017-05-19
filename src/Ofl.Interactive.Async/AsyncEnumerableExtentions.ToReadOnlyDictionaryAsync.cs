using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Interactive.Async
{
    public static partial class AsyncEnumerableExtentions
    {
        public static Task<ReadOnlyDictionary<TKey, TSource>> ToReadOnlyDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, 
            CancellationToken cancellationToken) => source.ToReadOnlyDictionaryAsync(keySelector, EqualityComparer<TKey>.Default, cancellationToken);

        public static Task<ReadOnlyDictionary<TKey, TSource>> ToReadOnlyDictionaryAsync<TSource, TKey>(
            this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer, CancellationToken cancellationToken) => 
                source.ToReadOnlyDictionaryAsync(keySelector, t => t, EqualityComparer<TKey>.Default, cancellationToken);

        public static Task<ReadOnlyDictionary<TKey, TElement>> ToReadOnlyDictionaryAsync<TSource, TKey, TElement>(
            this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            CancellationToken cancellationToken) => source.ToReadOnlyDictionaryAsync(keySelector, elementSelector, EqualityComparer<TKey>.Default,
                cancellationToken);

        public static async Task<ReadOnlyDictionary<TKey, TElement>> ToReadOnlyDictionaryAsync<TSource, TKey, TElement>(
            this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // Create the dictionary.
            IDictionary<TKey, TElement> dictionary = await source.ToDictionary(
                keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);

            // Wrap and return.
            return new ReadOnlyDictionary<TKey, TElement>(dictionary);
        }
    }
}
