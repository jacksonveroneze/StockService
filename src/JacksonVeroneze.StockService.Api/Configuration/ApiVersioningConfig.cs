using JacksonVeroneze.NET.Commons.ApiVersioning;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class ApiVersioningConfig
    {
        public static IServiceCollection AddVersioningConfigConfiguration(this IServiceCollection services)
            => services.AddApiVersioningConfiguration(x =>
            {
                x.MajorVersion = 1;
                x.MinorVersion = 0;
            });
    }
}
