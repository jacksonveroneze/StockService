using JacksonVeroneze.StockService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class DatabaseConfigTests
    {
        public static IServiceCollection AddDatabaseConfigurationTests(this IServiceCollection services)
            => services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<DatabaseContext>(options =>
                    options
                        .UseInMemoryDatabase("InMemoryDatabase")
                        .UseLazyLoadingProxies()
                        .UseSnakeCaseNamingConvention()
                        .EnableDetailedErrors()
                        .EnableSensitiveDataLogging());
    }
}
