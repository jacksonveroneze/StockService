using FluentValidation;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.DTO.Purchase.Validations;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem.Validations;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Services;
using JacksonVeroneze.StockService.Bus.Mediator;
using JacksonVeroneze.StockService.Data;
using JacksonVeroneze.StockService.Data.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using JacksonVeroneze.StockService.Domain.Services;
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
            services.AddScoped<IAdjustmentRepository, AdjustmentRepository>();
            services.AddScoped<IOutputRepository, OutputRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IMovementRepository, MovementRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IAdjustmentService, AdjustmentService>();
            services.AddScoped<IOutputService, OutputService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IMovementService, MovementService>();

            // Validations
            services.AddScoped<IValidator<AddOrUpdatePurchaseDto>, AddOrUpdatePurchaseDtoValidator>();
            services.AddScoped<IValidator<AddOrUpdatePurchaseItemDto>, AddOrUpdatePurchaseItemDtoValidator>();

            // Context
            services.AddScoped<DatabaseContext>();
        }
    }
}
