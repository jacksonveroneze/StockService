using JacksonVeroneze.StockService.Api.Middlewares.ErrorHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

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
                .AddAutoMediatRConfiguration()
                .AddDependencyInjectionConfiguration()
                .AddSwaggerConfiguration()
                .AddMassTransitConfiguration(configuration)
                .AddApplicationInsightsConfiguration(configuration)
                .AddAuthenticationConfiguration(configuration)
                .AddAuthorizationConfiguration(configuration)
                .AddVersioningConfigConfiguration()
                .AddControllers()
                .AddJsonOptionsSerializeConfiguration();

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider)
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
                .UseEndpoints(endpoints => { endpoints.MapControllers(); });

            return app;
        }
    }
}
