using CarRental.API.DependencyInjection;
using CarRental.API.Middleware;
using CarRental.Application;
using CarRental.Persistence;

namespace CarRental.API
{
    public class StartUp
    {
        public IConfiguration Configuration { get; }

        public StartUp(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // This method gets called by the runtime. Use this to add services to the conatiner.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddApplicationExtensions(); // Apply Application layer Services Extension

            services.AddPersistenceExtensions(Configuration); // Apply Persistence layer Services Extension

            services.AddApiExtensions(Configuration); // Apply API layer Services Extension

        }

        // This method gets called by the runtime. Use this to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApiExceptionHandler();

            app.UseHttpsRedirection();

            app.UseSwaggerServices();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
