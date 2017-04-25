using System;

namespace Ofl.Core.Serialization.Newtonsoft.Json.Shims
{
    internal static class TypeExtensions
    {
        internal static TypeShim ToTypeShim(this Type type)
        {
            // Validate parameters.
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Copy matching properties.
            return type.CopySharedProperties<Type, TypeShim>();
        }
    }
}
