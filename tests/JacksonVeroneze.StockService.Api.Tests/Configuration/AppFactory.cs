using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace JacksonVeroneze.StockService.Api.Tests.Configuration
{
    public class AppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            // Log.Logger = new LoggerConfiguration()
            //     .WriteTo.Console(
            //         outputTemplate:
            //         "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
            //         theme: AnsiConsoleTheme.Literate)
            //     .Enrich.FromLogContext()
            //     .CreateLogger();

            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>()
                        .UseEnvironment("Testing")
                        .UseSerilog()
                        .ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.AddConsole();
                        });
                });
        }
    }
}
