using System;
using System.Collections.Generic;

namespace Ofl.Core.Collections.Generic
{
    class AsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        #region Constructor.

        internal AsyncEnumerable(Func<IAsyncEnumerator<T>> enumeratorFactory)
        {
            // Validate parameters.
            if (enumeratorFactory == null) throw new ArgumentNullException(nameof(enumeratorFactory));

            // Assign values.
            _enumeratorFactory = enumeratorFactory;
        }

        #endregion

        #region Instance, read-only state.

        private readonly Func<IAsyncEnumerator<T>> _enumeratorFactory;

        #endregion

        #region Implementation of IAsyncEnumerable<out TResult>

        /// <summary>
        /// Gets an asynchronous enumerator over the sequence.
        /// </summary>
        /// <returns>
        /// Enumerator for asynchronous enumeration over the sequence.
        /// </returns>
        public IAsyncEnumerator<T> GetEnumerator()
        {
            // Return the enumerator.
            return _enumeratorFactory();
        }

        #endregion
    }
}
