using JacksonVeroneze.NET.Commons.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class ApplicationInsightsConfig
    {
        public static IServiceCollection AddApplicationInsightsConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration["ApplicationInsights_InstrumentationKey"]) is false)
                services.AddApplicationInsightsConfiguration(x =>
                    x.InstrumentationKey = configuration["ApplicationInsights_InstrumentationKey"]);

            return services;
        }
    }
}
