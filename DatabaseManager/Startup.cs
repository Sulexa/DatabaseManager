using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using DatabaseManager.Models;
using DatabaseManager.Services;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using MigrationManager.Example;
using DatabaseManager.Example;

namespace DatabaseManager
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
                    );

            services.AddScoped<IContextFactoryServices<ExampleDbContext>>(cfs => 
                new ContextFactoryServices<ExampleDbContext>(
                    this.GetDatabaseConfigList())
                );

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/error");
            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles(); 
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseRouting();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-docs/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api-docs/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        private List<DatabaseConfig> GetDatabaseConfigList()
        {
            var databaseConfigList = new List<DatabaseConfig>();

            foreach (DatabaseEnvironment databaseEnvironment in (DatabaseEnvironment[])System.Enum.GetValues(typeof(DatabaseEnvironment)))
            {
                var connectionString = _configuration.GetConnectionString(databaseEnvironment.ToString());
                if (!string.IsNullOrEmpty(connectionString))
                {
                    databaseConfigList.Add(new DatabaseConfig
                    {
                        ConnectionString = connectionString,
                        DatabaseEnvironment = databaseEnvironment
                    });
                }
            }
            return databaseConfigList;
        }
    }
}
