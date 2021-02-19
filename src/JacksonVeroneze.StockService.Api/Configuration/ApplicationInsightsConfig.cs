using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class ApplicationInsightsConfig
    {
        public static IServiceCollection AddApplicationInsightsConfiguration(this IServiceCollection services,
            IConfiguration configuration)
            => services.AddApplicationInsightsTelemetry(
                configuration.GetValue<string>("APPLICATIONINSIGHTS_INSTRUMENTATIONKEY"));
    }
}
