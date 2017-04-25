using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Ofl.Core.Linq;
using Ofl.Core.Serialization.Newtonsoft.Json;
using Xunit;

namespace Ofl.Core.Tests.Serialization.Newtonsoft.Json
{
    public class ExceptionJsonConverterTests
    {
        #region Helpers

        private static TestException CreateTestException()
        {
            // Creates the invalid operation exception.
            Exception CreateFillerException() => new InvalidOperationException(Guid.NewGuid().ToString());

            // Create an exception and return.
            return new TestException {
                ExceptionProperty = CreateFillerException(),
                ExceptionPropertyWithJsonPropertyAttribute = CreateFillerException(),
                ExceptionsProperty = new List<Exception> {
                    CreateFillerException(),
                    CreateFillerException()
                },
                AggregateException = new AggregateException(new List<Exception> {
                    CreateFillerException(),
                    CreateFillerException()
                })
            };
        }

        private static JsonSerializer CreateSerializer()
        {
            // Create the serializer, add the converter.
            var serializer = new JsonSerializer();

            // Add the converters.
            serializer.Converters.Add(new ExceptionJsonConverter());

            // Do not use ISerializable or the Serializable attribute.
            serializer.ContractResolver = new DefaultContractResolver {
                IgnoreSerializableAttribute = true,
                IgnoreSerializableInterface = true
            };

            // Return the serializer.
            return serializer;
        }

        private (JsonSerializer Serializer, TestException TestException) Setup()
        {
            // Create the tuple and return.
            return (CreateSerializer(), CreateTestException());
        }

        #endregion

        #region Tests

        [Fact]
        public void Test_SingleExceptionProperty()
        {
            // Setup.
            (JsonSerializer Serializer, TestException TestException) setup = Setup();

            // Create the token.
            JToken token = JToken.FromObject(setup.TestException, setup.Serializer);

            // Start asserting.
            setup.Serializer.ContractResolver.AssertExceptionsEqual(setup.TestException.ExceptionProperty, token["ExceptionProperty"]);
        }

        [Fact]
        public void Test_EnumerableExceptionsProperty()
        {
            // Setup.
            (JsonSerializer Serializer, TestException TestException) setup = Setup();

            // Create the token.
            JToken token = JToken.FromObject(setup.TestException, setup.Serializer);

            // Start asserting.
            setup.Serializer.ContractResolver.AssertExceptionsEqual(setup.TestException.ExceptionsProperty, token["ExceptionsProperty"]);
        }

        [Fact]
        public void Test_AggregateException()
        {
            // Setup.
            (JsonSerializer Serializer, TestException TestException) setup = Setup();

            // Create the token.
            JToken token = JToken.FromObject(setup.TestException, setup.Serializer);

            // Start asserting.
            setup.Serializer.ContractResolver.AssertExceptionsEqual(setup.TestException.AggregateException, token["AggregateException"]);
        }

        [Fact]
        public void Test_ExceptionPropertyWithJsonPropertyAttribute()
        {
            // Setup.
            (JsonSerializer Serializer, TestException TestException) setup = Setup();

            // Create the token.
            JToken token = JToken.FromObject(setup.TestException, setup.Serializer);

            // Start asserting.
            setup.Serializer.ContractResolver.AssertExceptionsEqual(setup.TestException.ExceptionPropertyWithJsonPropertyAttribute, token[TestException.ExceptionPropertyWithJsonPropertyAttributeName]);
        }

        #endregion
    }
}
