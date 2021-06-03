using JacksonVeroneze.NET.Commons.Cors;
using JacksonVeroneze.StockService.Api.Middlewares.ErrorHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using JacksonVeroneze.NET.Commons.Culture;
using JacksonVeroneze.NET.Commons.HealthCheck;
using JacksonVeroneze.NET.Commons.Routing;
using JacksonVeroneze.NET.Commons.Swagger;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            services.AddRoutingConfiguration()
                .AddCorsConfiguration(configuration)
                .AddHealthCheckConfiguration()
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
                .AddExternalServicesConfiguration(configuration)
                .AddVersioningConfigConfiguration()
                .AddControllers()
                .AddJsonOptionsSerializeConfiguration();

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider, IConfiguration configuration)
        {
            app.UseCultureConfiguration()
                .UseCorsConfiguration()
                .UseHealthCheckConfiguration()
                .UseSerilogRequestLogging()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseMiddleware<ErrorHandlingMiddleware>()
                .UseSwaggerConfiguration(provider)
                .UseEndpoints(endpoints => endpoints.MapControllers());

            return app;
        }
    }
}
