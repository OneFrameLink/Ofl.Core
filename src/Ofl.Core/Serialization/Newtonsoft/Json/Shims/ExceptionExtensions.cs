using System;

namespace Ofl.Core.Serialization.Newtonsoft.Json.Shims
{
    internal static class ExceptionExtensions
    {
        internal static ExceptionShim ToExceptionShim(this Exception exception)
        {
            // Validate parameters.

            // Special case, if exception is null, return null.
            return exception?.CopySharedProperties<Exception, ExceptionShim>();
        }
    }
}
