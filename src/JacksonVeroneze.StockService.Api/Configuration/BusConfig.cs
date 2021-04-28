using System;
using JacksonVeroneze.StockService.Core.Messages;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;
using JacksonVeroneze.StockService.Domain.Events.Product;
using MassTransit;
// using MassTransit.RabbitMqTransport.Topology;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// using RabbitMQ.Client;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class BusConfig
    {
        public static IServiceCollection AddBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            // if (configuration["BusType"].Equals("Memory", StringComparison.InvariantCultureIgnoreCase))
            //     return services.AddMassTransit(x =>
            //     {
            //         x.UsingInMemory((context, cfg) =>
            //         {
            //             cfg.TransportConcurrencyLimit = 100;
            //
            //             cfg.ConfigureEndpoints(context);
            //         });
            //     });
            //
            // services.AddMassTransit(cfg =>
            // {
            //     cfg.UsingRabbitMq((_, config) =>
            //     {
            //         config.Host(configuration["BusRabbitMq:Host"], configuration["BusRabbitMq:VirtualHost"], h =>
            //         {
            //             h.Username(configuration["BusRabbitMq:Username"]);
            //             h.Password(configuration["BusRabbitMq:Password"]);
            //         });
            //
            //         config.Publish<Message>(p => p.Exclude = true);
            //         config.Publish<Event>(p => p.Exclude = true);
            //         config.Publish<DomainEvent>(p => p.Exclude = true);
            //
            //         config.PublishTopology.BrokerTopologyOptions = PublishBrokerTopologyOptions.FlattenHierarchy;
            //
            //         config.Publish<ProductAddedEvent>(x =>
            //         {
            //             x.Durable = false;
            //             x.AutoDelete = true;
            //             x.ExchangeType = ExchangeType.Topic;
            //         });
            //     });
            // });
            //
            // services.AddMassTransitHostedService();

            return services;
        }
    }
}
