using JacksonVeroneze.StockService.Api.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JacksonVeroneze.StockService.Api
{
    public sealed class StartupTests : BaseStartup
    {
        public StartupTests(IHostEnvironment hostEnvironment) : base(hostEnvironment)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
            => services.AddApiConfiguration(Configuration, HostEnvironment)
                .AddDatabaseTestsConfiguration(Configuration);
    }
}
