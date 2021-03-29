using JacksonVeroneze.StockService.Domain.Events.Product;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class MassTransitConfig
    {
        public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration["RabbitMq:Host"]))
                return services.AddMassTransit(x =>
                {
                    x.UsingInMemory((context, cfg) =>
                    {
                        cfg.TransportConcurrencyLimit = 100;

                        cfg.ConfigureEndpoints(context);
                    });
                });

            services.AddMassTransit(cfg =>
            {
                cfg.UsingRabbitMq((_, config) =>
                {
                    config.Host(configuration["RabbitMq:Host"], configuration["RabbitMq:VirtualHost"], h =>
                    {
                        h.Username(configuration["RabbitMq:Username"]);
                        h.Password(configuration["RabbitMq:Password"]);
                    });

                    config.Publish<ProductAddedEvent>(x =>
                    {
                        x.Durable = false;
                        x.AutoDelete = true;
                        x.ExchangeType = "topic";
                    });
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
