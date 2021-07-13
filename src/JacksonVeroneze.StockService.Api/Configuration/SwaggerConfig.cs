using System;
using System.IO;
using System.Reflection;
using JacksonVeroneze.NET.Commons.Swagger;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            return services.AddSwaggerConfiguration(x =>
            {
                x.Title = "StockService Api";
                x.Version = "v1";
                x.Description = "StockService";
                x.ContactName = "Jackson Veroneze";
                x.ContactEmail = "jackson@jacksonveroneze.com";
                x.IncludeXmlComments = true;
                x.XmlCommentsPath = xmlPath;
            });
        }
    }
}
