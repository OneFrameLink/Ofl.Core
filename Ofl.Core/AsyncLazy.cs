using System;
using System.Threading.Tasks;

namespace Ofl.Core
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2013-04-06</created>
    /// <summary>Allows for the asynchronous lazy-loading
    /// of a resource.</summary>
    /// <remarks>
    /// <para>Obtained from: http://blogs.msdn.com/b/pfxteam/archive/2011/01/15/10116210.aspx.</para>
    /// <para><code>GetAwaiter</code> from above implementation doesn't seem to work.</para>
    /// </remarks>
    /// <typeparam name="T">The type to asynchronously lazy-load.</typeparam>
    ///
    //////////////////////////////////////////////////
    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        #region Constructor

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2013-04-06</created>
        /// <summary>Creates a new instance of <see cref="AsyncLazy{T}"/>.</summary>
        /// <param name="taskFactory">The <see cref="Func{TResult}"/>
        /// that returns a <see cref="Task{TResult}"/> which gets
        /// the result lazily.</param>
        ///
        //////////////////////////////////////////////////
        public AsyncLazy(Func<Task<T>> taskFactory) :
            base(() => Task.Factory.StartNew(taskFactory).Unwrap()) { }

        #endregion
    }
}
