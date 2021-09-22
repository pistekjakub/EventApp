using EventApp.Data.Configuration;
using EventApp.Data.Repositories;
using EventApp.Data.Sql;
using EventApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApp
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
            // Controllers
            services.AddControllers();

            // cors
            services.AddCors(options => options.AddDefaultPolicy(
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            // Database
            services.AddTransient<IDbConnectionFactory, SqlConnectionFactory>();
            services.AddTransient<ISqlExecutor, SqlExecutor>();
            services.AddScoped<IDbContext, DbContext>();

            // Repositories
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();

            // Services
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
