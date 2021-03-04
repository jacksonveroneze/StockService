using JacksonVeroneze.StockService.Api.Middlewares.ErrorHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class ApiConfigTests
    {
        private const string CorsPolicyName = "CorsPolicy";

        public static IServiceCollection AddApiConfigurationTests(this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            services.AddRouting(options => options.LowercaseUrls = true)
                .AddAutoMapperConfiguration()
                .AddAutoMapperConfigurationValid()
                .AddDatabaseConfigurationTests()
                .AddAutoMediatRConfiguration()
                .AddDependencyInjectionConfiguration()
                .AddAuthenticationConfiguration(configuration)
                .AddVersioningConfigConfiguration()
                .AddControllers();

            return services;
        }

        public static IApplicationBuilder UseApiConfigurationTests(this IApplicationBuilder app)
        {
            app.UseCultureSetup()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseMiddleware<ErrorHandlingMiddleware>()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); });

            return app;
        }
    }
}
