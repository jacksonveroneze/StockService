using JacksonVeroneze.StockService.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JacksonVeroneze.StockService.Api
{
    public class StartupTests
    {
        private IConfiguration Configuration { get; }

        private IHostEnvironment HostEnvironment { get; }

        public StartupTests(IHostEnvironment hostEnvironment)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables("STOCK_CONFIG_");

            Configuration = builder.Build();

            HostEnvironment = hostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
            => services.AddApiConfigurationTests(Configuration, HostEnvironment);

        public void Configure(IApplicationBuilder app)
            => app.UseApiConfigurationTests();
    }
}
