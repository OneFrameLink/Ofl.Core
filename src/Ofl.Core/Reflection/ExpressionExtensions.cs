using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Ofl.Core.Reflection
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2014-02-15</created>
    /// <summary>Extensions for working with <see cref="Expression{TDelegate}"/>
    /// instances.</summary>
    ///
    //////////////////////////////////////////////////
    public static class ExpressionExtensions
    {
        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2014-02-15</created>
        /// <summary>Gets the <see cref="PropertyInfo"/> given
        /// an <paramref name="expression"/> that references
        /// the property..</summary>
        /// <param name="source">The instance of <typeparamref name="TSource"/>
        /// that the propery that the <see cref="PropertyInfo"/> is to
        /// be retreived from.</param>
        /// <param name="expression">The <see cref="Expression{TDelegate}"/>
        /// that exposes accessing the property to get the
        /// <see cref="PropertyInfo"/> for.</param>
        /// <returns>The <see cref="PropertyInfo"/> for the property
        /// in the <paramref name="expression"/>.</returns>
        /// <typeparam name="TSource">The object that is the source
        /// of the property.</typeparam>
        /// <typeparam name="TProperty">The type of the property to
        /// get the property info for.</typeparam>
        ///
        //////////////////////////////////////////////////
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(TSource source,
            Expression<Func<TSource, TProperty>> expression)
        {
            // Validate parameters.
            if (CoreExtensions.IsNull(source)) throw new ArgumentNullException(nameof(source));
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            // Return the property info.
            return expression.GetPropertyInfoImplementation();
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2014-02-15</created>
        /// <summary>Gets the <see cref="PropertyInfo"/> given
        /// an <paramref name="expression"/> that references
        /// the property.</summary>
        /// <param name="expression">The <see cref="Expression{TDelegate}"/>
        /// that exposes accessing the property to get the
        /// <see cref="PropertyInfo"/> for.</param>
        /// <returns>The <see cref="PropertyInfo"/> for the property
        /// in the <paramref name="expression"/>.</returns>
        /// <typeparam name="TProperty">The type of the property to
        /// get the property info for.</typeparam>
        ///
        //////////////////////////////////////////////////
        public static PropertyInfo GetPropertyInfo<TProperty>(this Expression<Func<TProperty>> expression)
        {
            // Validate parameters.
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            // Return the property info.
            return expression.GetPropertyInfoImplementation();
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2014-02-15</created>
        /// <summary>Gets the <see cref="PropertyInfo"/> given
        /// an <paramref name="expression"/> that references
        /// the property.</summary>
        /// <param name="expression">The <see cref="MemberExpression"/>
        /// that needs to be checked for the property.</param>
        /// <returns>The <see cref="PropertyInfo"/> that is exposed
        /// by the <paramref name="expression"/>.</returns>
        /// <typeparam name="TDelegate">The type of the <see cref="Expression{T}"/>.</typeparam>
        ///
        //////////////////////////////////////////////////
        private static PropertyInfo GetPropertyInfoImplementation<TDelegate>(this Expression<TDelegate> expression)
        {
            // Validate parametrs.
            Debug.Assert(expression != null);

            // The member expression.
            var member = expression.Body as MemberExpression;

            // The exception message.
            string expressionNotPropertyExceptionMessage = string.Format(CultureInfo.CurrentCulture,
                "The expression parameter ({0}) is not a property expression.", expression);

            // If not a member expression, throw an exception.
            if (member == null)
                throw new ArgumentException(expressionNotPropertyExceptionMessage, nameof(expression));

            // Get the property info.
            var propertyInfo = member.Member as PropertyInfo;

            // If it is null, throw an exception.
            if (propertyInfo == null)
                throw new ArgumentException(expressionNotPropertyExceptionMessage, nameof(expression));

            // Return the property info.
            return propertyInfo;
        }
    }
}
