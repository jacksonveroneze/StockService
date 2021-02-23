using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
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

        public static IApplicationBuilder UseHealthChecksSetup(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health");

            return app;
        }

        public static IApplicationBuilder UseHealthChecksUISetup(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/healthchecks-data-ui", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI();

            return app;
        }
    }
}
