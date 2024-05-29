

namespace ReadyTech.BrewMug.Api
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ReadyTech.BrewMug.AC.OpenWeatherSystem;
    using ReadyTech.BrewMug.AppMgr;
    using ReadyTech.BrewMug.AppMgr.Interfaces;
    using ReadyTech.BrewMug.Data.Interfaces;
    using ReadyTech.BrewMug.Data.Repositories;
    using ReadyTech.BrewMug.Domain.Services;
    using System.Reflection;

    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Register MediatR and Automaper in AppManager DI
            services.AddAppMgrDI();
            
            services.AddSingleton<IBrewService, BrewService>();
            services.AddSingleton<IBrewRepository, BrewRepository>();

            services.AddSingleton<IOpenWeatherSystem, OpenWeatherSystem>();

            services.AddSingleton<RequestService>();

            services.AddHttpClient("OpenWeatherApi", options =>
            {
                options.Timeout = TimeSpan.FromSeconds(30);
            });
            // Enable CORS and add our own ploicy to restrict calls from non trusted apps
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                    });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                
            }
            
            //app.UseMiddleware<CoffeeAvailabilityCheckMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();
            // Enable CORS
            //Add the trusted sites list so that it check and allow the calls from those lsit
            app.UseCors("AllowAll");
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "brew-coffee",
                    pattern: "brew-coffee", // Customize this route as needed
                    defaults: new { controller = "Brew", action = "GetBrewCoffeeAsync" }
            );

            });
        }
    }
}
