using System.Net.Http;
using System.Net.Http.Headers;

namespace JacksonVeroneze.StockService.Api.Tests.Configuration
{
    public static class TestsExtensions
    {
        public static void AddJwtToken(this HttpClient client, string token)
        {
            client.AddJsonMediaType();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static void AddJsonMediaType(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
