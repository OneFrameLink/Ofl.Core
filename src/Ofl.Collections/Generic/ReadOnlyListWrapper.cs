using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ofl.Linq;

namespace Ofl.Collections.Generic
{
    internal sealed class ReadOnlyListWrapper<T> : IList<T>
    {
        #region Constructor

        internal ReadOnlyListWrapper(IReadOnlyList<T> list)
        {
            // Validate parameters.
            _list = list ?? throw new ArgumentNullException(nameof(list));
        }

        #endregion

        #region Helpers

        private object ThrowNotSupportedException(string member)
        {
            // Validate parameters.
            if (member == null) throw new ArgumentNullException(nameof(member));

            // Throw.
            throw new NotSupportedException($"{ GetType().FullName } does not support the { member } member.");
        }

        #endregion

        #region Read-only state and helpers.

        private readonly IReadOnlyList<T> _list;

        #endregion

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item) => ThrowNotSupportedException(nameof(Add));

        public void Clear() => ThrowNotSupportedException(nameof(Clear));

        public bool Contains(T item) => _list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => Buffer.BlockCopy(this.ToArray(), 0, array, arrayIndex, Count);

        public bool Remove(T item) => (bool) ThrowNotSupportedException(nameof(Remove));

        public int Count => _list.Count;

        public bool IsReadOnly => true;

        public int IndexOf(T item) => _list.IndexOf(item) ?? -1;

        public void Insert(int index, T item) => ThrowNotSupportedException(nameof(Insert));

        public void RemoveAt(int index) => ThrowNotSupportedException(nameof(RemoveAt));

        public T this[int index]
        {
            get => _list[index];
            set => ThrowNotSupportedException("[] setter");
        }
    }
}
