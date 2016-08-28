using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ofl.Core.Linq;

namespace Ofl.Core.Collections.Generic
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2011-10-31</created>
    /// <summary>Exposes extension methods for the <see cref="IDictionary{TKey,TValue}"/>
    /// interface.</summary>
    ///
    //////////////////////////////////////////////////
    public static class DictionaryExtensions
    {
        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-07-28</created>
        /// <summary>Gets a value from a
        /// <see cref="IDictionary{TKey,TValue}"/>
        /// where <typeparamref name="TValue"/> is
        /// a struct, returning null if
        /// it is not found.</summary>
        /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/>
        /// to get the value from.</param>
        /// <param name="key">The key to use to look up a value in
        /// the <paramref name="dictionary"/>.</param>
        /// <returns>The value looked up in the
        /// <paramref name="dictionary"/> using 
        /// the <paramref name="key"/> or null
        /// if <paramref name="key"/> does not exist.</returns>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value returned.</typeparam>
        ///
        //////////////////////////////////////////////////
        public static TValue? TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : struct
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            // The found value, if any.
            TValue value;

            // Was this found?  If not, return null.
            if (!dictionary.TryGetValue(key, out value)) return null;

            // Return the value.
            return value;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, 
            TKey key, Func<TKey, TValue> adder)
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (CoreExtensions.IsNull(key)) throw new ArgumentNullException(nameof(key));
            if (adder == null) throw new ArgumentNullException(nameof(adder));

            // The value.
            TValue value;

            // Try and get the value.
            if (dictionary.TryGetValue(key, out value)) return value;

            // Add the item.
            dictionary.Add(key, value = adder(key));                

            // Return the value.
            return value;
        }

        public static async Task<TValue> GetOrAddAsync<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, Func<TKey, Task<TValue>> adder)
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (CoreExtensions.IsNull(key)) throw new ArgumentNullException(nameof(key));

            // The value.
            TValue value;

            // Try and get the value.
            if (dictionary.TryGetValue(key, out value)) return value;

            // Add the item.
            dictionary.Add(key, value = await adder(key).ConfigureAwait(false));

            // Return the value.
            return value;
        }

        public static bool TryGetValueCascading<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, out TValue value, params IDictionary<TKey, TValue>[] fallbacks)
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (fallbacks == null) throw new ArgumentNullException(nameof(fallbacks));

            // Set output.
            value = default(TValue);

            // Cycle through the dictionaries.
            foreach (IDictionary<TKey, TValue> d in EnumerableExtensions.From(dictionary).Concat(fallbacks))
            {
                // Try and get the value, if found, return true.
                if (d.TryGetValue(key, out value)) return true;
            }

            // Not found.
            return false;
        }

        public static bool TryGetValueCascading<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary,
            TKey key, out TValue value, params IReadOnlyDictionary<TKey, TValue>[] fallbacks)
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (fallbacks == null) throw new ArgumentNullException(nameof(fallbacks));

            // Set output.
            value = default(TValue);

            // Cycle through the dictionaries.
            foreach (IReadOnlyDictionary<TKey, TValue> d in EnumerableExtensions.From(dictionary).Concat(fallbacks))
            {
                // Try and get the value, if found, return true.
                if (d.TryGetValue(key, out value)) return true;
            }

            // Not found.
            return false;
        }

        public static TValue? TryGetValueCascading<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, params IDictionary<TKey, TValue>[] fallbacks)
            where TValue : struct
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (fallbacks == null) throw new ArgumentNullException(nameof(fallbacks));

            // The value.
            TValue value = default(TValue);

            // Cycle through the dictionaries.
            if (EnumerableExtensions.From(dictionary).Concat(fallbacks).Any(d => d.TryGetValue(key, out value)))
            {
                // Return the value.
                return value;
            }

            // Not found.
            return null;
        }

        public static TValue? TryGetValueCascading<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary,
            TKey key, params IReadOnlyDictionary<TKey, TValue>[] fallbacks)
            where TValue : struct
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (fallbacks == null) throw new ArgumentNullException(nameof(fallbacks));

            // The value.
            TValue value = default(TValue);

            // Cycle through the dictionaries.
            if (EnumerableExtensions.From(dictionary).Concat(fallbacks).Any(d => d.TryGetValue(key, out value)))
            {
                // Return the value.
                return value;
            }

            // Not found.
            return null;
        }

        public static TValue? TryGetValue<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : struct
        {
            // Validate parameters.
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            // The value.
            TValue value;

            // If the value is not found, return null.
            if (!dictionary.TryGetValue(key, out value)) return null;

            // Return the value.
            return value;
        }
    }
}
