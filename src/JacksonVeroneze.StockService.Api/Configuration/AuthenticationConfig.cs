using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class AuthenticationConfig
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration["Auth:Authority"]) ||
                string.IsNullOrEmpty(configuration["Auth:Audience"]))
                throw new ArgumentException("Configuração do JWT não definida corretamente.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = configuration["Auth:Authority"];
                options.Audience = configuration["Auth:Audience"];
            });

            return services;
        }
    }
}
