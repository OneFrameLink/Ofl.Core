using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.IO
{
    public class StreamWithState<T> : Stream
    {
        #region Constructor

        public StreamWithState(Stream stream, T state)
        {
            // Validate parameters.
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));

            // Assign values.
            State = state;
        }

        #endregion

        #region Instance, read-only state.

        private readonly Stream _stream;

        public T State { get; }

        #endregion

        #region Overrides of Stream

        public override void Flush() => _stream.Flush();

        public override long Seek(long offset, SeekOrigin origin) =>_stream.Seek(offset, origin);

        public override void SetLength(long value) =>_stream.SetLength(value);

        public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);

        public override void Write(byte[] buffer, int offset, int count) =>_stream.Write(buffer, offset, count);

        public override bool CanRead => _stream.CanRead;

        public override bool CanSeek => _stream.CanSeek;

        public override bool CanWrite => _stream.CanWrite;

        public override long Length => _stream.Length;

        public override long Position { get => _stream.Position; set => _stream.Position = value; }

        public override bool CanTimeout => _stream.CanTimeout;

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) => _stream.CopyToAsync(destination, bufferSize, cancellationToken);

        public override Task FlushAsync(CancellationToken cancellationToken) => _stream.FlushAsync(cancellationToken);

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => _stream.ReadAsync(buffer, offset, count, cancellationToken);

        public override int ReadByte() => _stream.ReadByte();

        public override int ReadTimeout { get => _stream.ReadTimeout; set => _stream.ReadTimeout = value; }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => _stream.WriteAsync(buffer, offset, count, cancellationToken);

        public override void WriteByte(byte value) => _stream.WriteByte(value);

        public override int WriteTimeout { get => _stream.WriteTimeout; set => _stream.WriteTimeout = value; }

        protected override void Dispose(bool disposing)
        {
            // Call the base.
            base.Dispose(disposing);

            // Dispose of unmanaged resources.

            // If not disposing, get out.
            if (!disposing) return;

            // Dispose the state, if it is disposable.
            using (State as IDisposable) { }
        }

        #endregion
    }
}
