
using CarRental.Logging.Configurations;
using Serilog;
using Serilog.Enrichers.AspnetcoreHttpcontext;

namespace CarRental.API
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                                    .AddEnvironmentVariables()
                                    .Build();

        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                Log.Information($"Starting host...");
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<StartUp>();

                    webBuilder.UseSerilog((provider, context, loggerConfig) =>
                    {
                        loggerConfig.WithCarRentalConfiguration(provider, Configuration);
                    });
                });
        }
    }
}
