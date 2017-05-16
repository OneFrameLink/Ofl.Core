//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq.Expressions;
//using System.Reflection;

//namespace Ofl.Core
//{
//    public class PropertyPath
//    {
//        #region Constructor

//        protected PropertyPath() : this(null)
//        { }

//        protected PropertyPath(PropertyPath root)
//        {
//            // If the root is null, assign the path.
//            if (root == null)
//            {
//                // Assign path.
//                _path = new Queue<PropertyInfo>();

//                // Set the root.
//                Root = this;
//            }
//            else
//                // Root is root.
//                Root = root;

//            // Root must not be null.
//            Debug.Assert(Root != null);
//        }

//        #endregion

//        #region Instance, read-only state.

//        private readonly Queue<PropertyInfo> _path;

//        public PropertyPath Root { get; }

//        public IEnumerable<PropertyInfo> Path => Root._path;

//        #endregion

//        #region Action methods.

//        protected void AppendPath(PropertyInfo property)
//        {
//            // Validate parameters.
//            if (property == null) throw new ArgumentNullException(nameof(property));

//            // Push.
//            Root._path.Enqueue(property);
//        }

//        public static PropertyPath<T> Of<T>()
//        {
//            // Create a new instance.
//            return new PropertyPath<T>();
//        }

//        #endregion
//    }
//    public class PropertyPath<T> : PropertyPath
//    {
//        #region Constructors

//        internal PropertyPath() : base(null)
//        { }

//        internal PropertyPath(PropertyPath root) : base(root)
//        { }

//        #endregion

//        #region Helpers.

//        public PropertyPath<TResult> ThenEnumerable<TResult>(Expression<Func<T, IEnumerable<TResult>>> expression)
//        {
//            // Validate parameters.
//            if (expression == null) throw new ArgumentNullException(nameof(expression));

//            // Get the member info.
//            var propertyInfo = (expression.Body as MemberExpression)?.Member as PropertyInfo;

//            // If null, throw.
//            if (propertyInfo == null)
//                throw new InvalidOperationException($"The {nameof(expression)} parameter must be an expression backed by a PropertyInfo.");

//            // Push.
//            AppendPath(propertyInfo);

//            // Return the new expression.
//            return new PropertyPath<TResult>(Root);
//        }

//        public PropertyPath<TResult> Then<TResult>(Expression<Func<T, TResult>> expression)
//        {
//            // Validate parameters.
//            if (expression == null) throw new ArgumentNullException(nameof(expression));

//            // Get the member info.
//            var propertyInfo = (expression.Body as MemberExpression)?.Member as PropertyInfo;

//            // If null, throw.
//            if (propertyInfo == null)
//                throw new InvalidOperationException($"The {nameof(expression)} parameter must be an expression backed by a PropertyInfo.");

//            // Push.
//            AppendPath(propertyInfo);

//            // Return the new expression.
//            return new PropertyPath<TResult>(Root);
//        }

//        #endregion
//    }
//}
