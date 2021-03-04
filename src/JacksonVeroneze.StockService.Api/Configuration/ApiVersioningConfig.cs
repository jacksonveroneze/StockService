using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class ApiVersioningConfig
    {
        public static IServiceCollection AddVersioningConfigConfiguration(this IServiceCollection services)
        {
            services.AddApiVersioning(p =>
            {
                p.DefaultApiVersion = new ApiVersion(1, 0);
                p.ReportApiVersions = true;
                p.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
