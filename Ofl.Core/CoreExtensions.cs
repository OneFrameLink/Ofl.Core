using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ofl.Core.Linq;

namespace Ofl.Core
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2012-09-01</created>
    /// <summary>Utility methods.</summary>
    ///
    //////////////////////////////////////////////////
    public static class CoreExtensions
    {
        #region IsNull and helpers.

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-09-01</created>
        /// <summary>A helper class which contains the implementation
        /// for the <see cref="CoreExtensions.IsNull{T}"/>
        /// method, it performs the check in the constructor and
        /// then creates a predicate.</summary>
        /// <typeparam name="T">The type of the instance
        /// that null is checked for.</typeparam>
        ///
        //////////////////////////////////////////////////
        private static class InherentlyNullableIsNullHelper<T>
        {
            /// <summary>The <see cref="Predicate{T}"/>
            /// that indicates whether an instance
            /// of <typeparamref name="T"/> is null
            /// or not, without boxing.</summary>
            internal static readonly Predicate<T> IsNullImplementation = CreateIsNullImplementation();

            //////////////////////////////////////////////////
            ///
            /// <author>Nicholas Paldino</author>
            /// <created>2012-09-01</created>
            /// <summary>Creates the <see cref="Predicate{T}"/>
            /// that determines if an instance of <typeparamref name="T"/>
            /// is null or not.</summary>
            /// <returns>A <see cref="Predicate{T}"/> that will
            /// evaluate an instance of <typeparamref name="T"/>
            /// and return true if null, false otherwise.</returns>
            ///
            //////////////////////////////////////////////////
            private static Predicate<T> CreateIsNullImplementation()
            {
                // If the default of the type is not null then
                // return false all the time.
// ReSharper disable RedundantCast
                if ((object) default(T) != null) return item => false;
// ReSharper restore RedundantCast

                // The type of T.
                Type type = typeof(T);

                // Create the delegate here.
                ParameterExpression t = Expression.Parameter(type, "t");

                // Compare with null.
                BinaryExpression comparison = Expression.Equal(t, Expression.Convert(Expression.Constant(null, type), type), false, null);

                // Create the lambda, compile and return.
                return Expression.Lambda<Predicate<T>>(comparison, t).Compile();
            }
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-09-01</created>
        /// <summary>Determines if an instance of <typeparamref name="T"/>
        /// is null or not.</summary>
        /// <remarks>This is done without casing <paramref name="value"/>
        /// to an object for null comparison.  That leads to a lot
        /// of boxing in the case of value types, which there's no
        /// need for.</remarks>
        /// <param name="value">The instance of <typeparamref name="T"/>
        /// to check for null.</param>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <returns>True if <paramref name="value"/> is null, false otherwise.</returns>
        ///
        //////////////////////////////////////////////////
        // TODO: What to do with INullable implementations?
        public static bool IsNull<T>(T value)
        {
            // Call the helper.
            return InherentlyNullableIsNullHelper<T>.IsNullImplementation(value);
        }

        #region CopyFields

        public static T CopyFields<T>(T instance)
            where T : class, new()
        {
            // Validate parameters.
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            // The type.
            Type type = typeof (T);

            // Add or create the copier.
            Action<object, object> copier = CopyFieldCopiers.GetOrAdd(type, CreateCopier);

            // Get the uninitialized object.
            // TODO: Get FormatterServices which has GetSafeUninitializedObject, remove new() constraint.
            var t = new T();

            // Copy.
            copier(instance, t);

            // Return t.
            return t;
        }

        private static readonly ConcurrentDictionary<Type, Action<object, object>> CopyFieldCopiers = new ConcurrentDictionary<Type, Action<object, object>>();

        private static Action<object, object> CreateCopier(Type type)
        {
            // Validate parameters.
            Debug.Assert(type != null);

            // The parameter expressions.
            ParameterExpression fromParameterExpression = Expression.Parameter(typeof(object), "from");
            ParameterExpression toParameterExpression = Expression.Parameter(typeof(object), "to");

            // Typed parameter expressions.
            ParameterExpression typedFromParameterExpression = Expression.Variable(type, "f");
            ParameterExpression typedToParameterExpression = Expression.Variable(type, "t");

            // The block expression.
            var block = new List<Expression> {
                // Add the assignment to the variable.
                Expression.Assign(typedFromParameterExpression, Expression.Convert(fromParameterExpression, type)),
                Expression.Assign(typedToParameterExpression, Expression.Convert(toParameterExpression, type))
            };

            // Create the conversions that assign the conversion.
            block.AddRange(
                from f in type.GetTypeInfo().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                select Expression.Assign(Expression.Field(typedToParameterExpression, f), Expression.Field(typedFromParameterExpression, f))
            );

            // Cycle through the p
            return Expression.Lambda<Action<object, object>>(Expression.Block(
                EnumerableExtensions.From(typedFromParameterExpression).Append(typedToParameterExpression),
                block), fromParameterExpression, toParameterExpression).Compile();
        }

        #endregion

        #endregion
    }
}
