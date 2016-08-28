using System;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Ofl.Core.Threading.Tasks
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2014-04-12</created>
    /// <summary>Extensions for the <see cref="Task"/>
    /// and <see cref="Task{T}"/> classes.</summary>
    ///
    //////////////////////////////////////////////////
    public static class TaskExtensions
    {
        #region Static, read-only state.

        /// <summary>A <see cref="Task"/> that is always in the completed state.</summary>
        public static readonly Task CompletedTask = Task.FromResult((object) null);

        #endregion

        public static void RunInContext(this Task task)
        {
            // Validate parameters.
            if (task == null) throw new ArgumentNullException(nameof(task));

            // Run the context.
            AsyncContext.Run(() => task);
        }

        public static T RunInContext<T>(this Task<T> task)
        {
            // Validate parameters.
            if (task == null) throw new ArgumentNullException(nameof(task));

            // Run the task and return.
            return AsyncContext.Run(() => task);
        }

        public static Task<T> WrapInTask<T>(this T value)
        {
            // Return.
            return Task.FromResult(value);
        }
    }
}
