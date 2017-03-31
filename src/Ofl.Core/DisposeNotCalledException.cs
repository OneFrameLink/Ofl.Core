using System;

namespace Ofl.Core
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2011-04-03</created>
    /// <summary>Thrown from a finalizer because
    /// the implementation of <see cref="IDisposable.Dispose"/>
    /// was not called.</summary>
    ///
    //////////////////////////////////////////////////
    public sealed class DisposeNotCalledException : Exception
    {
        /// <summary>The message that is exposed by the <see cref="Exception.Message"/>
        /// property.</summary>
        private const string ExceptionMessage = "The finalizer should not be called; call IDisposable.Dispose instead.";

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2011-04-03</created>
        /// <summary>Creates a new instance of the
        /// <see cref="DisposeNotCalledException"/>.</summary>
        ///
        //////////////////////////////////////////////////
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IDisposable")]
        public DisposeNotCalledException() : base(ExceptionMessage)
        { }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2011-04-03</created>
        /// <summary>Creates a new instance of the
        /// <see cref="DisposeNotCalledException"/>.</summary>
        /// <param name="message">The value of the
        /// <see cref="Exception.Message"/> property.</param>
        ///
        //////////////////////////////////////////////////
        public DisposeNotCalledException(string message)
            : base(message)
        { }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2011-04-03</created>
        /// <summary>Creates a new instance of the
        /// <see cref="DisposeNotCalledException"/>.</summary>
        /// <param name="innerException">The <see cref="Exception"/>
        /// that will be assigned to the
        /// <see cref="Exception.InnerException"/>.</param>
        /// <param name="message">The value of the
        /// <see cref="Exception.Message"/> property.</param>
        ///
        //////////////////////////////////////////////////
        public DisposeNotCalledException(string message, Exception innerException) 
            : base(message, innerException)
        { }
    }
}
