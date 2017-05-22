using System;
using System.Collections.Generic;

namespace Ofl.Reactive
{
    public class ConsumedObservable<T>
    {
        #region Constructors

        public ConsumedObservable(IReadOnlyList<T> observations)
        {
            // Validate parameters.
            Observations = observations ?? throw new ArgumentNullException(nameof(observations));
        }

        public ConsumedObservable(IReadOnlyList<T> observations, Exception exception)
            : this(observations)
        {
            // Validate parameters.
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        public ConsumedObservable(IReadOnlyList<T> observations, bool canceled)
            : this(observations)
        {
            // Assign values.
            Canceled = canceled;
        }

        #endregion

        #region Instance, read-only state.

        public bool Canceled { get; }

        public Exception Exception { get; }

        public IReadOnlyList<T> Observations { get; }

        #endregion
    }
}
