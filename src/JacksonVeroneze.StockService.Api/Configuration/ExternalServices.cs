using System;
using System.Net.Http;
using System.Security.Authentication;
using JacksonVeroneze.StockService.AntiCorruption;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Refit;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class ExternalServices
    {
        public static IServiceCollection AddExternalServicesConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(10, _ => TimeSpan.FromSeconds(2), (_, _, retryCount, _) =>
                    Console.WriteLine($"Retry number: {retryCount}")
                );

            services.AddRefitClient<IMailService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration["UrlMailService"]))
                .ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler
                {
                    AllowAutoRedirect = true,
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
                    SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12
                })
                .AddPolicyHandler(retryPolicy);

            return services;
        }
    }
}
