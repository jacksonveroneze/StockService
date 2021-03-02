using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class CultureConfig
    {
        public static IApplicationBuilder UseCultureSetup(this IApplicationBuilder app)
            => app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR", "pt-BR"),
                SupportedCultures = {new CultureInfo("pt-BR")},
                SupportedUICultures = {new CultureInfo("pt-BR")}
            });
    }
}
