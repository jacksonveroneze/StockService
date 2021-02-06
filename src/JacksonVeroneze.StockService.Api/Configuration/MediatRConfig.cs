using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class MediatRConfig
    {
        public static void AddAutoMediatRConfiguration(this IServiceCollection services)
            => services.AddMediatR(typeof(Startup));
    }
}
