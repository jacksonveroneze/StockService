using JacksonVeroneze.StockService.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JacksonVeroneze.StockService.Api
{
    public class Startup : BaseStartup
    {
        public Startup(IHostEnvironment hostEnvironment) : base(hostEnvironment)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
            => services.AddApiConfiguration(Configuration, HostEnvironment);

        public override void Configure(IApplicationBuilder app)
            => app.UseApiConfiguration();
    }
}
