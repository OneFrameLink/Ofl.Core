using System;
using System.ComponentModel;

namespace Ofl.Core.ComponentModel
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2011-10-21</created>
    /// <summary>Defines a short code for whatever it is
    /// assigned to, much like the
    /// <see cref="DescriptionAttribute"/>
    /// attribute.</summary>
    ///
    //////////////////////////////////////////////////
    [AttributeUsage(AttributeTargets.All)]
    public class CodeAttribute : Attribute
    {
        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2011-10-21</created>
        /// <summary>Creates a new instance of the
        /// <see cref="CodeAttribute"/> class.</summary>
        /// <param name="code">The code.</param>
        ///
        //////////////////////////////////////////////////
        public CodeAttribute(string code)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));

            // Assign values.
            Code = code;
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2011-10-21</created>
        /// <summary>Gets the code for this attribute.</summary>
        /// <value>The code for this attribute.</value>
        ///
        //////////////////////////////////////////////////
        public string Code { get; private set; }
    }
}
