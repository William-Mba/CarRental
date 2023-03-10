using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CarRental.Application
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddApplicationExtensions(this IServiceCollection services)
        {
            return services
                .AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
