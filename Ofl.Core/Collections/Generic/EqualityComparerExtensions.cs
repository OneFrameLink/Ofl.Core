using System;
using System.Collections.Generic;

namespace Ofl.Core.Collections.Generic
{
    public static class EqualityComparerExtensions
    {
        public static IEqualityComparer<T> CreateFromProperty<T, TProperty>(Func<T, TProperty> getter)
        {
            // Validate parameters.
            if (getter == null) throw new ArgumentNullException(nameof(getter));

            // Call the overload.
            return Create<T>((x, y) => getter(x).Equals(getter(y)), t => getter(t).GetHashCode());
        }

        public static IEqualityComparer<T> CreateFromIEquatableProperty<T, TProperty>(Func<T, TProperty> getter)
            where TProperty : IEquatable<TProperty>
        {
            // Validate parameters.
            if (getter == null) throw new ArgumentNullException(nameof(getter));

            // Call the overload.
            return Create<T>((x, y) => getter(x).Equals(getter(y)), t => getter(t).GetHashCode());
        }

        public static IEqualityComparer<T> Create<T>(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            // Validate parameters.
            if (equals == null) throw new ArgumentNullException(nameof(@equals));
            if (getHashCode == null) throw new ArgumentNullException(nameof(getHashCode));

            // Return the delegated one.
            return new DelegatedEqualityComparer<T>(equals, getHashCode);
        }
    }
}
