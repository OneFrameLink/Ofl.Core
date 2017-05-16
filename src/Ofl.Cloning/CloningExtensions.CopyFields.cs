using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ofl.Linq;

namespace Ofl.Cloning
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2012-09-01</created>
    /// <summary>Utility methods.</summary>
    ///
    //////////////////////////////////////////////////
    public static partial class CloningExtensions
    {
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
                // NOTE: Used to be
                // from f in type.GetTypeInfo().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                from f in type.GetTypeInfo().DeclaredFields.Where(f => !f.IsStatic)
                select Expression.Assign(Expression.Field(typedToParameterExpression, f), Expression.Field(typedFromParameterExpression, f))
            );

            // Cycle through the p
            return Expression.Lambda<Action<object, object>>(Expression.Block(
                EnumerableExtensions.From(typedFromParameterExpression).Append(typedToParameterExpression),
                block), fromParameterExpression, toParameterExpression).Compile();
        }
    }
}
