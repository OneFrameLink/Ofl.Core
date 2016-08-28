using System;
using System.Collections;
using System.Collections.Generic;

namespace Ofl.Core.Collections.Generic
{
    class AppendedReadOnlyCollection<T> : IReadOnlyCollection<T>
    {
        #region Constructor

        internal AppendedReadOnlyCollection(IReadOnlyCollection<T> root, T item)
        {
            // Validate parameters.
            if (root == null) throw new ArgumentNullException(nameof(root));

            // Assign values.
            _enumerable = root.Append(item);
            Count = root.Count + 1;
        }

        #endregion

        #region Instance, read-only state.

        private readonly IEnumerable<T> _enumerable;

        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            // Return the enumerator with the last item concatenated.
            // Cannot use extension method, as it will call append on IReadOnlyCollection, recursing infinitely.
            return _enumerable.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            // Call the overload.
            return GetEnumerator();
        }

        #endregion

        #region Implementation of IReadOnlyCollection<out T>

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <returns>
        /// The number of elements in the collection. 
        /// </returns>
        public int Count { get; }

        #endregion
    }
}
