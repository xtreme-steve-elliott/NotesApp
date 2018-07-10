using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.Models;
using NotesApp.Repositories;
using NotesApp.Services;
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
using Swashbuckle.AspNetCore.Swagger;

namespace NotesApp
{
    public class Startup
    {
        private const string SwaggerVersion = "v1";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NotesAppDbContext>(ConfigureDbOptions);
            services.AddTransient<INoteRepository, NoteRepository>();
            services.AddTransient<INoteService, NoteService>();
            services.AddRouting(opt => opt.LowercaseUrls = true);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerVersion, new Info { Title = "Notes App API", Version = SwaggerVersion });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
               c.SwaggerEndpoint($"/swagger/{SwaggerVersion}/swagger.json", $"Notes App API {SwaggerVersion}"); 
            });
        }

        protected virtual void ConfigureDbOptions(DbContextOptionsBuilder options)
        {
            options.UseMySql(Configuration);
        }
    }
}
