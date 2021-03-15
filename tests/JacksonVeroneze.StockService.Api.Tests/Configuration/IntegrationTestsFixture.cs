using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Api.Tests.Configuration.AuthorizationToken;
using JacksonVeroneze.StockService.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace JacksonVeroneze.StockService.Api.Tests.Configuration
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupTests>>
    {
    }

    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        private readonly AppFactory<TStartup> _factory;
        public readonly HttpClient Client;

        public IntegrationTestsFixture()
        {
            WebApplicationFactoryClientOptions clientOptions = new()
            {
                AllowAutoRedirect = true, MaxAutomaticRedirections = 7
            };

            _factory = new AppFactory<TStartup>();

            Client = _factory.CreateClient(clientOptions);

            Client.AddJsonMediaType();

            ResponseToken token = FindToken.Find().Result;

            Client.AddJwtToken(token.AccessToken);
        }

        public async Task MockInDatabase<T>(T entity) where T : class
        {
            using IServiceScope scope = _factory.Services.CreateScope();

            IServiceProvider scopedServices = scope.ServiceProvider;

            DatabaseContext context = scopedServices.GetRequiredService<DatabaseContext>();

            await context.Database.EnsureDeletedAsync();

            await context.Set<T>().AddAsync(entity);

            await context.SaveChangesAsync();
        }

        public async Task MockInDatabase<T>(IList<T> entity) where T : class
        {
            using IServiceScope scope = _factory.Services.CreateScope();

            IServiceProvider scopedServices = scope.ServiceProvider;

            DatabaseContext context = scopedServices.GetRequiredService<DatabaseContext>();

            await context.Database.EnsureDeletedAsync();

            await context.Set<T>().AddRangeAsync(entity);

            await context.SaveChangesAsync();
        }

        public async Task ClearDatabase()
        {
            using IServiceScope scope = _factory.Services.CreateScope();

            IServiceProvider scopedServices = scope.ServiceProvider;

            DatabaseContext context = scopedServices.GetRequiredService<DatabaseContext>();

            await context.Database.EnsureDeletedAsync();
        }

        public async Task<T> DeserializeObject<T>(HttpResponseMessage response)
            => await response.Content.ReadFromJsonAsync<T>();

        public void Dispose()
        {
            _factory?.Dispose();
            Client?.Dispose();
        }
    }
}
