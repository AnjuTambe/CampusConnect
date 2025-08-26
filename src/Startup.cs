using CampusConnect.WebSite.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CampusConnect.WebSite
{
    /// <summary>
    /// Configures the services and request pipeline for the CampusConnect web application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the Startup class.
        /// </summary>
        /// <param name="configuration">The application's configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // The application's configuration.
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// Configures application services, including Razor Pages, Blazor, HttpClient, Controllers, and the Job Service.
        /// </summary>
        /// <param name="services">The collection of services to configure.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpClient();
            services.AddControllers();
            // Removed: services.AddTransient<JsonFileProductService>();
            // Registers the IJobService interface with the JsonFileJobService implementation for dependency injection.
            services.AddTransient<IJobService, JsonFileJobService>(); // Register interface mapping
            // Registers the IEmployerService interface with the JsonFileEmployerService implementation for dependency injection.
            services.AddTransient<IEmployerService, JsonFileEmployerService>(); // Register interface mapping
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// Configures the application's request handling pipeline, including error handling, static files, routing, and endpoints.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Use developer exception page in development environment.
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Use exception handler and HSTS in production environment.
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Redirect HTTP requests to HTTPS.
            app.UseHttpsRedirection();
            // Serve static files (HTML, CSS, JS, images).
            app.UseStaticFiles();

            // Configure routing for incoming requests.
            app.UseRouting();

            // Configure authorization middleware.
            app.UseAuthorization();

            // Configure endpoints for Razor Pages, Controllers, and Blazor Hub.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapBlazorHub();

                // Removed product endpoint mapping
            });
        }
    }
}