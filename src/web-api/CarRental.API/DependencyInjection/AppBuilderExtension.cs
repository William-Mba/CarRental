using CarRental.API.AuthorizationPolicies;
using CarRental.API.DtoValidations;
using CarRental.API.DtoValidations.Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace CarRental.API.DependencyInjection
{
    public static class AppBuilderExtension
    {
        public static IServiceCollection AddApiExtensions(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthenticationWithAuthorizationSupport(config);

            services.AddSwaggerServices();

            services.AddControllers(cfg =>
            {
                cfg.Filters.Add(new DtoValidationFilter());

                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();

                cfg.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddFluentValidationAutoValidation();

            services.AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<CustomerCarReservationValidator>();

            services.AddAuthorization(options => options.AddPolicy("AccessAsUser", policy =>
            {
                policy.Requirements.Add(new ScopesRequirement("access_as_user"));
            }));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddHttpContextAccessor();

            return services;
        }

    }
}
