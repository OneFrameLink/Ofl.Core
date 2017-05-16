using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Stuff<T>(this IEnumerable<T> source, IEnumerable<T> stuffing)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (stuffing == null) throw new ArgumentNullException(nameof(stuffing));

            // Call the implemenation.
            return source.StuffImplementation(stuffing);
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-09-04</created>
        /// <summary>Fills the gaps between items in a sequence with
        /// a <paramref name="stuffing"/>.</summary>
        /// <param name="source">The sequence of items to fill the gaps
        /// with.</param>
        /// <param name="stuffing">The instance of <typeparamref name="T"/>
        /// to insert between each consecutive item.</param>
        /// <typeparam name="T">The type of the instances in
        /// the sequence.</typeparam>
        /// <returns>The sequence, with <paramref name="stuffing"/>
        /// inserted between each consecutive item pair.</returns>
        /// 
        //////////////////////////////////////////////////
        private static IEnumerable<T> StuffImplementation<T>(this IEnumerable<T> source, IEnumerable<T> stuffing)
        {
            // Validate parameters.
            Debug.Assert(source != null);

            // The materialized stuffing.  Get here.
            IList<T> materializedStuffing = stuffing.ToList();

            // Get the enumerator.
            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                // Move to the next item.  If it fails, return.
                if (!enumerator.MoveNext()) yield break;

                // Yield the current item.
                yield return enumerator.Current;

                // Cycle while true.
                while (true)
                {
                    // Move to the next item.  If there's nothing to move to
                    // then break.
                    if (!enumerator.MoveNext()) break;

                    // There's an item to yield.
                    // Yield the stuffing first.
                    foreach (T stuff in materializedStuffing)
                        yield return stuff;

                    // Yield the current item.
                    yield return enumerator.Current;
                }
            }
        }
    }
}
