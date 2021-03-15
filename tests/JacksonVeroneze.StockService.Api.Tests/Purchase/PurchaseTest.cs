using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using JacksonVeroneze.StockService.Api.Controllers.v1;
using JacksonVeroneze.StockService.Api.Tests.Configuration;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Common.Integration;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace JacksonVeroneze.StockService.Api.Tests.Purchase
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class PurchaseTest
    {
        private const string _uriPart = "/api/v1/purchases";

        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public PurchaseTest(IntegrationTestsFixture<StartupTests> testsFixture)
            => _testsFixture = testsFixture;

        [Fact(DisplayName = "DeveFiltrarEPaginarOsDadosComSkipTakeCorretamente")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Filter))]
        public async Task PurchasesController_Filter_DeveFiltrarEPaginarOsDadosComSkipTakeCorretamente()
        {
            // Arrange
            int skip = 1;
            int take = 5;

            IList<Domain.Entities.Purchase> products = PurchaseFaker
                .GenerateFaker()
                .Generate(30)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            await _testsFixture.MockInDatabase(products);

            // Act
            HttpResponseMessage response =
                await _testsFixture.Client.GetAsync($"{_uriPart}?skip={skip}&take={take}&description=a");

            TestApiResponsePageable<PurchaseDto> result =
                await _testsFixture.DeserializeObject<TestApiResponsePageable<PurchaseDto>>(response);

            // Assert
            IList<Domain.Entities.Purchase> productsFiltered =
                products.Where(x => x.Description.Contains("a")).Skip(skip).Take(take).ToList();

            int total = products.Count(x => x.Description.Contains("a"));

            result.Should().NotBeNull();
            result.Total.Should().Be(total);
            result.Pages.Should().Be((int)Math.Ceiling(total / (decimal)(take)));
            result.CurrentPage.Should().Be(skip);
            result.Data.Should().HaveCount(productsFiltered.Count);
        }

        [Fact(DisplayName = "DeveBuscarCorretamentePeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait("ProductController", "Find")]
        public async Task ProductController_Find_DeveBuscarCorretamentePeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            IList<Domain.Entities.Purchase> purchases = PurchaseFaker.GenerateFaker().Generate(10);

            Domain.Entities.Purchase purchase = purchases.First();

            await _testsFixture.MockInDatabase(purchases);

            // Act
            HttpResponseMessage response = await _testsFixture.Client.GetAsync($"{_uriPart}/{purchase.Id}");

            PurchaseDto result = await _testsFixture.DeserializeObject<PurchaseDto>(response);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(purchase.Id);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode404QuandoNaoEstiverCadastrado")]
        [Trait("ProductController", "Find")]
        public async Task ProductController_Find_DeveRetornarStatusCode404QuandoNaoEstiverCadastrado()
        {
            // Arrange && Act
            HttpResponseMessage response =
                await _testsFixture.Client.GetAsync($"{_uriPart}/{Guid.NewGuid()}");

            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<PurchaseDto>>(response);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Status.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact(DisplayName = "DeveSalvarCorretamenteQuandoEmEstadoValido")]
        [Trait("ProductController", "Create")]
        public async Task ProductController_Create_DeveSalvarCorretamenteQuandoEmEstadoValido()
        {
            await _testsFixture.ClearDatabase();

            // Arrange
            AddOrUpdatePurchaseDto purchaseDto = AddOrUpdatePurchaseDtoFaker.GenerateValidFaker().Generate();

            // Act
            HttpResponseMessage response = await _testsFixture.Client.PostAsJsonAsync($"{_uriPart}/", purchaseDto);

            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<PurchaseDto>>(response);

            Uri location = response.Headers.Location;

            HttpResponseMessage responseGet = await _testsFixture.Client.GetAsync(location);

            PurchaseDto resultGet = await _testsFixture.DeserializeObject<PurchaseDto>(responseGet);

            // Assert
            result.Should().NotBeNull();
            result.Data.Description.Should().Be(purchaseDto.Description);
            result.Errors.Should().BeEmpty();

            resultGet.Should().NotBeNull();
            resultGet.Description.Should().Be(purchaseDto.Description);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarSalvarEmEstadoInvalido")]
        [Trait("ProductController", "Create")]
        public async Task ProductController_Create_DeveRetornarErro400QuandoTentarSalvarEmEstadoInvalido()
        {
            // Arrange
            AddOrUpdatePurchaseDto purchaseDto = AddOrUpdatePurchaseDtoFaker.GenerateInvalidFaker().Generate();

            // Act
            HttpResponseMessage response = await _testsFixture.Client.PostAsJsonAsync($"{_uriPart}/", purchaseDto);

            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<PurchaseDto>>(response);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
