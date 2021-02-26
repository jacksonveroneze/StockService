using AutoMapper;
using JacksonVeroneze.StockService.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
            => services.AddAutoMapper((cfg) =>
            {
                cfg.AddProfile<ProfileMapStock>();
                cfg.ForAllMaps((_, me) => me.IgnoreAllPropertiesWithAnInaccessibleSetter());
            });

        public static IServiceCollection AddAutoMapperConfigurationValid(this IServiceCollection services)
        {
            services.BuildServiceProvider().GetService<IMapper>()
                ?.ConfigurationProvider
                .AssertConfigurationIsValid();

            return services;
        }
    }
}
