using CarRental.API.AuthorizationPolicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace CarRental.API.DependencyInjection
{
    public static class AuthenticationServicesExtension
    {
        public static IServiceCollection AddAuthenticationWithAuthorizationSupport(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddMicrosoftIdentityWebApiAuthentication(config, "AzureAdB2C");

            services.AddSingleton<IAuthorizationHandler, ScopesHandler>();

            return services;
        }
    }
}
