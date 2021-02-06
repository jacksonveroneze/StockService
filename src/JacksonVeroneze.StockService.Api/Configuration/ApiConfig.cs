using JacksonVeroneze.StockService.Api.Middlewares;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class ApiConfig
    {
        private const string CorsPolicyName = "AllowAll";

        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddHealthChecks();

            services.AddCorsConfiguration(CorsPolicyName);

            services.AddAutoMapperConfiguration();

            services.AddDatabaseConfiguration(configuration);

            services.AddAutoMediatRConfiguration();

            services.AddDependencyInjectionConfiguration();

            services.AddSwaggerConfiguration();

            services.AddControllers();

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCultureSetup();

            app.UseHealthChecks("/health");

            app.UseMetricServer();

            app.UseHttpMetrics();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(CorsPolicyName);

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseSwaggerSetup();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            return app;
        }
    }
}
