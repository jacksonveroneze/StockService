using JacksonVeroneze.NET.Commons.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class CorsConfig
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services,
            IConfiguration configuration) =>
            services.AddCorsConfiguration(x =>
                x.UrlsAllowed = configuration["Urls_Allow_Cors"].Split(";"));
    }
}
