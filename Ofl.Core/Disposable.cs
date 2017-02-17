using System;
using System.Reactive.Disposables;

namespace Ofl.Core
{
    public abstract class Disposable : IDisposable
    {
        #region Instance, read-only state.

        protected readonly CompositeDisposable Disposables = new CompositeDisposable();

        protected bool Disposed { get; private set; }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            // Call the overload, remove from finalization.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // If disposed already, get out.
            if (Disposed) return;

            // If disposing of resources that this is referencing, do
            // do now.
            if (disposing)
                using (Disposables) { }

            // Dispose of unmanaged resources.

            // Call the base.

            // Disposed.
            Disposed = true;
        }

        ~Disposable()
        {
            // Call the overload.
            Dispose(false);
        }

        #endregion
    }
}
