using Microsoft.Extensions.Configuration;
using System;

namespace Ofl.Core.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T BindToNewInstanceOf<T>(this IConfigurationSection configuration)
            where T : new()
        {
            // Validate parameters.
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            // Create a new instance of T.
            var t = new T();

            // Bind.
            configuration.Bind(t);

            // Return T.
            return t;
        }
    }
}
