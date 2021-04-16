using System;
using System.Diagnostics;
using JacksonVeroneze.StockService.Api.Middlewares.ErrorHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using HealthChecks.UI.Client;
using Microsoft.ApplicationInsights.DependencyCollector;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class ApiConfig
    {
        private const string CorsPolicyName = "CorsPolicy";

        public static IServiceCollection AddApiConfiguration(this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            services.AddRouting(options => options.LowercaseUrls = true)
                .AddCorsConfiguration(configuration, CorsPolicyName)
                .HealthChecksConfiguration()
                .AddAutoMapperConfiguration()
                .AddAutoMapperConfigurationValid()
                .AddDependencyInjectionConfiguration()
                .AddSwaggerConfiguration()
                .AddAutoMediatRConfiguration()
                .AddBusConfiguration(configuration)
                .AddApplicationInsightsConfiguration(configuration)
                .AddOpenTelemetryTracingConfiguration(configuration, hostEnvironment)
                .AddAuthenticationConfiguration(configuration)
                .AddAuthorizationConfiguration(configuration)
                .AddVersioningConfigConfiguration()
                .AddControllers()
                .AddJsonOptionsSerializeConfiguration();

            services.AddHttpClient("viacep", c =>
            {
                c.BaseAddress = new Uri("https://viacep.com.br/ws/01001000/json");
                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            });

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider, IConfiguration configuration)
        {
            app.UseCultureSetup()
                .UseCors(CorsPolicyName)
                .UseHealthChecksSetup()
                .UseSerilogRequestLogging()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseMiddleware<ErrorHandlingMiddleware>()
                .UseSwaggerSetup(provider)
                .UseElasticApmSetup(configuration)
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHealthChecks("/hc",
                        new HealthCheckOptions
                        {
                            Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                        });
                });

            return app;
        }
    }
}
