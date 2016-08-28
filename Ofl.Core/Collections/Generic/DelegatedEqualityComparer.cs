using System;
using System.Collections.Generic;

namespace Ofl.Core.Collections.Generic
{
    internal class DelegatedEqualityComparer<T> : IEqualityComparer<T>
    {
        #region Constructor

        internal DelegatedEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            // Validate parameters.
            if (equals == null) throw new ArgumentNullException(nameof(equals));
            if (equals == null) throw new ArgumentNullException(nameof(getHashCode));

            // Assign values.
            _equals = equals;
            _getHashCode = getHashCode;
        }

        #endregion

        #region Instance, read-only state.

        private readonly Func<T, T, bool> _equals;
        private readonly Func<T, int> _getHashCode;

        #endregion

        public bool Equals(T x, T y)
        {
            // What's null.
            bool xNull = CoreExtensions.IsNull(x);
            bool yNull = CoreExtensions.IsNull(y);

            // If both are null, or one is null and the other is not, then return those.
            if (xNull && yNull) return true;
            if (xNull || yNull) return false;

            // Call equals.
            return _equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            // Call get hash code.
            return _getHashCode(obj);
        }
    }
}
