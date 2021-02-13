using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class HealthChecksConfig
    {
        public static IServiceCollection HealthChecksConfiguration(this IServiceCollection services)
        {
            services.AddHealthChecks();

            return services;
        }
    }
}
