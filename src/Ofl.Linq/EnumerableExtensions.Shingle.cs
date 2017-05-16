using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        #region Shingle

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-09-04</created>
        /// <summary>Takes a sequence of one type and
        /// outputs overlapping shingles of another type.</summary>
        /// <param name="source">The sequence of items to shingle.</param>
        /// <param name="count">The number of items that each shingle
        /// should contain.  The last group of shingles may not be this
        /// size, because there aren't enough elements left.</param>
        /// <typeparam name="T">The type of the sequence to shingle</typeparam>
        /// <returns>The sequence of shingles (each shingle represented by
        /// another sequence)</returns>
        /// 
        //////////////////////////////////////////////////
        public static IEnumerable<IEnumerable<T>> Shingle<T>(this IEnumerable<T> source, int count) =>
            source.Shingle(count, false);

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-09-04</created>
        /// <summary>Takes a sequence of one type and
        /// outputs overlapping shingles of another type.</summary>
        /// <param name="source">The sequence of items to shingle.</param>
        /// <param name="count">The number of items that each shingle
        /// should contain.  The last group of shingles may not be this
        /// size, because there aren't enough elements left.</param>
        /// <param name="yieldRemainder">If true, then the remaining
        /// shingles are yielded, even though they might not have
        /// <paramref name="count"/> members.</param>
        /// <typeparam name="T">The type of the sequence to shingle</typeparam>
        /// <returns>The sequence of shingles (each shingle represented by
        /// another sequence)</returns>
        /// 
        //////////////////////////////////////////////////
        public static IEnumerable<IEnumerable<T>> Shingle<T>(this IEnumerable<T> source, int count,
            bool yieldRemainder) => source.ShingleImplementation(count, yieldRemainder);

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-09-04</created>
        /// <summary>Takes a sequence of one type and
        /// outputs overlapping shingles of another type.</summary>
        /// <param name="source">The sequence of items to shingle.</param>
        /// <param name="count">The number of items that each shingle
        /// should contain.  The last group of shingles may not be this
        /// size, because there aren't enough elements left.</param>
        /// <param name="yieldRemainder">If true, then the remaining
        /// shingles are yielded, even though they might not have
        /// <paramref name="count"/> members.</param>
        /// <typeparam name="T">The type of the sequence to shingle</typeparam>
        /// <returns>The sequence of shingles (each shingle represented by
        /// another sequence)</returns>
        /// 
        //////////////////////////////////////////////////
        private static IEnumerable<IEnumerable<T>> ShingleImplementation<T>(this IEnumerable<T> source, int count,
            bool yieldRemainder)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), count, $"The { nameof(count) } parameter must be a positive value.");

            // The stack of shingles.
            var shingles = new Queue<IList<T>>();

            // Cycle through the token stream.
            // TODO: Do not store in list, cycle through enumerator.
            foreach (T t in source)
            {
                // Enqueue the item.
                shingles.Enqueue(new List<T>(count));

                // Was the shingle dequeued?
                bool dequeue = false;

                // Cycle through the shingles.
                foreach (IList<T> shingle in shingles)
                {
                    // Add the word.
                    shingle.Add(t);

                    // If the shingle is the size of the
                    // max, return it.
                    if (shingle.Count == count)
                    {
                        // The dequeue is not true.
                        Debug.Assert(!dequeue);

                        // Yield the item.
                        // TODO: Wrap in read only collection.
                        yield return shingle.Select(w => w);

                        // Set the dequeue flag to true.
                        dequeue = true;
                    }
                }

                // Dequeue if necessary.
                if (dequeue) shingles.Dequeue();
            }

            // If there are shingles left and yielding them, yield those.
            if (yieldRemainder) foreach (IList<T> shingle in shingles) yield return shingle.Select(w => w);
        }

        #endregion
    }
}
