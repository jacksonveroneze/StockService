using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace JacksonVeroneze.StockService.Api.Util
{
    public class Logger
    {
        public static ILogger FactoryLogger()
        {
            return new LoggerConfiguration()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate)
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }
}
