using JacksonVeroneze.NET.Commons.Database;
using JacksonVeroneze.StockService.Infra.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class DatabaseConfig
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services,
            IConfiguration configuration)
            => services.AddPostgreSqlDatabaseConfiguration<DatabaseContext>(x =>
                x.ConnectionString = configuration.GetConnectionString("DefaultConnection"));
    }
}
