using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace NotesApp.IntegrationTests.Utils
{
    public static class HttpClientUtils
    {
        public static HttpClient CreateClient()
        {
            var server = new TestServer(
                new WebHostBuilder()
                    .UseStartup<TestStartup>()
            );
            return server.CreateClient();
        }
    }
}
