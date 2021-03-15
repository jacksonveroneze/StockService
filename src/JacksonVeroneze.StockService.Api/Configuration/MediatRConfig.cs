using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class MediatRConfig
    {
        public static IServiceCollection AddAutoMediatRConfiguration(this IServiceCollection services)
            => services.AddMediatR(
                AppDomain.CurrentDomain.Load("JacksonVeroneze.StockService.Domain"));
    }
}
