using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class JsonOptionsSerializeConfig
    {
        public static void AddJsonOptionsSerializeConfiguration(this IMvcBuilder services)
            => services.AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);
    }
}
