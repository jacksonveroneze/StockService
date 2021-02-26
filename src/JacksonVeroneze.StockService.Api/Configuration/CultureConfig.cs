using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class CultureConfig
    {
        public static IApplicationBuilder UseCultureSetup(this IApplicationBuilder app)
        {
            CultureInfo[] supportedCultures = {new CultureInfo("pt-BR")};
            
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR", "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            return app;
        }
    }
}
