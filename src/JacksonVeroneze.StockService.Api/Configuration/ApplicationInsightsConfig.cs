using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class ApplicationInsightsConfig
    {
        public static IServiceCollection AddApplicationInsightsConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            string instrumentationKey = configuration.GetValue<string>("ApplicationInsights_InstrumentationKey");

            if (string.IsNullOrEmpty(instrumentationKey) is false)
                services.AddApplicationInsightsTelemetry(
                    configuration.GetValue<string>("ApplicationInsights_InstrumentationKey"));

            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
            {
                module.EnableSqlCommandTextInstrumentation = true;
                module.EnableRequestIdHeaderInjectionInW3CMode = true;
            });

            return services;
        }
    }
}
