using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Ofl.Core.Linq
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2011-04-03</created>
    /// <summary>Contains extensions for LINQ operations.</summary>
    ///
    //////////////////////////////////////////////////
    public static class EnumerableExtensions
    {
        #region WrapWithList

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-31</created>
        /// <summary>Returns an immutable <see cref="IList{T}"/>
        /// implementation that wraps an <see cref="IReadOnlyList{T}"/>
        /// implementation.</summary>
        /// <param name="list">The <see cref="IReadOnlyList{T}"/>
        /// to wrap.</param>
        /// <returns>The <see cref="IList{T}"/> implementation
        /// that will throw a <see cref="NotSupportedException"/>
        /// on any methods that may mutate the list.</returns>
        ///
        //////////////////////////////////////////////////
        public static IList<T> WrapInList<T>(this IReadOnlyList<T> list)
        {
            // Validate parameters.
            if (list == null) throw new ArgumentNullException(nameof(list));

            // Wrap and return.
            return new ReadOnlyListWrapper<T>(list);
        }

        #endregion

        #region HasExactly

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-09-04</created>
        /// <summary>Will validate that a sequence has exactly
        /// <paramref name="count"/> number of elements.</summary>
        /// <remarks>This extension is only effective if the list is iterated
        /// to the point past <paramref name="count"/>; if the user breaks out, it
        /// will not catch the condition.</remarks>
        /// <param name="source">The sequence of items to take count of.</param>
        /// <param name="count">The number of items to make sure the
        /// sequence has.</param>
        /// <typeparam name="T">The type of the items in <paramref name="source"/>.</typeparam>
        /// <returns>The sequence, but one which will throw an exception if there are
        /// not <paramref name="count"/> elements in it.</returns>
        /// 
        //////////////////////////////////////////////////
        public static IEnumerable<T> HasExactly<T>(this IEnumerable<T> source, int count)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "The count parameter must be a non-negative number.");

            // Call the implementation.
            return source.HasExactlyImplementation(count);
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-09-04</created>
        /// <summary>Will validate that a sequence has exactly
        /// <paramref name="count"/> number of elements.</summary>
        /// <remarks>In the event of a break before the fail case is hit, this
        /// will not throw by design.  If you are breaking then there's a situation
        /// that caused you to exit the full processing of the loop and
        /// this shouldn't interfere with that.</remarks>
        /// <param name="source">The sequence of items to take count of.</param>
        /// <param name="count">The number of items to make sure the
        /// sequence has.</param>
        /// <typeparam name="T">The type of the items in <paramref name="source"/>.</typeparam>
        /// <returns>The sequence, but one which will throw an exception if there are
        /// not <paramref name="count"/> elements in it.</returns>
        /// 
        //////////////////////////////////////////////////
        private static IEnumerable<T> HasExactlyImplementation<T>(this IEnumerable<T> source, int count)
        {
            // Validate parameters.
            Debug.Assert(source != null);
            Debug.Assert(count >= 0);

            // The original count.
            int originalCount = count;

            // Iterate through the items.
            foreach (var t in source)
            {
                // Decrement the count.  If less than 0, then throw an exception.
                if (--count < 0) throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, "The sequence must have exactly {0} elements", count));

                // Yield the item.
                yield return t;
            }

            // If the count is not 0, throw the same exception.
            if (count != 0) throw new InvalidOperationException(
                string.Format(CultureInfo.CurrentCulture, "The sequence must have exactly {0} elements", originalCount));

        }

        #endregion

        #region FillGapsWith

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-09-04</created>
        /// <summary>Fills the gaps between items in a sequence with
        /// a <paramref name="fill"/>.</summary>
        /// <param name="source">The sequence of items to fill the gaps
        /// with.</param>
        /// <param name="fill">The instance of <typeparamref name="T"/>
        /// to insert between each consecutive item.</param>
        /// <typeparam name="T">The type of the instances in
        /// the sequence.</typeparam>
        /// <returns>The sequence, with <paramref name="fill"/>
        /// inserted between each consecutive item pair.</returns>
        /// 
        //////////////////////////////////////////////////
        // TODO: Think of more semantic name.
        public static IEnumerable<T> FillGapsWith<T>(this IEnumerable<T> source, T fill)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Call the implemenation.
            return source.FillGapsWithImplementation(fill);
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-09-04</created>
        /// <summary>Fills the gaps between items in a sequence with
        /// a <paramref name="fill"/>.</summary>
        /// <param name="source">The sequence of items to fill the gaps
        /// with.</param>
        /// <param name="fill">The instance of <typeparamref name="T"/>
        /// to insert between each consecutive item.</param>
        /// <typeparam name="T">The type of the instances in
        /// the sequence.</typeparam>
        /// <returns>The sequence, with <paramref name="fill"/>
        /// inserted between each consecutive item pair.</returns>
        /// 
        //////////////////////////////////////////////////
        private static IEnumerable<T> FillGapsWithImplementation<T>(this IEnumerable<T> source, T fill)
        {
            // Validate parameters.
            Debug.Assert(source != null);

            // Has the first item been yielded?
            bool firstYielded = false;

            // Cycle through everything.
            foreach (T item in source)
            {
                // If the first item has been yielded, then
                // yield the fill.
                if (firstYielded)
                {
                    // Yield the fill.
                    yield return fill;
                }
                else
                {
                    // The first has been yielded.
                    // NOTE: This is in the event of a continue.
                    firstYielded = true;
                }

                // Yield the item.
                yield return item;
            }
        }

        #endregion

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
        public static IEnumerable<IEnumerable<T>> Shingle<T>(this IEnumerable<T> source, int count)
        {
            // Call the overload, assume false.
            return source.Shingle(count, false);
        }

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
            bool yieldRemainder)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), count, "The count parameter must be a positive value.");

            // Call the implemenation.
            return source.ShingleImplementation(count, yieldRemainder);
        }

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
            Debug.Assert(source != null);
            Debug.Assert(count > 0);

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

        #region TryGetValue

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-09-04</created>
        /// <summary>Gets a value in a
        /// <paramref name="dictionary"/> given a
        /// <paramref name="key"/>, optionally returning
        /// null if the key doesn't have a value.</summary>
        /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/>
        /// to perform the lookup in.</param>
        /// <param name="key">The instance of
        /// <typeparamref name="TKey"/> to look up in the
        /// <paramref name="dictionary"/>.</param>
        /// <returns>The instance of <typeparamref name="TValue"/>, or
        /// null if the <paramref name="key"/> doesn't exist in the
        /// dictionary.</returns>
        /// 
        //////////////////////////////////////////////////
        public static TValue? TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : struct
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            // The value.
            TValue value;

            // Try and get the value.  If it succeeds, return the value.
            if (dictionary.TryGetValue(key, out value)) return value;

            // Return null.
            return null;
        }

        #endregion

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2011-05-29</created>
        /// <summary>Gets a sequence of sequences of
        /// <typeparamref name="T"/>, grouped in batches of
        /// <paramref name="size"/>.</summary>
        /// <param name="source">The original sequence of instances
        /// of <typeparamref name="T"/>.</param>
        /// <param name="size">The number of items max to contain in each group.</param>
        /// <returns>A sequence of sequences of
        /// <typeparamref name="T"/>, grouped in batches of
        /// <paramref name="size"/>.</returns>
        /// <typeparam name="T">The type of the items to be grouped into
        /// segments.</typeparam>
        ///
        //////////////////////////////////////////////////
        public static IEnumerable<IEnumerable<T>> GroupEvery<T>(this IEnumerable<T> source,
            int size)
        {
            // Validate the parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size <= 0)
            {
                // Must be positive.
                throw new ArgumentOutOfRangeException(nameof(size), size, 
                    "The size parameter must be a positive number.");
            }

            // The list of items, max size.
            IList<T> g = new List<T>(size);

            // Cycle through the source.
            foreach (T item in source)
            {
                // Add the item.
                g.Add(item);

                // Does the list have a number of items
                // equal to size?
                if (g.Count == size)
                {
                    // Yield the list, yield as an enumerable
                    // to obfuscate.
                    yield return g.Select(i => i);

                    // Cannot maintain the same list, calls
                    // to Single() and the like will fail as
                    // this will continue executing and clear
                    // what is in g, causing the items
                    // to be wiped.  Set to a new list.
                    g = new List<T>(size);
                }
            }

            // If the list has items, yield it.
            if (g.Count > 0) yield return g.Select(i => i);
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-01-05</created>
        /// <summary>Converts a sequence of <see cref="KeyValuePair{TKey,TValue}"/>
        /// instances into a dictionary.</summary>
        /// <param name="source">The sequence of
        /// <see cref="KeyValuePair{TKey,TValue}"/>
        /// to materialize into an <see cref="IDictionary{TKey,TValue}"/>
        /// implementation.</param>
        /// <remarks>This uses the <see cref="IEqualityComparer{T}"/>
        /// implementation returned by <see cref="EqualityComparer{T}.Default"/>.</remarks>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>An <see cref="IDictionary{TKey,TValue}"/> implementation
        /// formed from the <paramref name="source"/> sequence.</returns>
        ///
        //////////////////////////////////////////////////
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            // Call the overload, use the default comparer.
            return source.ToDictionary(EqualityComparer<TKey>.Default);
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-01-05</created>
        /// <summary>Converts a sequence of <see cref="KeyValuePair{TKey,TValue}"/>
        /// instances into a dictionary.</summary>
        /// <param name="source">The sequence of
        /// <see cref="KeyValuePair{TKey,TValue}"/>
        /// to materialize into an <see cref="IDictionary{TKey,TValue}"/>
        /// implementation.</param>
        /// <param name="equalityComparer">An <see cref="IEqualityComparer{T}"/>
        /// implementation which will be used for comparison of
        /// instances of the <typeparamref name="TKey"/> type
        /// parameter.</param>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>An <see cref="IDictionary{TKey,TValue}"/> implementation
        /// formed from the <paramref name="source"/> sequence.</returns>
        ///
        //////////////////////////////////////////////////
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source,
            IEqualityComparer<TKey> equalityComparer)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (equalityComparer == null) throw new ArgumentNullException(nameof(equalityComparer));

            // Convert.
            return source.ToDictionary(p => p.Key, p => p.Value, equalityComparer);
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2011-09-17</created>
        /// <summary>Converts a sequence of instances
        /// of <typeparamref name="T"/> to a
        /// <see cref="ReadOnlyCollection{T}"/>, sniffing
        /// out an <see cref="IList{T}"/> implementation
        /// to pass to the constructor.</summary>
        /// <param name="source">The sequence of
        /// <typeparamref name="T"/> to materialize
        /// into a <see cref="ReadOnlyCollection{T}"/>.</param>
        /// <returns>A <see cref="ReadOnlyCollection{T}"/>
        /// based on the <paramref name="source"/>
        /// sequence.</returns>
        /// <typeparam name="T">The type of the sequence
        /// to materialize into a
        /// <see cref="ReadOnlyCollection{T}"/>.</typeparam>
        ///
        //////////////////////////////////////////////////
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Create the read only collection.
            // Do not sniff for a list, as the
            // ReadOnlyCollection<T> is only a wrapper, so
            // this needs to be copied in case a reference to the
            // original list is held on to and mutated.
            return source.ToList().WrapInReadOnlyCollection();
        }

        public static ReadOnlyCollection<T> WrapInReadOnlyCollection<T>(this IList<T> source)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Wrap.
            return new ReadOnlyCollection<T>(source);
        }

            //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2011-10-31</created>
        /// <summary>Converts a sequence to
        /// an indexed sequence, attaching the
        /// index in the sequence to the
        /// item.</summary>
        /// <param name="source">The sequence to generate
        /// the indexed sequence from.</param>
        /// <returns>A sequence of <see cref="KeyValuePair{TKey,TValue}"/>
        /// where the key is the index of the item in the sequence and
        /// the value is the original value from the sequence.</returns>
        ///
        //////////////////////////////////////////////////
        public static IEnumerable<KeyValuePair<int, T>> ToIndexedSequence<T>(this IEnumerable<T> source)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Return the sequence mapped to the index.
            return source.Select((t, i) => new KeyValuePair<int, T>(i, t));
        }

        public static IEnumerable<KeyValuePair<int, T>?> ToNullableIndexedSequence<T>(this IEnumerable<T> source)
        {
            // Return the sequence, nulled out.
            return source.ToIndexedSequence().Select(p => new KeyValuePair<int, T>?(p));
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2011-11-20</created>
        /// <summary>Drains an <see cref="IEnumerable{T}"/>
        /// of all it's items.</summary>
        /// <param name="source">The sequence drain.</param>
        ///
        //////////////////////////////////////////////////
        public static void Drain<T>(this IEnumerable<T> source)
        {
            // Call Do with an empty action.
            source.Do(t => { });
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Call the overload.
            return source.ToHashSet(t => t);
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-06-24</created>
        /// <summary>Creates a <see cref="HashSet{T}"/>
        /// from a sequence.</summary>
        /// <param name="source">The sequence to
        /// convert to a <see cref="HashSet{T}"/>.</param>
        /// <param name="keySelector">The <see cref="Func{T1,TResult}"/>
        /// which takes an element of type <typeparamref name="T"/>
        /// and converts it to a the type of the key,
        /// <typeparamref name="TKey"/>.</param>
        /// <typeparam name="T">The type of the items in the
        /// <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TKey">The type that is to
        /// be the key in the <see cref="HashSet{T}"/>
        /// that is returned.</typeparam>
        /// <returns>A <see cref="HashSet{T}"/> where
        /// the elements are of type
        /// <typeparamref name="TKey"/>.</returns>
        ///
        //////////////////////////////////////////////////
        public static HashSet<TKey> ToHashSet<T, TKey>(this IEnumerable<T> source, 
            Func<T, TKey> keySelector)
        {
            // Validate parameters.
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            // Just return the hashset.
            return new HashSet<TKey>(source.Select(keySelector));
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-06-24</created>
        /// <summary>Creates a <see cref="HashSet{T}"/>
        /// from a sequence using a specific
        /// <see cref="IEqualityComparer{T}"/>
        /// implementation to compare the keys.</summary>
        /// <param name="source">The sequence to
        /// convert to a <see cref="HashSet{T}"/>.</param>
        /// <param name="keySelector">The <see cref="Func{T1,TResult}"/>
        /// which takes an element of type <typeparamref name="T"/>
        /// and converts it to a the type of the key,
        /// <typeparamref name="TKey"/>.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/>
        /// instance which will be used to compare the
        /// <typeparamref name="TKey"/> instances
        /// contained in the returned <see cref="HashSet{T}"/>.</param>
        /// <typeparam name="T">The type of the items in the
        /// <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TKey">The type that is to
        /// be the key in the <see cref="HashSet{T}"/>
        /// that is returned.</typeparam>
        /// <returns>A <see cref="HashSet{T}"/> where
        /// the elements are of type
        /// <typeparamref name="TKey"/>.</returns>
        ///
        //////////////////////////////////////////////////
        public static HashSet<TKey> ToHashSet<T, TKey>(this IEnumerable<T> source,
            Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            // Validate parameters.
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // Just return the hashset.
            return new HashSet<TKey>(source.Select(keySelector), comparer);
        }

        public static IEnumerable<T> From<T>(params T[] items)
        {
            // Validate parameters.
            if (items == null) throw new ArgumentNullException(nameof(items));

            // Just return items.
            return items;
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-12-23</created>
        /// <summary>Creates a sequence from a single
        /// item.</summary>
        /// <remarks>This is an immutable sequence that can't
        /// be altered by type sniffing.  Combined with
        /// <see cref="Append{T}"/>, this allows a
        /// sequence to be generated that can be
        /// passed outside of the caller's control
        /// and ensures immutability.</remarks>
        /// <param name="item">The instance of
        /// <typeparamref name="T"/> that is the root
        /// of the sequence to be returned.</param>
        /// <returns>A single-item sequence that consists
        /// of the <paramref name="item"/> passed in.</returns>
        ///
        //////////////////////////////////////////////////
        public static IEnumerable<T> From<T>(T item)
        {
            // Yield the item.
            yield return item;
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-01-19</created>
        /// <summary>Takes a sequence and chunks it
        /// into smaller sub-sequences.</summary>
        /// <remarks>This only allows for forward-only, read-only
        /// enumeration of the sequences and sub-sequences
        /// If other operations are needed, integrate
        /// Sam Saffron's answer on SO:
        /// http://stackoverflow.com/a/10425490/50776
        /// </remarks>
        /// <param name="source">The sequence to be chunked.</param>
        /// <param name="size">The size of the chunks.</param>
        /// <returns>A sequence of sequences, each with the
        /// specified <paramref name="size"/>, but possibly
        /// with a shorter chunk at the end.</returns>
        ///
        //////////////////////////////////////////////////
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source,
            int size)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size),
                "The size parameter must be a positive value.");

            // Call the internal implementation.
            return source.ChunkImplementation(size);
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-01-19</created>
        /// <summary>Internal implementation of
        /// <see cref="Chunk{T}"/>.</summary>
        /// <param name="source">The sequence to be chunked.</param>
        /// <param name="size">The size of the chunks.</param>
        /// <returns>A sequence of sequences, each with the
        /// specified <paramref name="size"/>, but possibly
        /// with a shorter chunk at the end.</returns>
        ///
        //////////////////////////////////////////////////
        private static IEnumerable<IEnumerable<T>> ChunkImplementation<T>(
            this IEnumerable<T> source, int size)
        {
            // Validate parameters.
            Debug.Assert(source != null);
            Debug.Assert(size > 0);

            // Get the enumerator.  Dispose of when done.
            using (IEnumerator<T> enumerator = source.GetEnumerator())
            do
            {
                // Move to the next element.  If there's nothing left
                // then get out.
                if (!enumerator.MoveNext()) yield break;

                // Return the chunked sequence.
                yield return ChunkSequence(enumerator, size);
            } while (true);
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-01-19</created>
        /// <summary>Called by
        /// <see cref="ChunkImplementation{T}"/>, it
        /// is responsible for yielding the individual
        /// chunks.</summary>
        /// <param name="enumerator">The <see cref="IEnumerator{T}"/>
        /// that is to be yielded.</param>
        /// <param name="size">The size of the chunk.</param>
        /// <returns>The chunk that has no more than
        /// <paramref name="size"/> elements in it.</returns>
        ///
        //////////////////////////////////////////////////
        private static IEnumerable<T> ChunkSequence<T>(IEnumerator<T> enumerator,
            int size)
        {
            // Validate parameters.
            Debug.Assert(enumerator != null);
            Debug.Assert(size > 0);

            // The count.
            int count = 0;

            // There is at least one item.  Yield and then continue.
            do
            {
                // Yield the item.
                yield return enumerator.Current;
            } while (++count < size && enumerator.MoveNext());
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2014-06-17</created>
        /// <summary>Takes a sequence and pushes it into a
        /// <see cref="LinkedList{T}"/>.</summary>
        /// <param name="source">The sequence of items to
        /// push into the returned <see cref="LinkedList{T}"/>.</param>
        /// <returns>The <see cref="LinkedList{T}"/> that is
        /// created from the <paramref name="source"/>
        /// sequence.</returns>
        /// <typeparam name="T">The type of the sequence.</typeparam>
        ///
        //////////////////////////////////////////////////
        public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> source)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Add to a linked list and return.
            return new LinkedList<T>(source);
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2014-07-01</created>
        /// <summary>Performs an action on each item in a sequence.</summary>
        /// <param name="source">The sequence of items to
        /// perform an action on.</param>
        /// <param name="action">The <see cref="Action{T}"/>
        /// to perform on each item in the <paramref name="source"/>.</param>
        /// <returns>The sequence, with the <paramref name="action"/>
        /// performed on each item.</returns>
        /// <typeparam name="T">The type of the sequence.</typeparam>
        ///
        //////////////////////////////////////////////////
        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            // Call the implementation.
            return source.DoImplementation(action);
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2014-07-01</created>
        /// <summary>Performs an action on each item in a sequence.</summary>
        /// <param name="source">The sequence of items to
        /// perform an action on.</param>
        /// <param name="action">The <see cref="Action{T}"/>
        /// to perform on each item in the <paramref name="source"/>.</param>
        /// <returns>The sequence, with the <paramref name="action"/>
        /// performed on each item.</returns>
        /// <typeparam name="T">The type of the sequence.</typeparam>
        ///
        //////////////////////////////////////////////////
        private static IEnumerable<T> DoImplementation<T>(this IEnumerable<T> source, Action<T> action)
        {
            // Validate parameters.
            Debug.Assert(source != null);
            Debug.Assert(action != null);

            // Cycle through the items.
            foreach (T item in source)
            {
                // Perform the action.
                action(item);

                // Yield the item.
                yield return item;
            }
        }

        public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Call the overload.
            return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary(
                p => p.Key, p => p.Value, EqualityComparer<TKey>.Default));
        }

        public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey> comparer)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // Create and return.
            return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary(
                p => p.Key, p => p.Value, comparer));
        }

        public static IReadOnlyDictionary<TKey, TSource> ToReadOnlyDictionary<TSource, TKey>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            // Call the overload.
            return source.ToReadOnlyDictionary(keySelector, EqualityComparer<TKey>.Default);
        }

        public static IReadOnlyDictionary<TKey, TSource> ToReadOnlyDictionary<TSource, TKey>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // Call the overload.
            return source.ToReadOnlyDictionary(keySelector, t => t, comparer);
        }

        public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TSource, TKey, TValue>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));

            // Call the overload.
            return source.ToReadOnlyDictionary(keySelector, elementSelector, EqualityComparer<TKey>.Default);

        }

        public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TSource, TKey, TValue>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, IEqualityComparer<TKey> comparer)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // Create and return.
            return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary(keySelector, elementSelector, comparer));
        }

        public static IReadOnlyCollection<T> EmptyReadOnlyCollection<T>()
        {
            // Return an empty collection.
            return new ReadOnlyCollection<T>(new T[0]);
        }

        public static IEnumerable<T> Safeguard<T>(this IEnumerable<T> source)
        {
            // If source is null, return empty, otherwise, source.
            return source ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<T> AddRange<T>(this IEnumerable<T> source, IEnumerable<T> collection)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            // Sniff.  List<T> first.
            var listSource = source as List<T>;

            // If not null, add range.
            if (listSource != null)
            {
                // Add the range and return.
                listSource.AddRange(collection);
                return source;
            }

            // Check for collection.
            var collectionSource = source as ICollection<T>;

            // If the collection is null, then just concat the enumerables.
            if (collectionSource == null) return source.Concat(collection);

            // Add all the items to the collection.
            collection.Do(t => collectionSource.Add(t));

            // Return the source.
            return source;
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Append the item.  Concat from.
            return source.Concat(From(item));
        }
    }
}
