using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Adjustment;
using JacksonVeroneze.StockService.Domain.Events.Output;
using JacksonVeroneze.StockService.Domain.Events.Purchase;
using JacksonVeroneze.StockService.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace JacksonVeroneze.StockService.Api.Util
{
    public static class ExecuteMigrations
    {
        public static async Task Execute(IHost host, string[] args)
        {
            Log.Information("Migrations: {0}", "Performing migrations");

            using IServiceScope scope = host.Services.CreateScope();

            DatabaseContext databaseContext =
                scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            if (((IList)args).Contains("d"))
                await databaseContext.Database.EnsureDeletedAsync();

            await databaseContext.Database.MigrateAsync();

            //await SeedData(databaseContext);
        }

        private static async Task SeedData(DatabaseContext databaseContext)
        {
            Log.Information("Seed: {0}", "Performing data");

            IList<Product> listProducts = new List<Product>() {new("Café 1"), new("Farinha 2"), new("Água 3")};

            await SeedDataPurchase(databaseContext, listProducts);
            await databaseContext.CommitAsync();

            await SeedDataAdjustment(databaseContext, listProducts);
            await databaseContext.CommitAsync();

            await SeedDataPurchase(databaseContext, listProducts);
            await databaseContext.CommitAsync();

            await SeedDataPurchase(databaseContext, listProducts);
            await databaseContext.CommitAsync();

            await SeedDataOutput(databaseContext, listProducts);
            await databaseContext.CommitAsync();
        }

        private static async Task SeedDataAdjustment(DatabaseContext databaseContext, IList<Product> listProducts)
        {
            Adjustment adjustment = new($"Ajuste de estoque - {DateTime.Now:d}", DateTime.Now);

            Random r = new();

            foreach (Product product in listProducts)
                adjustment.AddItem(new AdjustmentItem(r.Next(1, 10), r.Next(10, 100), adjustment, product));

            adjustment.AddEvent(new AdjustmentClosedEvent(adjustment.Id));

            await databaseContext.Set<Adjustment>().AddAsync(adjustment);
        }

        private static async Task SeedDataPurchase(DatabaseContext databaseContext, IList<Product> listProducts)
        {
            Purchase purchase = new($"Compra de produtos - {DateTime.Now:d}", DateTime.Now);

            Random r = new();

            foreach (Product product in listProducts)
                purchase.AddItem(new PurchaseItem(r.Next(1, 10), r.Next(10, 100), purchase, product));

            purchase.AddEvent(new PurchaseClosedEvent(purchase.Id));

            await databaseContext.Set<Purchase>().AddAsync(purchase);
        }

        private static async Task SeedDataOutput(DatabaseContext databaseContext, IList<Product> listProducts)
        {
            Output output = new($"Saída de produtos - {DateTime.Now:d}", DateTime.Now);

            Random r = new();

            foreach (Product product in listProducts)
                output.AddItem(new OutputItem(r.Next(1, 10), r.Next(10, 100), output, product));

            output.AddEvent(new OutputClosedEvent(output.Id));

            await databaseContext.Set<Output>().AddAsync(output);
        }
    }
}
