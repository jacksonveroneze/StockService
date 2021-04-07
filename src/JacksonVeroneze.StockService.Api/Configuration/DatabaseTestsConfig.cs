using JacksonVeroneze.StockService.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class DatabaseTestsConfig
    {
        public static IServiceCollection AddDatabaseTestsConfiguration(this IServiceCollection services, IConfiguration configuration)
            => services
                .AddDbContext<DatabaseContext>((_, options) =>
                    options
                        .UseSqlite(configuration.GetConnectionString("DefaultConnection"))
                        .UseLazyLoadingProxies()
                        .UseSnakeCaseNamingConvention()
                        .EnableDetailedErrors()
                        .EnableSensitiveDataLogging());
    }
}
