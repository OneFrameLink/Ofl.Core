using System;

namespace Ofl.Core.Parameters
{
    public static class ParameterExtensions
    {
        public static T IsNotNull<T>(this T parameter, string paramName)
            where T : class
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(paramName)) throw new ArgumentNullException(nameof(paramName));

            // Validate parameter.
            if (parameter == null) throw new ArgumentNullException(paramName);

            // Return parameter.
            return parameter;
        }

        public static string IsNotNull(this string parameter, string paramName)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(paramName)) throw new ArgumentNullException(nameof(paramName));

            // Validate parameter.
            if (string.IsNullOrWhiteSpace(parameter)) throw new ArgumentNullException(paramName);

            // Return parameter.
            return parameter;
        }

        public static T? IsNotNull<T>(this T? parameter, string paramName)
            where T : struct
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(paramName)) throw new ArgumentNullException(nameof(paramName));

            // Validate parameter.
            if (parameter == null) throw new ArgumentNullException(paramName);

            // Return parameter.
            return parameter;
        }
    }
}
