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

        #endregion


        #region Copy

        public static TDestination CopySharedProperties<TSource, TDestination>(this TSource source)
            where TDestination : new()
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Call the overload.
            return CopySharedProperties(source, new TDestination());
        }

        public static TDestination CopySharedProperties<TSource, TDestination>(TSource source, TDestination destination)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            // Map the destination properties.
            // TODO: Generate compiled code, cache.
            // TODO: Map by property type as well?
            IReadOnlyDictionary<string, PropertyInfo> destinationProperties = typeof(TDestination).
                GetPropertiesWithPublicInstanceGetters().
                ToReadOnlyDictionary(pi => pi.Name, pi => pi);

            // Cycle through the source properties.  Copy to the destination.
            foreach (PropertyInfo sourceProperty in typeof(TSource).GetPropertiesWithPublicInstanceGetters())
            {
                // If not found, then skip.
                if (!destinationProperties.TryGetValue(sourceProperty.Name, out PropertyInfo destinationProperty)) continue;

                // If the source type is not assignable to the destination type, continue.
                if (!destinationProperty.PropertyType.GetTypeInfo().IsAssignableFrom(sourceProperty.PropertyType.GetTypeInfo()))
                    continue;

                // Assign.
                destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
            }

            // Return the destination.
            return destination;
        }

        #endregion
    }
}
