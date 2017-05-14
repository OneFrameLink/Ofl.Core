using System;

namespace Ofl.Core.Reactive
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2013-07-20</created>
    /// <summary>The result of calling <see cref="Core.Reactive.ObservableExtensions.ToTask{T}"/>.</summary>
    ///
    //////////////////////////////////////////////////
    public class ObservationResult
    {
        #region Constructors

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-07-20</created>
        /// <summary>Creates a new instance of the
        /// <see cref="ObservationResult"/> class.</summary>
        /// <remarks>This constructor is for cases where the observation
        /// ends with no errors.</remarks>
        /// <param name="observations">The number of observations that were made.</param>
        ///
        //////////////////////////////////////////////////
        public ObservationResult(int observations)
        {
            // Validate parameters.
            if (observations < 0) throw new ArgumentOutOfRangeException(nameof(observations),
                observations, "The observations parameter must be a non-negative number");

            // Assign values.
            Observations = observations;
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-07-20</created>
        /// <summary>Creates a new instance of the
        /// <see cref="ObservationResult"/> class.</summary>
        /// <remarks>Indicates that an observation was ended as the result
        /// of an exception being thrown..</remarks>
        /// <param name="observations">The number of observations that were made.</param>
        /// <param name="exception">The <see cref="Exception"/> that
        /// was thrown and ended the observation.</param>
        ///
        //////////////////////////////////////////////////
        public ObservationResult(int observations, Exception exception)
            : this(observations)
        {
            // Validate parameters.
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-07-20</created>
        /// <summary>Creates a new instance of the
        /// <see cref="ObservationResult"/> class.</summary>
        /// <remarks>Call to indicate that the observation ended as
        /// the result of a cancellation.</remarks>
        /// <param name="observations">The number of observations that were made.</param>
        /// <param name="canceled">True if a cancellation ended the
        /// observation, false otherwise.</param>
        ///
        //////////////////////////////////////////////////
        public ObservationResult(int observations, bool canceled)
            : this(observations)
        {
            // Assign values.
            IsCanceled = canceled;
        }

        #endregion

        #region Instance, read-only state.

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-07-20</created>
        /// <summary>Gets whether or not the observation was canceled.</summary>
        /// <value>True if the observation was canceled, false otherwise.</value>
        ///
        //////////////////////////////////////////////////
        public bool IsCanceled { get; }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-07-20</created>
        /// <summary>Gets the <see cref="Exception"/> that
        /// stoped the observation, if there was one.</summary>
        /// <value>The <see cref="Exception"/> that caused the observation
        /// to end, null otherwise.</value>
        ///
        //////////////////////////////////////////////////
        public Exception Exception { get; }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-07-20</created>
        /// <summary>Gets the number of successful observations were made.</summary>
        /// <value>The number of observations that were made.</value>
        ///
        //////////////////////////////////////////////////
        public int Observations { get; }

        #endregion
    }
}
