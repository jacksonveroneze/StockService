using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using JacksonVeroneze.NET.Commons.Logger;
using JacksonVeroneze.StockService.Api.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace JacksonVeroneze.StockService.Api
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Activity.DefaultIdFormat = ActivityIdFormat.W3C;
                Activity.ForceDefaultIdFormat = true;

                string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                Log.Logger = FactoryLogger(environment);

                Log.Information($"Application: {0}", "Starting up");

                IHost host = CreateHostBuilder(args).Build();

                //await MigrateDatabase(host, args, environment);

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
                    webBuilder.UseSerilog();
                });

        private static ILogger FactoryLogger(string environment)
        {
            return Logger.FactoryLogger(x =>
            {
                x.Environment = environment;
                x.ApplicationName = "Stock Service";
                x.CurrentDirectory = Directory.GetCurrentDirectory();
            });
        }

        private static async Task MigrateDatabase(IHost host, string[] args, string environment)
        {
            if (environment != null &&
                !environment.Equals("Production", StringComparison.InvariantCultureIgnoreCase))
            {
                await ExecuteMigrations.Execute(host, args);

                Log.Information("Executed database migrate");
            }
        }
    }
}
