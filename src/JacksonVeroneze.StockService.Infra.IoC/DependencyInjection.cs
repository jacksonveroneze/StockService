using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Services;
using JacksonVeroneze.StockService.Core.Communication.Mediator;
using JacksonVeroneze.StockService.Data;
using JacksonVeroneze.StockService.Data.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Infra.IoC
{
    public static class DependencyInjection
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // MediatorHandler
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Application
            services.AddScoped<IProductApplicationService, ProductApplicationService>();

            // Domain
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<DatabaseContext>();
        }
    }
}
