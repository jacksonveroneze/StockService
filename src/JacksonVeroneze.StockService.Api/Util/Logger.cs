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
            return new LoggerConfiguration()
                .ReadFrom.Configuration(FactoryConfiguration())
                .Enrich.WithProperty("ApplicationName", "Stock Service")
                .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithDemystifiedStackTraces()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}{NewLine}{Message:lj} {Properties:j}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate)
                .Enrich.FromLogContext()
                .CreateLogger();
        }

        private static IConfigurationRoot FactoryConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            bool isDevelopment = IsDevelopmentEnvironment();

            if (isDevelopment && File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")))
                builder.AddJsonFile("appsettings.json", true, true);

            if (isDevelopment &&
                File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json")))
                builder.AddJsonFile("appsettings.Development.json", true, true);

            return builder
                .AddEnvironmentVariables("APP_CONFIG_")
                .Build();
        }

        private static bool IsDevelopmentEnvironment()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return environment != null &&
                   environment.Equals("Development", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
