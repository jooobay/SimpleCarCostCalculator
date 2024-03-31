using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

using Microsoft.EntityFrameworkCore;
using SimpleCarCostCalculator.Api.Cars;
using SimpleCarCostCalculatorServer.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SimpleCarCostCalculator
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
            // A InMemory Database, implement class CarDb in order to setup will other persistant types
            services.AddDbContext<CarDb>(opt => opt.UseInMemoryDatabase("CarList"));
            
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
            
            // To ease client/server interactions. Needs to be changed if deployed 
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAny",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAny");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/categories", () =>
                {
                    var categories =
                        Configuration.GetSection("Categories").Get<List<CategoryDto>>()?
                            .Select(c => new { c.Key, c.Name });

                    return Results.Json(categories);
                });
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                spa.Options.DevServerPort = 5173;
                spa.UseReactDevelopmentServer(npmScript: "start");
            });
        }
    }
}
