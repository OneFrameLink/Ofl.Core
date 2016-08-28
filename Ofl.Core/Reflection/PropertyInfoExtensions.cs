using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ofl.Core.Linq;

namespace Ofl.Core.Reflection
{
    public static class PropertyInfoExtensions
    {
        public static IReadOnlyCollection<PropertyInfo> Remove<T, TResult>(this IReadOnlyCollection<PropertyInfo> properties,
            T t, Expression<Func<T, TResult>> propertyToRemove)
        {
            // Validate parameters.
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            if (propertyToRemove == null) throw new ArgumentNullException(nameof(propertyToRemove));

            // Get the type.
            Type type = typeof(T);

            // Is it a member expression?
            var member = propertyToRemove.Body as MemberExpression;

            // If null, then it's not a property.
            if (member == null)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Expression \"{0}\" is not a property.",
                    propertyToRemove));

            // Get the property info.
            var propInfo = member.Member as PropertyInfo;

            // Not a property?
            if (propInfo == null)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Expression \"{0}\" is not a property.",
                    propertyToRemove));

            // If not part of the type.
            if (!type.GetTypeInfo().IsAssignableFrom(type))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    "Expresion \"{0}\" refers to a property that is not from type {1}.", propertyToRemove, type));

            // Remove the property info.
            return properties.Where(p => p.Name != propInfo.Name).ToReadOnlyCollection();
        }
    }
}
