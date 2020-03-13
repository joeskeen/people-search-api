using System;
using System.IO;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Newtonsoft.Json;

namespace assessment
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
            services.AddDbContext<PeopleContext>(opt => opt.UseInMemoryDatabase("People"));
            services.AddControllers(mvcOptions => mvcOptions.EnableEndpointRouting = false);
            services.AddOData();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            SeedData(scopeFactory);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Person>("People");
            return builder.GetEdmModel();
        }

        private static void SeedData(IServiceScopeFactory scopeFactory)
        {
            Console.WriteLine("Seeding database...");
            using (var scope = scopeFactory.CreateScope())
            {
                var peopleJson = File.ReadAllText("Data/People.json");
                var people = JsonConvert.DeserializeObject<Person[]>(peopleJson);
                var context = scope.ServiceProvider.GetRequiredService<PeopleContext>();
                context.People.AddRange(people);
                context.SaveChanges();
            }
            Console.WriteLine("Database seeded.");
        }
    }
}
