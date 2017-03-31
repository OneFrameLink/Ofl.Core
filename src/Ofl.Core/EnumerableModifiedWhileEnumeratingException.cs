using System;

namespace Ofl.Core
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2011-04-03</created>
    /// <summary>Thrown when an enumeration is changed
    /// while enumerating through it.</summary>
    ///
    //////////////////////////////////////////////////
    public sealed class EnumerableModifiedWhileEnumeratingException : Exception
    {
        /// <summary>The message that is exposed by the <see cref="Exception.Message"/>
        /// property.</summary>
        private const string ExceptionMessage = "The enumerable was modified after the enumerator was created.";

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2011-04-03</created>
        /// <summary>Creates a new instance of the
        /// <see cref="EnumerableModifiedWhileEnumeratingException"/>.</summary>
        ///
        //////////////////////////////////////////////////
        public EnumerableModifiedWhileEnumeratingException() : base(ExceptionMessage)
        { }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2011-04-03</created>
        /// <summary>Creates a new instance of the
        /// <see cref="EnumerableModifiedWhileEnumeratingException"/>.</summary>
        /// <param name="message">The message to assign to the
        /// <see cref="Exception.Message"/> property.</param>
        ///
        //////////////////////////////////////////////////
        public EnumerableModifiedWhileEnumeratingException(string message)
            : base(message)
        { }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2011-04-03</created>
        /// <summary>Creates a new instance of the
        /// <see cref="EnumerableModifiedWhileEnumeratingException"/>.</summary>
        /// <param name="innerException">The <see cref="Exception"/>
        /// that will be assigned to the
        /// <see cref="Exception.InnerException"/>.</param>
        ///
        //////////////////////////////////////////////////
        public EnumerableModifiedWhileEnumeratingException(Exception innerException)
            : base(ExceptionMessage, innerException)
        { }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2011-04-03</created>
        /// <summary>Creates a new instance of the
        /// <see cref="EnumerableModifiedWhileEnumeratingException"/>.</summary>
        /// <param name="message">The value to be assigned to the
        /// <see cref="Exception.Message"/> property.</param>
        /// <param name="innerException">The <see cref="Exception"/>
        /// that will be assigned to the
        /// <see cref="Exception.InnerException"/>.</param>
        ///
        //////////////////////////////////////////////////
        public EnumerableModifiedWhileEnumeratingException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
