using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ofl.Reflection
{
    public static class PropertyInfoExtensions
    {
        public static IEnumerable<PropertyInfo> Remove<T, TResult>(this IEnumerable<PropertyInfo> properties,
            T t, Expression<Func<T, TResult>> propertyToRemove)
        {
            // Validate parameters.
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            if (propertyToRemove == null) throw new ArgumentNullException(nameof(propertyToRemove));

            // Get the type.
            TypeInfo type = typeof(T).GetTypeInfo();

            // Is it a member expression?
            var member = propertyToRemove.Body as MemberExpression;

            // If null, then it's not a property.
            if (member == null)
                throw new ArgumentException($"Expression \"{ propertyToRemove }\" is not a property.");

            // Get the property info.
            var propInfo = member.Member as PropertyInfo;

            // Not a property?
            if (propInfo == null)
                throw new ArgumentException($"Expression \"{ propertyToRemove }\" is not a property.");

            // If not part of the type.
            if (!type.IsAssignableFrom(type))
                throw new ArgumentException($"Expresion \"{ propertyToRemove }\" refers to a property that is not from type { type }.");

            // Remove the property info.
            return properties.Where(p => p.Name != propInfo.Name);
        }
    }
}
