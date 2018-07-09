using Microsoft.Extensions.Configuration;

namespace NotesApp.IntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration) { }
    }
}
