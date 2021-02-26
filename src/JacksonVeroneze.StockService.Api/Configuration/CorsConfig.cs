using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class CorsConfig
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services,
            IConfiguration configuration,
            string corsPolicy)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy,
                    builder =>
                    {
                        builder
                            .WithOrigins(configuration["URLS_ALLOW_CORS"].Split(";"))
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                    });
            });

            return services;
        }
    }
}
