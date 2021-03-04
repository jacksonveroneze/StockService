using JacksonVeroneze.StockService.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JacksonVeroneze.StockService.Api
{
    public class StartupTests : BaseStartup
    {
        public StartupTests(IHostEnvironment hostEnvironment) : base(hostEnvironment)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
            => services.AddApiConfigurationTests(Configuration, HostEnvironment);

        public override void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
            => app.UseApiConfigurationTests();
    }
}
