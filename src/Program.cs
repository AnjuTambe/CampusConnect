using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CampusConnect.WebSite
{
    /// <summary>
    /// The main entry point for the CampusConnect web application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method that starts the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the HostBuilder for the web application.
        /// Configures default settings and specifies the Startup class.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>An IHostBuilder instance.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}