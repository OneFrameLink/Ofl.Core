using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Ofl.Core.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static IEnumerable<PropertyInfo> GetProperties<T>(
            this IEnumerable<Expression<Func<T, object>>> expressions)
        {
            // Validate parameters.
            if (expressions == null) throw new ArgumentNullException(nameof(expressions));

            // Call the private implementation.
            return expressions.GetPropertiesImplementation();
        }

        private static IEnumerable<PropertyInfo> GetPropertiesImplementation<T>(
            this IEnumerable<Expression<Func<T, object>>> expressions)
        {
            // Validate parameters.
            Debug.Assert(expressions != null);

            // Iterate.
            foreach (Expression<Func<T, object>> expression in expressions)
            {
                // The expression is a lambda.
                Debug.Assert(expression.NodeType == ExpressionType.Lambda);

                // If the expression is convert, then get the body.
                var memberExpression = (expression.Body.NodeType == ExpressionType.Convert ?
                    ((UnaryExpression) expression.Body).Operand : expression.Body) as MemberExpression;

                // PropertyInfo?
                var propertyInfo = memberExpression?.Member as PropertyInfo;

                // If not null, yield.
                if (propertyInfo != null) yield return propertyInfo;
            }
        }

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

            // Convert.
            Expression convertExpression = Expression.Convert(propertyExpression, typeof(TProperty));

            // Package in a lambda.
            return Expression.Lambda<Func<T, TProperty>>(convertExpression, parameterExpression);
        }
    }
}
