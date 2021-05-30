using System;
using JacksonVeroneze.StockService.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class DatabaseConfig
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services,
            IConfiguration configuration)
            => services.AddDbContext<DatabaseContext>((serviceProvider, options) =>
                    options
                        .UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                            optionsBuilder =>
                            {
                                optionsBuilder
                                    .CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds)
                                    .EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                            })
                        .UseLazyLoadingProxies()
                        .UseSnakeCaseNamingConvention()
                        .EnableDetailedErrors()
                        .EnableSensitiveDataLogging());
    }
}
