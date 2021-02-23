using JacksonVeroneze.StockService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class DatabaseConfig
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
            => services.AddEntityFrameworkSqlServer()
                .AddDbContext<DatabaseContext>(options =>
                    options
                        .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                        .UseLazyLoadingProxies()
                        .UseSnakeCaseNamingConvention());
    }
}
