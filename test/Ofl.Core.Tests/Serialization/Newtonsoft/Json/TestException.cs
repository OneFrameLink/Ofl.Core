using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ofl.Core.Tests.Serialization.Newtonsoft.Json
{
    public class TestException : Exception
    {
        public Exception ExceptionProperty { get; set; }

        public IReadOnlyCollection<Exception> ExceptionsProperty { get; set; }

        internal const string ExceptionPropertyWithJsonPropertyAttributeName = "MyException";

        [JsonProperty(ExceptionPropertyWithJsonPropertyAttributeName)]
        public Exception ExceptionPropertyWithJsonPropertyAttribute { get; set; }

        public AggregateException AggregateException { get; set; }
    }
}