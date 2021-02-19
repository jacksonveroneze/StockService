using JacksonVeroneze.StockService.Api.Util;
using JacksonVeroneze.StockService.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;


namespace JacksonVeroneze.StockService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = Logger.FactoryLogger();

            Log.Information("Application: {0}", "Starting up");

            IHost host = CreateHostBuilder(args).Build();

            Log.Information("Migrations: {0}", "Performing migrations");

            using IServiceScope scope = host.Services.CreateScope();

            DatabaseContext db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            db.Database.Migrate();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog()
                        .ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.AddConsole();
                            logging.AddAzureWebAppDiagnostics();
                        });
                });
    }
}
