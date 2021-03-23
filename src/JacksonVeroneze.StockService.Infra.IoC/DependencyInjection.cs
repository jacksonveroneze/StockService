using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Services;
using JacksonVeroneze.StockService.Application.Validations.Product;
using JacksonVeroneze.StockService.Application.Validations.Purchase;
using JacksonVeroneze.StockService.Application.Validations.PurchaseItem;
using JacksonVeroneze.StockService.Infra.Bus;
using JacksonVeroneze.StockService.Infra.Data;
using JacksonVeroneze.StockService.Infra.Data.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Identity;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using JacksonVeroneze.StockService.Domain.Services;
using JacksonVeroneze.StockService.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Infra.IoC
{
    public static class DependencyInjection
    {
        public static void RegisterServices(IServiceCollection services)
        {
            RegisterApplicationServices(services);
            RegisterDomainServices(services);
            RegisterRepositories(services);
            RegisterServicesValidations(services);
            RegisterServicesOthers(services);
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddScoped<IProductApplicationService, ProductApplicationService>();
            services.AddScoped<IPurchaseApplicationService, PurchaseApplicationService>();
            services.AddScoped<IMovementApplicationService, MovementApplicationService>();
        }

        private static void RegisterDomainServices(IServiceCollection services)
        {
            services.AddScoped<IAdjustmentService, AdjustmentService>();
            services.AddScoped<IOutputService, OutputService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IMovementService, MovementService>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IAdjustmentRepository, AdjustmentRepository>();
            services.AddScoped<IOutputRepository, OutputRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IMovementRepository, MovementRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }

        private static void RegisterServicesValidations(IServiceCollection services)
        {
            services.AddScoped<IProductValidator, ProductValidator>();
            services.AddScoped<IPurchaseValidator, PurchaseValidator>();
            services.AddScoped<IPurchaseItemValidator, PurchaseItemValidator>();
        }

        private static void RegisterServicesOthers(IServiceCollection services)
        {
            services.AddScoped<IBus, Bus.Mediator.Bus>();
            services.AddScoped<IUser, AspNetUser>();
            services.AddScoped<DatabaseContext>();
        }
    }
}
