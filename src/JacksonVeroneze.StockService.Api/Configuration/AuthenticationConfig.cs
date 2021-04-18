using JacksonVeroneze.NET.Commons.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class AuthenticationConfig
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services,
            IConfiguration configuration)
            => services.AddAuthenticationConfiguration(x =>
            {
                x.Authority = configuration["Auth:Authority"];
                x.Audience = configuration["Auth:Audience"];
            });
    }
}
