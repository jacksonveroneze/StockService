using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Services;
using JacksonVeroneze.StockService.Application.Validations.Adjustment;
using JacksonVeroneze.StockService.Application.Validations.AdjustmentItem;
using JacksonVeroneze.StockService.Application.Validations.Output;
using JacksonVeroneze.StockService.Application.Validations.OutputItem;
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
using JacksonVeroneze.StockService.Infra.Bus.Mediator;
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
            services.AddScoped<IMovementApplicationService, MovementApplicationService>();
            services.AddScoped<IAdjustmentApplicationService, AdjustmentApplicationService>();
            services.AddScoped<IOutputApplicationService, OutputApplicationService>();
            services.AddScoped<IPurchaseApplicationService, PurchaseApplicationService>();
        }

        private static void RegisterDomainServices(IServiceCollection services)
        {
            services.AddScoped<IAdjustmentService, AdjustmentService>();
            services.AddScoped<IDevolutionService, DevolutionService>();
            services.AddScoped<IOutputService, OutputService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IMovementService, MovementService>();
            services.AddScoped<IProductService, ProductService>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IAdjustmentRepository, AdjustmentRepository>();
            services.AddScoped<IDevolutionRepository, DevolutionRepository>();
            services.AddScoped<IOutputRepository, OutputRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IMovementRepository, MovementRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }

        private static void RegisterServicesValidations(IServiceCollection services)
        {
            services.AddScoped<IProductValidator, ProductBaseValidator>();
            //
            services.AddScoped<IAdjustmentValidator, AdjustmentValidator>();
            services.AddScoped<IOutputValidator, OutputValidator>();
            services.AddScoped<IPurchaseValidator, PurchaseValidator>();
            //
            services.AddScoped<IAdjustmentItemValidator, AdjustmentItemValidator>();
            services.AddScoped<IOutputItemValidator, OutputItemValidator>();
            services.AddScoped<IPurchaseItemValidator, PurchaseItemBaseValidator>();
        }

        private static void RegisterServicesOthers(IServiceCollection services)
        {
            services.AddScoped<IBus, BusMediator>();
            services.AddScoped<IUser, AspNetUser>();
            services.AddScoped<DatabaseContext>();
        }
    }
}
