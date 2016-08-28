using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Core.Linq;

namespace Ofl.Core.Collections.Generic
{
    public static class AsyncEnumerableExtentions
    {
        public static IAsyncEnumerable<TResult> Generate<TState, TResult>(Func<CancellationToken, Task<TState>> stateFactory, 
            Func<TState, CancellationToken, Task<bool>> condition, Func<TState, CancellationToken, Task<TState>> iterator, 
            Func<TState, CancellationToken, Task<TResult>> selector)
        {
            // Validate parameters.
            if (stateFactory == null) throw new ArgumentNullException(nameof(stateFactory));
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            if (iterator == null) throw new ArgumentNullException(nameof(iterator));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            // Create the factory and return.
            return new AsyncEnumerable<TResult>(() => new AsyncEnumerator<TState, TResult>(stateFactory, condition, iterator, selector));
        }

        public static async Task<ReadOnlyCollection<T>> ToReadOnlyCollectionAsync<T>(this IAsyncEnumerable<T> source, 
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Create the list, wrap.
            return (await source.ToList(cancellationToken).ConfigureAwait(false)).
                WrapInReadOnlyCollection();
        }
    }
}
