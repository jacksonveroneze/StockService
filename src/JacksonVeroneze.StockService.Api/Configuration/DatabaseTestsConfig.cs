using JacksonVeroneze.StockService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class DatabaseTestsConfig
    {
        public static IServiceCollection AddDatabaseTestsConfiguration(this IServiceCollection services, IConfiguration configuration)
            => services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<DatabaseContext>(options =>
                    options
                        .UseInMemoryDatabase("DefaultConnection")
                        .UseLazyLoadingProxies()
                        .UseSnakeCaseNamingConvention()
                        .EnableDetailedErrors()
                        .EnableSensitiveDataLogging());
    }
}
