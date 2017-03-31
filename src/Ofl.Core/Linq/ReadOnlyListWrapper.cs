using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Core.Linq
{
    //////////////////////////////////////////////////
    /// 
    /// <author>Nicholas Paldino</author>
    /// <created>2012-08-31</created>
    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    /// 
    //////////////////////////////////////////////////
    internal sealed class ReadOnlyListWrapper<T> : IList<T>
    {
        #region Constructor

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>Creates a new instance of the <see cref="ReadOnlyListWrapper{T}"/>.</summary>
        /// <param name="list">The <see cref="IReadOnlyList{T}"/> that is
        /// to be wrapped.</param>
        /// 
        //////////////////////////////////////////////////
        internal ReadOnlyListWrapper(IReadOnlyList<T> list)
        {
            // Validate parameters.
            if (list == null) throw new ArgumentNullException(nameof(list));

            // Assign values.
            _list = list;
        }

        #endregion

        #region Read-only state and helpers.

        /// <summary>The <see cref="IReadOnlyList{T}"/> that the
        /// calls will be forwarded to.</summary>
        private readonly IReadOnlyList<T> _list;

        /// <summary>The error message for the <see cref="NotSupportedException"/>
        /// that is thrown when calls to methods that would mutate
        /// this instance are thrown.</summary>
        /// <remarks>While <see cref="NotSupportedException"/> is read-only, having
        /// an instance hang around for exceptional cases doesn't make sense.  There's no
        /// reason to keep an object on the heap for the life of the app domain
        /// for what is an exceptional situation which could very well tear down
        /// the app domain.  Just better to throw when needed, you're screwed already
        /// at that point.</remarks>
        private const string NotSupportedExceptionMessage = "This IList{T} implementation is read-only.";

        #endregion

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        /// 
        //////////////////////////////////////////////////
        public IEnumerator<T> GetEnumerator()
        {
            // Forward the call.
            return _list.GetEnumerator();
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        /// 
        //////////////////////////////////////////////////
        IEnumerator IEnumerable.GetEnumerator()
        {
            // Forward the call.
            return GetEnumerator();
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        /// 
        //////////////////////////////////////////////////
        public void Add(T item)
        {
            // Not supported.
            throw new NotSupportedException(NotSupportedExceptionMessage);
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        /// 
        //////////////////////////////////////////////////
        public void Clear()
        {
            // Not supported.
            throw new NotSupportedException(NotSupportedExceptionMessage);
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// 
        //////////////////////////////////////////////////
        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        /// 
        //////////////////////////////////////////////////
        public void CopyTo(T[] array, int arrayIndex)
        {
            // Pass along to be copied.
            Buffer.BlockCopy(this.ToArray(), 0, array, arrayIndex, Count);
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        /// 
        //////////////////////////////////////////////////
        public bool Remove(T item)
        {
            // Not supported.
            throw new NotSupportedException(NotSupportedExceptionMessage);
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// 
        //////////////////////////////////////////////////
        public int Count => _list.Count;

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        /// 
        //////////////////////////////////////////////////
        public bool IsReadOnly => true;

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// 
        //////////////////////////////////////////////////
        public int IndexOf(T item)
        {
            // The index.
            int index = 0;

            // Cycle through all the items to find.
            foreach (T t in this)
            {
                // If equal, return the index.
                if (EqualityComparer<T>.Default.Equals(t, item)) return index;

                // Increment the index.
                index++;
            }

            // Not found, return -1.
            return -1;
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        /// 
        //////////////////////////////////////////////////
        public void Insert(int index, T item)
        {
            // Not supported.
            throw new NotSupportedException(NotSupportedExceptionMessage);
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        /// 
        //////////////////////////////////////////////////
        public void RemoveAt(int index)
        {
            // Not supported.
            throw new NotSupportedException(NotSupportedExceptionMessage);
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        /// 
        //////////////////////////////////////////////////
        public T this[int index]
        {
            get { return _list[index]; }
            set { throw new NotSupportedException(NotSupportedExceptionMessage); }
        }
    }
}
