using System;

namespace Ofl.Core
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2013-01-21</created>
    /// <summary>Extensions for the <see cref="Exception"/>
    /// class.</summary>
    ///
    //////////////////////////////////////////////////
    public static class ExceptionExtensions
    {
        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-01-21</created>
        /// <summary>Takes an <paramref name="exception"/>
        /// and flattens it if it is an
        /// <see cref="AggregateException"/> with
        /// one exception.</summary>
        /// <param name="exception">The <see cref="Exception"/>
        /// to try and flatten.</param>
        /// <returns>The <paramref name="exception"/> if it
        /// is not an <see cref="AggregateException"/>, or
        /// an <see cref="AggregateException"/> if it is
        /// and has more than one exception in the
        /// <see cref="AggregateException.InnerExceptions"/>
        /// property, or the <see cref="Exception.InnerException"/>
        /// if there is only one.</returns>
        ///
        //////////////////////////////////////////////////
        public static Exception FlattenException(this Exception exception)
        {
            // Validate parameters.
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            // Is this an aggregate exception.
            var aggregate = exception as AggregateException;

            // If the aggregate is null, return the exception.
            if (aggregate == null) return exception;

            // An aggregate.  Flatten.
            aggregate = aggregate.Flatten();

            // If there is only one exception, then
            // return the exception.
            if (aggregate.InnerExceptions.Count > 1) return aggregate;

            // Return the inner exception.
            return aggregate.InnerException;
        }
    }
}
