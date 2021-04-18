using System;
using JacksonVeroneze.NET.Commons.OpenTelemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class OpenTelemetryTracingConfig
    {
        public static IServiceCollection AddOpenTelemetryTracingConfiguration(this IServiceCollection services,
            IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            if (string.IsNullOrEmpty(configuration["Jaeger:AgentHost"]) is false &&
                string.IsNullOrEmpty(configuration["Jaeger:AgentPort"]) is false)
                services.AddOpenTelemetryTracingConfiguration(x =>
                {
                    x.ApplicationName = hostEnvironment.ApplicationName;
                    x.JaegerAgentHost = configuration["Jaeger:AgentHost"];
                    x.JaegerAgentPort = Convert.ToInt32(configuration["Jaeger:AgentPort"]);
                });

            return services;
        }
    }
}
