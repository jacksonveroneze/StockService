using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class OpenTelemetryTracingConfig
    {
        public static IServiceCollection AddOpenTelemetryTracingConfiguration(this IServiceCollection services,
            IConfiguration configuration, IHostEnvironment hostEnvironment)
            => services.AddOpenTelemetryTracing(
                builder => builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(hostEnvironment.ApplicationName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation(options => { options.SetTextCommandContent = true; })
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = configuration["Jaeger:AgentHost"];
                        options.AgentPort = Convert.ToInt32(configuration["Jaeger:AgentPort"]);
                    })
                    .AddConsoleExporter());
    }
}
