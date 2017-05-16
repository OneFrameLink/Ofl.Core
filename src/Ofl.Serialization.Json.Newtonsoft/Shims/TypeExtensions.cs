using System;
using Ofl.Cloning;

namespace Ofl.Serialization.Json.Newtonsoft.Shims
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
