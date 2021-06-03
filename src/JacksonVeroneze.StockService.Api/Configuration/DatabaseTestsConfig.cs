using JacksonVeroneze.NET.Commons.Database;
using JacksonVeroneze.StockService.Infra.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class DatabaseTestsConfig
    {
        public static IServiceCollection AddDatabaseTestsConfiguration(this IServiceCollection services,
            IConfiguration configuration)
            => services.AddSqliteDatabaseConfiguration<DatabaseContext>(x =>
                x.ConnectionString = configuration.GetConnectionString("DefaultConnection"));
    }
}
