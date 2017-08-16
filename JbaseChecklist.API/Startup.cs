using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JbaseChecklist.Data.Contexts;
using JbaseChecklist.Domain;
using JbaseChecklist.Data.Repositories;

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
            //services.AddDbContext<ChecklistItemContext>(opt => opt.UseInMemoryDatabase("JbaseChecklist"));
            services.AddDbContext<InMemoryChecklistContext>(opt => opt.UseInMemoryDatabase("JbaseChecklist"));
            services.AddScoped<IChecklistContext, InMemoryChecklistContext>();
            services.AddScoped<IChecklistRepository, EFChecklistRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
