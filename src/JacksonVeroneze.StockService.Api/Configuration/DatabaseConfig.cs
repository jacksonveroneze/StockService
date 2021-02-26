using JacksonVeroneze.StockService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class DatabaseConfig
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionStringConf = configuration.GetConnectionString("DefaultConnection");
            string connectionStringEnv = configuration.GetValue<string>("CONNECION_STRING");

            return services.AddEntityFrameworkSqlServer()
                .AddDbContext<DatabaseContext>(options =>
                    options
                        .UseSqlServer(connectionStringConf ??= connectionStringEnv)
                        .UseLazyLoadingProxies()
                        .UseSnakeCaseNamingConvention()
                        .EnableDetailedErrors()
                        .EnableSensitiveDataLogging());
        }
    }
}
