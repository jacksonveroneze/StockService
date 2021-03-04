using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "StockService Api",
                        Version = "v1",
                        Description = "StockService",
                        Contact = new OpenApiContact {Name = "Jackson Veroneze", Email = "jackson@jacksonveroneze.com"},
                        License = new OpenApiLicense {Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT")}
                    }
                );

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Input the JWT like: Bearer {your token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {{new OpenApiSecurityScheme {Reference = new OpenApiReference
                        {Type = ReferenceType.SecurityScheme, Id = "Bearer"}}, new string[] { }}});
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerSetup(this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = String.Empty;

                foreach (var description in provider.ApiVersionDescriptions)
                    c.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
            });

            return app;
        }
    }
}
