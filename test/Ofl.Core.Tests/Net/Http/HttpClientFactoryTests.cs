using System.Net.Http;
using System.Threading;
using Ofl.Core.Net.Http;
using Xunit;

namespace Ofl.Core.Tests.Net.Http
{
    public class HttpClientFactoryTests
    {
        [Fact]
        public void Test_Create()
        {
            // The cancellation token.
            CancellationToken cancellationToken = CancellationToken.None;

            // Create the instance.
            IHttpClientFactory factory = new HttpClientFactory();

            // Create.
            HttpClient client = factory.Create(new HttpClientHandler(), true);

            // Not null.
            Assert.NotNull(client);
        }
    }
}
