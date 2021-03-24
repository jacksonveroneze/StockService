using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace JacksonVeroneze.StockService.Api.Tests.Configuration.AuthorizationToken
{
    public static class FindToken
    {
        public static async Task<ResponseToken> FindAsync()
        {
            RequestToken requestToken = new()
            {
                ClientId = "dHBnIudUunKw8pYQPcdjJr40AOJyU1lt",
                ClientSecret = "ljz4vCOt3kvWgiF-0_P_kgQX_eP6hMlebMUeWR6_hy9TzZ5nVAUuXkRhtWhDC6mu",
                Audience = "https://stock-jacksonveroneze.azurewebsites.net",
                GrantType = "client_credentials",
            };

            HttpClient httpClient = new();
            httpClient.BaseAddress = new Uri("https://jacksonveroneze.auth0.com/");

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("oauth/token", requestToken);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ResponseToken>();

            return default;
        }
    }
}
