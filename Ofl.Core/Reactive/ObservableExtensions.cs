using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Core.Reactive
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2013-07-20</created>
    /// <summary>Observations for <see cref="IObservable{T}"/>
    /// implementations.</summary>
    ///
    //////////////////////////////////////////////////
    public static class ObservableExtensions
    {
        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-07-20</created>
        /// <summary>Subscribes to an <paramref name="observable"/> returning
        /// a result that can be awaited on and contains the results
        /// of the observation.</summary>
        /// <param name="observable">The <see cref="IObservable{T}"/> to safely
        /// subscribe and wait to.</param>
        /// <param name="onNext">The action to take when an item is
        /// observed from the <paramref name="observable"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>
        /// that is used to cancel the observation, if necessary.</param>
        /// <returns>A <see cref="ObservationResult"/> that indicates whether or
        /// not the observable was cancelled, there was an exception, etc.</returns>
        ///
        //////////////////////////////////////////////////
        // TODO: Switch to Materialize and Notification<T>, might
        // be able to remove registration to cancellation token
        // and pass it to subscription.
        public static async Task<ObservationResult> ToTask<T>(this IObservable<T> observable, 
            Action<T> onNext, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (observable == null) throw new ArgumentNullException(nameof(observable));
            if (onNext == null) throw new ArgumentNullException(nameof(onNext));

            // The task continuation source.
            var tcs = new TaskCompletionSource<Exception>();

            // The number of items observed.
            int observations = 0;

            // Was this cancelled?
            bool cancelled = false;

            // Subscribe to the observable.
            using (var subscription = observable.Subscribe(i => { onNext(i); Interlocked.Increment(ref observations); }, 
                e => tcs.TrySetResult(e), () => tcs.TrySetResult(null)))
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

                // Was the task cancelled?
                if (cancelled || task.IsCanceled) return new ObservationResult(observations, true);

                // Is there a result? If so, then this was an exception.
                if (task.Result != null) return new ObservationResult(observations, task.Result);

                // If the task was completed but no exception, then it ended naturally.
                return new ObservationResult(observations);
            }
        }
    }
}
