using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JbaseChecklist.Data.Contexts;
using JbaseChecklist.Domain;
using JbaseChecklist.Data.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace JbaseChecklist.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            /* You can uncomment this to use the in-memory database context. This is mainly for testing and development */
            //services.AddDbContext<InMemoryChecklistContext>(opt => opt.UseInMemoryDatabase("JbaseChecklist"));
            //services.AddScoped<IChecklistContext, InMemoryChecklistContext>();

            services.AddDbContextPool<SqlChecklistContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IChecklistContext, SqlChecklistContext>();

            services.AddScoped<IChecklistRepository, EFChecklistRepository>();

            services.AddMvc();

            // Register the Swagger generator
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new Info { Title = "JbaseChecklist API", Version = "v1" });

                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "JbaseChecklist.API.xml");
                opt.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SqlChecklistContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "JbaseChecklist V1");
            });

            DbInitializer.Initialize(context);

        }


    }
}
