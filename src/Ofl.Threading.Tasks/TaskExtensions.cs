using System;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Ofl.Threading.Tasks
{
    public static class TaskExtensions
    {
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
    }
}
