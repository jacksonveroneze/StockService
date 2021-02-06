using JacksonVeroneze.StockService.Infra.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
            => DependencyInjection.RegisterServices(services);
    }
}
