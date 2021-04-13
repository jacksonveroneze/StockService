using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Api.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace JacksonVeroneze.StockService.Api
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Log.Logger = Logger.FactoryLogger();

                Log.Information($"Application: {0}", "Starting up");
                Log.Information($"Total params: {0}", args.Length);

                IHost host = CreateHostBuilder(args).Build();

                await ExecuteMigrations.Execute(host, args);

                await host.RunAsync();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<StartupApi>();
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
