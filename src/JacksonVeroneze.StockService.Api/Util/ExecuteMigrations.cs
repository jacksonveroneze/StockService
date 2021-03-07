using System.Threading.Tasks;
using JacksonVeroneze.StockService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace JacksonVeroneze.StockService.Api.Util
{
    public static class ExecuteMigrations
    {
        public static async Task Execute(IHost host)
        {
            Log.Information("Migrations: {0}", "Performing migrations");

            using IServiceScope scope = host.Services.CreateScope();

            DatabaseContext databaseContext =
                scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            await databaseContext.Database.EnsureDeletedAsync();

            await databaseContext.Database.MigrateAsync();
        }
    }
}
