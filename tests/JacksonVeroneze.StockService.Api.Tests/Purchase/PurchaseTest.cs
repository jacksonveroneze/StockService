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
using JacksonVeroneze.StockService.Application.Validations;
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
        {
            _testsFixture = testsFixture;

            Task.Run(async () => await _testsFixture.ClearDatabase());
        }

        [Fact(DisplayName = "DeveFiltrarEPaginarOsDadosComSkipTakeCorretamente", Skip = "Erro")]
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
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Find))]
        public async Task PurchasesController_Find_DeveBuscarCorretamentePeloIdQuandoOMesmoEstiverCadastrado()
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
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Find))]
        public async Task PurchasesController_Find_DeveRetornarStatusCode404QuandoNaoEstiverCadastrado()
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
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Create))]
        public async Task PurchasesController_Create_DeveSalvarCorretamenteQuandoEmEstadoValido()
        {
            // Arrange
            await _testsFixture.ClearDatabase();

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
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Create))]
        public async Task PurchasesController_Create_DeveRetornarErro400QuandoTentarSalvarEmEstadoInvalido()
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

        [Fact(DisplayName = "DeveAtualizarCorretamenteQuandoEmEstadoValido")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Update))]
        public async Task PurchasesController_Update_DeveAtualizarCorretamenteQuandoEmEstadoValido()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateFaker().Generate();

            await _testsFixture.MockInDatabase(purchase);

            AddOrUpdatePurchaseDto purchasetDto = new() {Description = $"{purchase.Description}_atualizado"};

            // Act
            HttpResponseMessage response =
                await _testsFixture.Client.PutAsJsonAsync($"{_uriPart}/{purchase.Id}", purchasetDto);

            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<PurchaseDto>>(response);

            HttpResponseMessage responseGet = await _testsFixture.Client.GetAsync($"{_uriPart}/{purchase.Id}");

            PurchaseDto resultGet = await _testsFixture.DeserializeObject<PurchaseDto>(responseGet);

            // Assert
            result.Should().NotBeNull();
            result.Data.Description.Should().Be(purchasetDto.Description);
            result.Errors.Should().BeEmpty();

            resultGet.Should().NotBeNull();
            resultGet.Description.Should().Be(purchasetDto.Description);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarEmEstadoInvalido")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Update))]
        public async Task PurchasesController_Update_DeveRetornarErro400QuandoTentarAtualizarEmEstadoInvalido()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateFaker().Generate();

            await _testsFixture.MockInDatabase(purchase);

            AddOrUpdatePurchaseDto purchasetDto = new() {Description = string.Empty};

            // Act
            HttpResponseMessage response =
                await _testsFixture.Client.PutAsJsonAsync($"{_uriPart}/{purchase.Id}", purchasetDto);

            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<PurchaseDto>>(response);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarUmItemInexistente")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Update))]
        public async Task PurchasesController_Update_DeveRetornarErro400QuandoTentarAtualizarUmItemInexistente()
        {
            // Arrange
            AddOrUpdatePurchaseDto purchasetDto = AddOrUpdatePurchaseDtoFaker.GenerateValidFaker().Generate();

            // Act
            HttpResponseMessage response =
                await _testsFixture.Client.PutAsJsonAsync($"{_uriPart}/{Guid.NewGuid()}", purchasetDto);

            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<PurchaseDto>>(response);

            // Assert
            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseNotFoundById));
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteQuandoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Delete))]
        public async Task PurchasesController_Delete_DeveRemoverCorretamenteQuandoEstiverCadastrado()
        {
            // Arrange
            IList<Domain.Entities.Purchase> purchases = PurchaseFaker.GenerateFaker().Generate(10);

            Domain.Entities.Purchase purchase = purchases.First();

            await _testsFixture.MockInDatabase(purchases);

            // Act
            HttpResponseMessage response = await _testsFixture.Client.DeleteAsync($"{_uriPart}/{purchase.Id}");

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode404QuandoRemoverENaoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Delete))]
        public async Task PurchasesController_Delete_DeveRetornarStatusCode404QuandoRemoverENaoEstiverCadastrado()
        {
            // Arrange && Act
            HttpResponseMessage response =
                await _testsFixture.Client.DeleteAsync($"{_uriPart}/{Guid.NewGuid()}");

            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<PurchaseDto>>(response);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverETiverDependentes")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Delete))]
        public async Task PurchasesController_Delete_DeveRetornarStatusCode400QuandoRemoverETiverDependentes()
        {
            // Arrange
            Domain.Entities.Product product = ProductFaker.GenerateFaker().Generate();
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateFaker().Generate();
            Domain.Entities.PurchaseItem purchaseItem = PurchaseItemFaker.GenerateFaker(purchase, product).Generate();

            purchase.AddItem(purchaseItem);

            await _testsFixture.MockInDatabase(purchase);

            // Act
            HttpResponseMessage response = await _testsFixture.Client.DeleteAsync($"{_uriPart}/{purchase.Id}");

            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<PurchaseDto>>(response);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseHasItems));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverETiverFechado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Delete))]
        public async Task PurchasesController_Delete_DeveRetornarStatusCode400QuandoRemoverETiverFechado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateFaker().Generate();
            purchase.Close();

            await _testsFixture.MockInDatabase(purchase);

            // Act
            HttpResponseMessage response = await _testsFixture.Client.DeleteAsync($"{_uriPart}/{purchase.Id}");

            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<PurchaseDto>>(response);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseIsClosed));
        }

        [Fact(DisplayName = "DeveFecharCorretamenteQuandoNaoEstiverFechado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Close))]
        public async Task PurchasesController_Close_DeveFecharCorretamenteQuandoNaoEstiverFechado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateFaker().Generate();

            await _testsFixture.MockInDatabase(purchase);

            // Act
            HttpResponseMessage response =
                await _testsFixture.Client.PutAsync($"{_uriPart}/{purchase.Id}/close", null!);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoFecharENaoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Close))]
        public async Task PurchasesController_Close_DeveRetornarStatusCode400QuandoFecharENaoEstiverCadastrado()
        {
            // Arrange && Act
            HttpResponseMessage response =
                await _testsFixture.Client.PutAsync($"{_uriPart}/{Guid.NewGuid()}/close", null!);

            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<PurchaseDto>>(response);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoFecharEJaEstiverFechado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Close))]
        public async Task PurchasesController_Close_DeveRetornarStatusCode400QuandoFecharEJaEstiverFechado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateFaker().Generate();
            purchase.Close();

            await _testsFixture.MockInDatabase(purchase);

            // Act
            HttpResponseMessage response =
                await _testsFixture.Client.PutAsync($"{_uriPart}/{purchase.Id}/close", null!);

            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<PurchaseDto>>(response);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseIsClosed));
        }
    }
}
