using Moq;
using Moq.Protected;

namespace MoviesToWatch.UnitTests.Helpers
{
    public static class HttpClientExtensions
    {
        public static Mock<HttpMessageHandler> SetupRequest(this Mock<HttpMessageHandler> mockHandler, HttpMethod method, string url, HttpResponseMessage response)
        {
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == method && req.RequestUri.AbsoluteUri.Contains(url)), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            return mockHandler; // Returning the mock so we can chain the setups
        }
    }

}
