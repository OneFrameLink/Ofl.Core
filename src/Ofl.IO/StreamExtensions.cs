using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ofl.IO
{
    public static class StreamExtensions
    {
        public static readonly int DefaultToAsyncEnumerableBufferSize = 4096;

        public static IAsyncEnumerable<byte> ToAsyncEnumerable(this Stream stream) =>
            stream.ToAsyncEnumerable(DefaultToAsyncEnumerableBufferSize);

        public static IAsyncEnumerable<byte> ToAsyncEnumerable(this Stream stream, int bufferSize) =>
            AsyncEnumerable.CreateEnumerable(() => new StreamAsyncEnumerator(stream, bufferSize));
    }
}
