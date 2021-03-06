﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Core.Net.Http
{
    internal static class AsyncEnumeratorExtensions
    {
        private const byte Cr = 13;
        private const byte Lf = 10;

        private static SlidingWindowMask<byte> CreateCrLfSlidingWindowMask()
        {
            // Create the sliding window mask.
            return new SlidingWindowMask<byte>(new [] { Cr, Lf });
        }

        internal static async Task<ArraySegment<byte>> ReadLineBytesAsync(this IAsyncEnumerator<byte> enumerator,
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));

            // Create the sliding window mask.
            SlidingWindowMask<byte> window = CreateCrLfSlidingWindowMask();

            // The memory stream.
            using (var ms = new MemoryStream())
            {
                // While there are bytes.
                while (await enumerator.MoveNext(cancellationToken).ConfigureAwait(false) && !window.Slide(enumerator.Current))
                    // Push the byte.
                    // TODO: If WriteByteAsync pops up, move to that.
                    ms.WriteByte(enumerator.Current);

                // If the window is masked, remove the CRLF.
                // NOTE: if the slide has been called, then the last character (LF) has not been written, account
                // for that here.
                if (window.Masked) ms.SetLength(ms.Length - 1);

                // The buffer.
                ArraySegment<byte> buffer;

                // Return the buffer.
                if (!ms.TryGetBuffer(out buffer)) throw new InvalidOperationException("An error occured while trying to get the buffer from a MemoryStream.");

                // Return the buffer.
                return buffer;
            }
        }

        internal static async Task<string> ReadLineAsync(this IAsyncEnumerator<byte> enumerator,
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));

            // The buffer.
            ArraySegment<byte> buffer = await enumerator.ReadLineBytesAsync(cancellationToken).ConfigureAwait(false);

            // Decode.
            return Encoding.ASCII.GetString(buffer.Array, buffer.Offset, buffer.Count);
        }

        internal static async Task<Match> MatchLineAsync(this IAsyncEnumerator<byte> enumerator, Regex regex,
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));
            if (regex == null) throw new ArgumentNullException(nameof(regex));

            // The line.
            string line = await enumerator.ReadLineAsync(cancellationToken).ConfigureAwait(false);

            // Match.
            Match match = regex.Match(line);

            // If not successful, throw an exception.
            if (!match.Success) throw new InvalidOperationException($"The regular expression did not match the line. (regex: { regex }, line: { line }");

            // Return the match.
            return match;
        }
    }
}
