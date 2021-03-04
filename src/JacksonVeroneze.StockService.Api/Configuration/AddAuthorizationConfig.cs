using JacksonVeroneze.StockService.Api.Configuration.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class AddAuthorizationConfig
    {
        public static IServiceCollection AddAuthorizationConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            string[] customPolices =
            {
                "products:filter", "products:find", "products:create", "products:update", "products:delete"
            };

            services.AddAuthorization(options =>
            {
                foreach (string customPolice in customPolices)
                    options.AddCustomPolicy(customPolice, configuration["Auth:Authority"]);
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }

        private static void AddCustomPolicy(this AuthorizationOptions options, string policyName,
            string authority)
        {
            options.AddPolicy(policyName,
                policy => policy.Requirements.Add(new HasScopeRequirement(policyName, authority)));
        }
    }
}
