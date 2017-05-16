using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ofl.Linq
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2011-04-03</created>
    /// <summary>Contains extensions for LINQ operations.</summary>
    ///
    //////////////////////////////////////////////////
    public static partial class EnumerableExtensions
    {
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
