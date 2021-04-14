using JacksonVeroneze.StockService.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JacksonVeroneze.StockService.Api
{
    public abstract class BaseStartup
    {
        protected IConfiguration Configuration { get; }

        protected IHostEnvironment HostEnvironment { get; }

        protected BaseStartup(IHostEnvironment hostEnvironment)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath);

            if (!hostEnvironment.IsProduction())
                builder.AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true);

            builder.AddEnvironmentVariables("APP_CONFIG_");

            Configuration = builder.Build();

            HostEnvironment = hostEnvironment;
        }

        public virtual void ConfigureServices(IServiceCollection services)
            => services.AddApiConfiguration(Configuration, HostEnvironment);

        public virtual void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
            => app.UseApiConfiguration(provider, Configuration);
    }
}
