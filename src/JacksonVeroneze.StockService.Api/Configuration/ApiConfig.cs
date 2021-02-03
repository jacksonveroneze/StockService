using System.Globalization;
using JacksonVeroneze.StockService.Api.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class ApiConfig
    {
        private const string AllowAllCors = "AllowAll";

        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddHealthChecks();

            services.AddCors(options =>
            {
                options.AddPolicy(AllowAllCors,
                    builder =>
                    {
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                        builder.AllowAnyOrigin();
                    });
            });

            services.AddControllers();

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            CultureInfo[] supportedCultures = {new CultureInfo("pt-BR")};
            app.UseRequestLocalization(new RequestLocalizationOptions {DefaultRequestCulture = new RequestCulture("pt-BR", "pt-BR"), SupportedCultures = supportedCultures, SupportedUICultures = supportedCultures});

            app.UseHttpsRedirection();

            app.UseHealthChecks("/health");

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(AllowAllCors);

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            return app;
        }
    }
}
