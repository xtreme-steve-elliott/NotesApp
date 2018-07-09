using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NotesApp.IntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration) { }
        
        protected override void ConfigureDbOptions(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("notes_app");
        }
    }
}
