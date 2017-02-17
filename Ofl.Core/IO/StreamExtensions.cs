using System.Collections.Generic;
using System.IO;
using Ofl.Core.Collections.Generic;

namespace Ofl.Core.IO
{
    public static class StreamExtensions
    {
        public static readonly int DefaultToAsyncEnumerableBufferSize = 4096;

        public static IAsyncEnumerable<byte> ToAsyncEnumerable(this Stream stream)
        {
            // Call the overload with the default buffer size.
            return stream.ToAsyncEnumerable(DefaultToAsyncEnumerableBufferSize);
        }

        public static IAsyncEnumerable<byte> ToAsyncEnumerable(this Stream stream, int bufferSize)
        {
            // Return the implementation.
            return new AsyncEnumerable<byte>(() => new StreamAsyncEnumerator(stream, bufferSize));            
        }
    }
}
