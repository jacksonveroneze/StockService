using System;
using CacheManager.Core;
using EasyCaching.InMemory;
using EFCoreSecondLevelCacheInterceptor;
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
            => services.AddEfSecondLevelCacheConfiguration()
                .AddDbContext<DatabaseContext>((_, options) =>
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

        private static DbContextOptionsBuilder AddEfSecondLevelCacheInterceptor(this DbContextOptionsBuilder services,
            IServiceProvider serviceProvider)
            => services.AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>());

        private static IServiceCollection AddEfSecondLevelCacheConfiguration(this IServiceCollection services)
        {
            const string providerName = "InMemory";

            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));

            services.AddEFSecondLevelCache(options =>
                options.UseEasyCachingCoreProvider(providerName).DisableLogging()
            );

            services.AddEasyCaching(options =>
            {
                options.UseInMemory(config =>
                {
                    config.DBConfig = new InMemoryCachingOptions
                    {
                        ExpirationScanFrequency = 60,
                        SizeLimit = 100,
                        EnableReadDeepClone = false,
                        EnableWriteDeepClone = false,
                    };
                    config.MaxRdSecond = 120;
                    config.EnableLogging = false;
                    config.LockMs = 5000;
                    config.SleepMs = 300;
                }, providerName);
            });

            return services;
        }
    }
}
