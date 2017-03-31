using System;

namespace Ofl.Core
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2012-07-28</created>
    /// <summary>An attribute that can be applied to
    /// an enumeration value that allows lookup
    /// based on a code.</summary>
    ///
    //////////////////////////////////////////////////
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumCodeAttribute : Attribute
    {
        #region Constructor

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-07-28</created>
        /// <summary>Creates a new instance of the <see cref="EnumCodeAttribute"/>.</summary>
        /// <param name="code">The code to apply to the enum value.</param>
        ///
        //////////////////////////////////////////////////
        public EnumCodeAttribute(string code)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));

            // Assign values.
            Code = code;
        }

        #endregion

        #region Instance, read-only properties.

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2012-07-28</created>
        /// <summary>Gets the code assigned to the enumeration.</summary>
        /// <value>The code applied to the enumeration.</value>
        ///
        //////////////////////////////////////////////////
        public string Code { get; private set; }

        #endregion
    }
}
