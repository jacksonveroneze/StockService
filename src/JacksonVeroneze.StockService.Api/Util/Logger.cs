using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace JacksonVeroneze.StockService.Api.Util
{
    public static class Logger
    {
        public static ILogger FactoryLogger()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment != null && environment.Equals("Development", StringComparison.CurrentCultureIgnoreCase))
                builder.AddJsonFile("appsettings.json", true, true);

            IConfigurationRoot configurat = builder
                .AddEnvironmentVariables("APP_CONFIG_")
                .Build();

            return new LoggerConfiguration()
                .ReadFrom.Configuration(configurat)
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate)
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }
}
