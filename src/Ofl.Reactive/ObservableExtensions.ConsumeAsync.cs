using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Collections.Generic;

namespace Ofl.Reactive
{
    public static partial class ObservableExtensions
    {
        public static async Task<ConsumedObservable<T>> ConsumeAsync<T>(this IObservable<T> observable, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (observable == null) throw new ArgumentNullException(nameof(observable));

            // The task continuation source.
            var tcs = new TaskCompletionSource<Exception>();

            // The items collected.
            IList<T> collected = new List<T>();

            // Was this cancelled?
            bool cancelled = false;

            // Subscribe to the observable.
            using (var subscription = observable.Subscribe(i => { collected.Add(i); }, e => tcs.TrySetResult(e), () => tcs.TrySetResult(null)))
            {
                // The task.
                Task<Exception> task;

                // The copy of the subscription.
                var subscriptionCopy = subscription;

                // Register with the cancellation token.  Dispose when cancelled.
                using (cancellationToken.Register(() => {
                        subscriptionCopy.Dispose();
                        cancelled = true;
                        tcs.TrySetCanceled();
                }))
                {
                    // Get the task.
                    task = tcs.Task;

                    // Wait on the task.
                    await task.ConfigureAwait(false);
                }

                // Wrap the observations.
                ReadOnlyCollection<T> observations = collected.WrapInReadOnlyCollection();

                // Was the task cancelled?
                if (cancelled || task.IsCanceled) return new ConsumedObservable<T>(observations, true);

                // Is there a result? If so, then this was an exception.
                if (task.Result != null) return new ConsumedObservable<T>(observations, task.Result);

                // If the task was completed but no exception, then it ended naturally.
                return new ConsumedObservable<T>(observations);
            }
        }
    }
}
