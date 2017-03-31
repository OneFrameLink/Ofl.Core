using System;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Core.Hashing
{
    //////////////////////////////////////////////////
    /// 
    /// <author>Nicholas Paldino</author>
    /// <created>2012-08-22</created>
    /// <summary>Extension methods for the implementation of
    /// the FNV hash algorithm.</summary>
    /// <remarks>
    /// <para>All constants are as per: http://isthe.com/chongo/tech/comp/fnv/ </para>
    /// <para>Also, uses the FNV-1a algorithm, as per the suggestion
    /// at http://isthe.com/chongo/tech/comp/fnv/ </para>
    /// <para>
    /// As with all classes in the
    /// <see cref="Ofl.Hashing"/> namespace, this is
    /// needed so that consistent hashes may be produced
    /// (.NET does not guarantee that hashes produced
    /// in different application domains will be the same, as
    /// per Eric Lippert: http://stackoverflow.com/a/6114944/50776).
    /// </para>
    /// </remarks>
    /// 
    //////////////////////////////////////////////////
    public static class FowlerNollVoHashExtensions
    {
        #region Static, read-only state.

        /// <summary>The prime used for hashing 32-bit values.</summary>
        private const uint ThirtyTwoBitPrime = 16777619;

        /// <summary>The value used to offset subsequent offsets
        /// of 32-bit values.</summary>
        private const uint ThirtyTwoBitOffsetBasis = 2166136261;

        /// <summary>The prime used for hashing 64-bit values.</summary>
        private const ulong SixtyFourBitPrime = 1099511628211;

        /// <summary>The value used to offset subsequent offsets
        /// of 64-bit values.</summary>
        private const ulong SixtyFourBitOffsetBasis = 14695981039346656037;

        #endregion

        #region Hash functions.

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-22</created>
        /// <summary>Given a sequence of integers (hash codes) computes
        /// the composite hash code.</summary>
        /// <param name="hashCodes">The array of hash codes that are to be
        /// computed.</param>
        /// <returns>The composite hash code.</returns>
        /// 
        //////////////////////////////////////////////////
        public static int Compute32BitFnvCompositeHashCode(params int[] hashCodes)
        {
            // Call the overload.
            return hashCodes.Compute32BitFnvCompositeHashCode();
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-22</created>
        /// <summary>Given a sequence of integers (hash codes) computes
        /// the composite hash code.</summary>
        /// <param name="hashCodes">The array of hash codes that are to be
        /// computed.</param>
        /// <returns>The composite hash code.</returns>
        /// 
        //////////////////////////////////////////////////
        public static int Compute32BitFnvCompositeHashCode(this IEnumerable<int> hashCodes)
        {
            // Validate the parameters.
            if (hashCodes == null) throw new ArgumentNullException(nameof(hashCodes));

            // Convert each argument into a byte array, convert singular result back.
            return BitConverter.ToInt32(
                hashCodes.SelectMany(BitConverter.GetBytes).Compute32BitFnvHash().ToArray(), 0);
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-22</created>
        /// <summary>Computes the 32-bit hash for a sequence of bytes.</summary>
        /// <param name="source">The sequence of bytes to hash.</param>
        /// <returns>The 32-bit hash of the sequence of items that were passed in.</returns>
        /// 
        //////////////////////////////////////////////////
        public static IEnumerable<byte> Compute32BitFnvHash(this IEnumerable<byte> source)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Aggregate.
            return BitConverter.GetBytes(source.Aggregate(ThirtyTwoBitOffsetBasis, (current, b) => (current ^ b) * ThirtyTwoBitPrime));
        }

        //////////////////////////////////////////////////
        /// 
        /// <author>Nicholas Paldino</author>
        /// <created>2012-08-22</created>
        /// <summary>Computes the 64-bit hash for a sequence of bytes.</summary>
        /// <param name="source">The sequence of bytes to hash.</param>
        /// <returns>The 64-bit hash of the sequence of items that were passed in.</returns>
        /// 
        //////////////////////////////////////////////////
        public static IEnumerable<byte> Compute64BitFnvHash(this IEnumerable<byte> source)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Aggregate.
            return BitConverter.GetBytes(source.Aggregate(SixtyFourBitOffsetBasis, (current, b) => (current ^ b) * SixtyFourBitPrime));
        }

        #endregion
    }
}
