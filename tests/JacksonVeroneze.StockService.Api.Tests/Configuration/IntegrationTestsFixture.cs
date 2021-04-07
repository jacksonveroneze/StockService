using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Api.Tests.Configuration.AuthorizationToken;
using JacksonVeroneze.StockService.Common.Integration;
using JacksonVeroneze.StockService.Infra.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace JacksonVeroneze.StockService.Api.Tests.Configuration
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupTests>>
    {
    }

    /// <summary>
    /// Class responsible for fixture
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly AppFactory<TStartup> _factory;
        private readonly HttpClient _client;
        private readonly DatabaseContext _context;

        /// <summary>
        /// Method responsible for initialize fixture.
        /// </summary>
        public IntegrationTestsFixture()
        {
            WebApplicationFactoryClientOptions clientOptions = new() {AllowAutoRedirect = true, MaxAutomaticRedirections = 7};

            _factory = new AppFactory<TStartup>();

            _client = _factory.CreateClient(clientOptions);

            _client.AddJsonMediaType();

            ResponseToken token = FindToken.FindAsync().Result;

            _client.AddJwtToken(token.AccessToken);

            _context = _factory.Services.GetRequiredService<DatabaseContext>();
        }

        /// <summary>
        /// Method responsible for run migrations.
        /// </summary>
        /// <returns></returns>
        public async Task RunMigrations()
            => await _context.Database.MigrateAsync();

        /// <summary>
        /// Method responsible for mock data in database.
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task MockInDatabase<T>(T entity) where T : class
        {
            await _context.Set<T>().AddAsync(entity);

            await _context.CommitAsync();
        }

        /// <summary>
        /// Method responsible for mock data in database.
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task MockInDatabase<T>(IList<T> entity) where T : class
        {
            await _context.Set<T>().AddRangeAsync(entity);

            await _context.CommitAsync();
        }

        /// <summary>
        /// Method responsible for remove data.
        /// </summary>
        /// <returns></returns>
        public async Task ClearDatabase()
            => await _context.Database.EnsureDeletedAsync();

        /// <summary>
        /// Method responsible for deserialize data.
        /// </summary>
        /// <param name="response"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private async Task<T> DeserializeObject<T>(HttpResponseMessage response)
            => await response.Content.ReadFromJsonAsync<T>();

        /// <summary>
        /// Method responsible for send request.
        /// </summary>
        /// <param name="url"></param>
        /// <typeparam name="TResponseType"></typeparam>
        /// <returns></returns>
        public async Task<TestApiResponseOperationGet<TResponseType>> SendGetRequest<TResponseType>(string url)
            where TResponseType : class
        {
            HttpResponseMessage resultHttp = await _client.GetAsync(url);

            if (resultHttp.IsSuccessStatusCode)
            {
                return new TestApiResponseOperationGet<TResponseType>() {Content = await DeserializeObject<TResponseType>(resultHttp), HttpResponse = resultHttp};
            }

            TestApiResponseOperationGet<TResponseType> result =
                await DeserializeObject<TestApiResponseOperationGet<TResponseType>>(resultHttp);

            result.HttpResponse = resultHttp;

            return result;
        }

        /// <summary>
        /// Method responsible for send request.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dto"></param>
        /// <typeparam name="TResponseType"></typeparam>
        /// <returns></returns>
        public async Task<TestApiResponseOperations<TResponseType>> SendPostRequest<TInputType,
            TResponseType>(string url, TInputType dto)
            where TInputType : class where TResponseType : class
        {
            HttpResponseMessage resultHttp = await _client.PostAsJsonAsync(url, dto);

            TestApiResponseOperations<TResponseType> result =
                await DeserializeObject<TestApiResponseOperations<TResponseType>>(resultHttp);

            result.HttpResponse = resultHttp;

            return result;
        }

        /// <summary>
        /// Method responsible for send request.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dto"></param>
        /// <typeparam name="TResponseType"></typeparam>
        /// <returns></returns>
        public async Task<TestApiResponseOperations<TResponseType>> SendPutRequest<TInputType, TResponseType>(
            string url, TInputType dto) where TInputType : class where TResponseType : class
        {
            HttpResponseMessage resultHttp = await _client.PutAsJsonAsync(url, dto);

            TestApiResponseOperations<TResponseType> result =
                await DeserializeObject<TestApiResponseOperations<TResponseType>>(resultHttp);

            result.HttpResponse = resultHttp;

            return result;
        }

        /// <summary>
        /// Method responsible for send request.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<TestApiResponseBase> SendPutEmptyBodyRequest(string url)
        {
            HttpResponseMessage resultHttp = await _client.PutAsync(url, null);

            TestApiResponseBase result = new();

            if (!resultHttp.IsSuccessStatusCode)
                result = await DeserializeObject<TestApiResponseBase>(resultHttp);

            result.HttpResponse = resultHttp;

            return result;
        }

        /// <summary>
        /// Method responsible for send request.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<TestApiResponseBase> SendDeleteRequest(string url)
        {
            HttpResponseMessage resultHttp = await _client.DeleteAsync(url);

            TestApiResponseBase result = new();

            if (!resultHttp.IsSuccessStatusCode)
                result = await DeserializeObject<TestApiResponseBase>(resultHttp);

            result.HttpResponse = resultHttp;

            return result;
        }

        public void Dispose()
        {
            _factory?.Dispose();
            _client?.Dispose();
        }
    }
}
