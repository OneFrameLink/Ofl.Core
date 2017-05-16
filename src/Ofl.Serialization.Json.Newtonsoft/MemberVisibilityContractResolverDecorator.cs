//using System;
//using Newtonsoft.Json.Serialization;

//namespace Ofl.Serialization.Json.Newtonsoft
//{
//    //////////////////////////////////////////////////
//    ///
//    /// <author>Nicholas Paldino</author>
//    /// <created>2014-03-21</created>
//    /// <summary>A <see cref="IContractResolver"/>
//    /// implementation that allows for binding of
//    /// JSON properties which map to private
//    /// settors.</summary>
//    ///
//    //////////////////////////////////////////////////
//    public class MemberVisibilityContractResolverDecorator : IContractResolver
//    {
//        #region Constructor

//        //////////////////////////////////////////////////
//        ///
//        /// <author>Nicholas Paldino</author>
//        /// <created>2014-03-21</created>
//        /// <summary>Creates a new instance of the
//        /// <see cref="MemberVisibilityContractResolverDecorator"/>
//        /// class.</summary>
//        /// <param name="contractResolver">The <see cref="IContractResolver"/>
//        /// instance to decorate.</param>
//        ///
//        //////////////////////////////////////////////////
//        public MemberVisibilityContractResolverDecorator(IContractResolver contractResolver)
//        {
//            // Validate parameters.
//            if (contractResolver == null) throw new ArgumentNullException(nameof(contractResolver));

//            // Assign values.
//            _contractResolver = contractResolver;
//        }

//        #endregion

//        #region Instance, read-only state.

//        /// <summary>If true, non public members are written to.</summary>
//        private readonly IContractResolver _contractResolver;

//        #endregion

//        #region Overrides of DefaultContractResolver


//        //////////////////////////////////////////////////
//        ///
//        /// <author>Nicholas Paldino</author>
//        /// <created>2014-03-21</created>
//        /// <summary>Given a <paramref name="type"/>, resolves
//        /// the <see cref="JsonContract"/> for that type.</summary>
//        /// <param name="type">The <see cref="Type"/> that the contract
//        /// is to be resolved to.</param>
//        /// <returns>The <see cref="JsonContract"/> that was resolved from
//        /// the type.</returns>
//        ///
//        //////////////////////////////////////////////////
//        public JsonContract ResolveContract(Type type)
//        {
//            // Validate parameters.
//            if (type == null) throw new ArgumentNullException(nameof(type));

//            // Call the decorated instance.
//            JsonContract contract = _contractResolver.ResolveContract(type);

//            // The properties collection.
//            JsonPropertyCollection properties = null;

//            // Check for type.
//            var objectContract = contract as JsonObjectContract;

//            // Assign.
//            if (objectContract != null) properties = objectContract.Properties;
//            else
//            {
//                // Type sniff.
//                var dynamicContract = contract as JsonDynamicContract;

//                // Assign.
//                if (dynamicContract != null) properties = dynamicContract.Properties;
//            }

//            // If the properties is null, return the contract.
//            if (properties == null) return contract;

//            // Cycle through the properties.
//            foreach (JsonProperty property in properties)
//            {
//                // Make writable.
//                property.Writable = true;
//            }

//            // Return the contract.
//            return contract;
//        }

//        #endregion
//    }
//}
