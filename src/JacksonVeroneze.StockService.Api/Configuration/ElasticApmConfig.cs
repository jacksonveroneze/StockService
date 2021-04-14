using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class ElasticApmConfig
    {
        public static IApplicationBuilder UseElasticApmSetup(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration["ElasticApm:SecretToken"]) is false &&
                string.IsNullOrEmpty(configuration["ElasticApm:ServerUrls"]) is false)
                app.UseAllElasticApm(configuration);

            return app;
        }
    }
}
