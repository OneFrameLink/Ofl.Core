using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Ofl.Core.Collections.Generic
{
    internal class AsyncEnumerator<TState, TResult> : IAsyncEnumerator<TResult>
    {
        #region Constructor

        internal AsyncEnumerator(Func<CancellationToken, Task<TState>> stateFactory, Func<TState, CancellationToken, Task<bool>> condition,
            Func<TState, CancellationToken, Task<TState>> iterator, Func<TState, CancellationToken, Task<TResult>> selector)
        {
            // Validate parameters.
            if (stateFactory == null) throw new ArgumentNullException(nameof(stateFactory));
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            if (iterator == null) throw new ArgumentNullException(nameof(iterator));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            // Assign values.
            _stateFactory = stateFactory;
            _condition = condition;
            _iterator = iterator;
            _selector = selector;
        }

        #endregion

        #region Instance state.

        private readonly Func<CancellationToken, Task<TState>> _stateFactory;

        private readonly Func<TState, CancellationToken, Task<bool>> _condition;

        private readonly Func<TState, CancellationToken, Task<TState>> _iterator;

        private readonly Func<TState, CancellationToken, Task<TResult>> _selector;

        private TState _state;

        private bool _hasState;

        private bool _disposed;

        private readonly AsyncLock _asyncLock = new AsyncLock();

        private TResult _current;

        private bool _alive = true;

        #endregion

        #region Implementation of IAsyncEnumerator<out T>

        /// <summary>
        /// Advances the enumerator to the next element in the sequence, returning the result asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        /// Task containing the result of the operation: true if the enumerator was successfully advanced 
        ///             to the next element; false if the enumerator has passed the end of the sequence.
        /// </returns>
        public async Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            // If disposed, return false.
            if (_disposed) return false;

            // Wait on the async lock.
            using (await _asyncLock.LockAsync(cancellationToken).ConfigureAwait(false))
            {
                // If not alive get out.
                if (!_alive) return false;

                // If there is no state, get it.
                if (!_hasState)
                {
                    // Get the state.
                    _state = await _stateFactory(cancellationToken).ConfigureAwait(false);

                    // Set the flag.
                    _hasState = true;
                }

                // Check the condition.
                if (!await _condition(_state, cancellationToken).ConfigureAwait(false)) return _alive = false;

                // Iterate.
                _state = await _iterator(_state, cancellationToken).ConfigureAwait(false);

                // Set the current value.
                _current = await _selector(_state, cancellationToken).ConfigureAwait(false);

                // Return true.
                return true;
            }
        }

        /// <summary>
        /// Gets the current element in the iteration.
        /// </summary>
        public TResult Current
        {
            get
            {
                // If this has been disposed, throw an exception.
                if (_disposed) throw new ObjectDisposedException(GetType().FullName);

                // Lock.
                using (_asyncLock.Lock())
                {
                    // If not alive, throw an exception.
                    if (!_alive) throw new InvalidOperationException("The last call to MoveNext returned false.");

                    // Return the value.
                    return _current;
                }
            }
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Call the overload of dispose, remove
            // from finalization.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            // If disposed, get out.
            if (_disposed) return;

            // Always set the flag.
            try
            {
                // Dispose of unmanaged resources.

                // If not disposing, get out.
                if (!disposing) return;

                // Dispose of the state.
                using (_state as IDisposable) { }
            }
            finally
            {
                // This is disposed.
                _disposed = true;
            }
        }

        ~AsyncEnumerator()
        {
            // Call the overload of Dispose.
            Dispose(false);
        }

        #endregion
    }
}
