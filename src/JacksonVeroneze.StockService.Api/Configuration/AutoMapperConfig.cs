using JacksonVeroneze.StockService.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
            => services.AddAutoMapper(typeof(ProfileMapStock));
    }
}
