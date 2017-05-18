using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Ofl.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, object>> CreateGetPropertyLambdaExpression<T>(this PropertyInfo propertyInfo)
        {
            // Validate parameters.
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            // Call the overload.
            return propertyInfo.CreateGetPropertyLambdaExpression<T, object>();
        }

        public static Expression<Func<T, TProperty>> CreateGetPropertyLambdaExpression<T, TProperty>(this PropertyInfo propertyInfo)
        {
            // Validate parameters.
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            // Create the parameter expression.
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "o");

            // Access the property.
            Expression propertyExpression = Expression.Property(parameterExpression, propertyInfo);

            // Convert if the type of the property is not the type of the property info.
            Expression convertExpression = Expression.Convert(propertyExpression, typeof(TProperty));

            // Package in a lambda.
            return Expression.Lambda<Func<T, TProperty>>(convertExpression, parameterExpression);
        }
    }
}
