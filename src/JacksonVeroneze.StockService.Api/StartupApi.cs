using JacksonVeroneze.StockService.Api.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JacksonVeroneze.StockService.Api
{
    public sealed class StartupApi : BaseStartup
    {
        public StartupApi(IHostEnvironment hostEnvironment) : base(hostEnvironment)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
            => services.AddApiConfiguration(Configuration, HostEnvironment)
                .AddDatabaseConfiguration(Configuration, HostEnvironment);
    }
}
