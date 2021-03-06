﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ofl.Core.Hashing;

namespace Ofl.Core.Collections.Generic
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2013-11-17</created>
    /// <summary>A <see cref="IEqualityComparer{T}"/> implementation
    /// that will compare sequences of instances of
    /// <typeparamref name="T"/>.</summary>
    /// <typeparam name="T">The type of the sequences to compare.</typeparam>
    ///
    //////////////////////////////////////////////////
    public class EnumerableEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        #region Constructor

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-11-17</created>
        /// <summary>Creates a new instance of the <see cref="EnumerableEqualityComparer{T}"/>
        /// class.</summary>
        /// <remarks>This uses the default comparer for instances
        /// of <typeparamref name="T"/> returned by
        /// <see cref="EqualityComparer{T}.Default"/>.</remarks>
        ///
        //////////////////////////////////////////////////
        public EnumerableEqualityComparer() : this(EqualityComparer<T>.Default)
        { }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-11-17</created>
        /// <summary>Creates a new instance of the <see cref="EnumerableEqualityComparer{T}"/>
        /// class.</summary>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/>
        /// used to compare the individual instances of <typeparamref name="T"/>.</param>
        ///
        //////////////////////////////////////////////////
        public EnumerableEqualityComparer(IEqualityComparer<T> equalityComparer)
        {
            // Validate parameters.
            if (equalityComparer == null) throw new ArgumentNullException(nameof(equalityComparer));

            // Assign values.
            _equalityComparer = equalityComparer;
        }

        #endregion

        #region Instance, read-only state.

        /// <summary>The <see cref="IEqualityComparer{T}"/> used to compare
        /// instances of <typeparamref name="T"/>.</summary>
        private readonly IEqualityComparer<T> _equalityComparer;

        #endregion

        #region Implementation of IEqualityComparer<in IEnumerable<T>>

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-11-17</created>
        /// <summary>Determines whether the specified objects are equal.</summary>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        /// <param name="x">The first object of type <typeparamref name="T"/> to compare.</param>
        /// <param name="y">The second object of type <typeparamref name="T"/> to compare.</param>
        ///
        //////////////////////////////////////////////////
        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            // If both are null, return true.
            if (x == null && y == null) return true;

            // If one is null and the other is not, return false.
            if (x == null || y == null) return false;

            // Both are not null.  Get the enumerator for one.
            using (IEnumerator<T> xEnumerator = x.GetEnumerator())
            {
                // Cycle through y.
                foreach (T yItem in y)
                {
                    // Advance the enumerator. If it fails, not equal.
                    if (!xEnumerator.MoveNext()) return false;

                    // Are the objects equal?  If not, get out.
                    if (!_equalityComparer.Equals(xEnumerator.Current, yItem)) return false;
                }

                // Can the enumerator move to the next item?
                // This means x has more items than y.
                if (xEnumerator.MoveNext()) return false;
            }

            // They are equal, return true.
            return true;
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-11-17</created>
        /// <summary>Returns a hash code for the specified object.</summary>
        /// <returns>A hash code for the specified object.</returns>
        /// <param name="obj">The instance of <typeparamref name="T"/> for which a hash code is to be returned.</param>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type 
        /// and <paramref name="obj"/> is null.</exception>
        ///
        //////////////////////////////////////////////////
        public int GetHashCode(IEnumerable<T> obj)
        {
            // Validate parameters.
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            // Get the hash codes for the items in the enumerable, and then compose a hash of those.
            // If the instance is null, assume 0.
            return obj.Select(o => CoreExtensions.IsNull(o) ? 0 : o.GetHashCode()).Compute32BitFnvCompositeHashCode();
        }

        #endregion
    }
}
