﻿using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Services;
using JacksonVeroneze.StockService.Bus.Mediator;
using JacksonVeroneze.StockService.Data;
using JacksonVeroneze.StockService.Data.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Infra.IoC
{
    public static class DependencyInjection
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // BusHandler
            services.AddScoped<IBusHandler, BusHandler>();

            // Application
            services.AddScoped<IProductApplicationService, ProductApplicationService>();

            // Domain
            services.AddScoped<IProductRepository, ProductRepository>();

            // Context
            services.AddScoped<DatabaseContext>();
        }
    }
}
