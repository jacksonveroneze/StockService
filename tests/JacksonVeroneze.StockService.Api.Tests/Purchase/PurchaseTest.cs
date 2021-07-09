using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JacksonVeroneze.StockService.Api.Controllers.v1;
using JacksonVeroneze.StockService.Api.Tests.Configuration;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Application.Validations;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Common.Integration;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Entities;
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

            _testsFixture.DropDatabase().Wait();
            _testsFixture.CreateDatabase().Wait();
        }

        [Fact(DisplayName = "DeveFiltrarEPaginarOsDadosComSkipTakeCorretamente")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Filter))]
        public async Task PurchasesController_Filter_DeveFiltrarEPaginarOsDadosComSkipTakeCorretamente()
        {
            // Arrange
            int skip = 1;
            int take = 5;

            IList<Domain.Entities.Purchase> products = PurchaseFaker
                .Generate(30)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            await _testsFixture.MockInDatabase(products);

            // Act
            TestApiResponseOperationGet<Pageable<PurchaseDto>> result =
                await _testsFixture.SendGetRequest<Pageable<PurchaseDto>>(
                    $"{_uriPart}?skip={skip}&take={take}&description=a&state=1&dateInitial=2020-01-01&dateEnd=2022-01-01");

            // Assert
            IList<Domain.Entities.Purchase> productsFiltered =
                products.Where(x => x.Description.Contains("a")).Skip(skip).Take(take).ToList();

            int total = products.Count(x => x.Description.Contains("a"));

            result.Should().NotBeNull();
            result.Content.Total.Should().Be(total);
            result.Content.Pages.Should().Be((int)Math.Ceiling(total / (decimal)take));
            result.Content.CurrentPage.Should().Be(skip);
            result.Content.Data.Should().HaveCount(productsFiltered.Count);
        }

        [Fact(DisplayName = "DeveBuscarCorretamentePeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Find))]
        public async Task PurchasesController_Find_DeveBuscarCorretamentePeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            IList<Domain.Entities.Purchase> purchases = PurchaseFaker.Generate(10);

            Domain.Entities.Purchase purchase = purchases.First();

            await _testsFixture.MockInDatabase(purchases);

            // Act
            TestApiResponseOperationGet<PurchaseDto> result =
                await _testsFixture.SendGetRequest<PurchaseDto>($"{_uriPart}/{purchase.Id}");

            // Assert
            result.Should().NotBeNull();
            result.Content.Id.Should().Be(purchase.Id);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode404QuandoNaoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Find))]
        public async Task PurchasesController_Find_DeveRetornarStatusCode404QuandoNaoEstiverCadastrado()
        {
            // Arrange && Act
            TestApiResponseOperationGet<PurchaseDto> result =
                await _testsFixture.SendGetRequest<PurchaseDto>($"{_uriPart}/{Guid.NewGuid()}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Status.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact(DisplayName = "DeveSalvarCorretamenteQuandoEmEstadoValido")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Create))]
        public async Task PurchasesController_Create_DeveSalvarCorretamenteQuandoEmEstadoValido()
        {
            // Arrange
            AddOrUpdatePurchaseDto purchaseDto = AddOrUpdatePurchaseDtoFaker.GenerateValid();

            // Act
            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdatePurchaseDto, PurchaseDto>(
                    $"{_uriPart}/", purchaseDto);

            TestApiResponseOperationGet<PurchaseDto> resultGet =
                await _testsFixture.SendGetRequest<PurchaseDto>(result.HttpResponse.Headers.Location?.ToString());

            // Assert
            result.Should().NotBeNull();
            result.Data.Description.Should().Be(purchaseDto.Description);
            result.Errors.Should().BeEmpty();

            resultGet.Should().NotBeNull();
            resultGet.Content.Description.Should().Be(purchaseDto.Description);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarSalvarEmEstadoInvalido")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Create))]
        public async Task PurchasesController_Create_DeveRetornarErro400QuandoTentarSalvarEmEstadoInvalido()
        {
            // Arrange
            AddOrUpdatePurchaseDto purchaseDto = AddOrUpdatePurchaseDtoFaker.GenerateInvalid();

            // Act
            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdatePurchaseDto, PurchaseDto>(
                    $"{_uriPart}/", purchaseDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteQuandoEmEstadoValido")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Update))]
        public async Task PurchasesController_Update_DeveAtualizarCorretamenteQuandoEmEstadoValido()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.Generate();

            await _testsFixture.MockInDatabase(purchase);

            AddOrUpdatePurchaseDto purchaseDto = new() {Description = $"{purchase.Description}_atualizado"};

            // Act
            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdatePurchaseDto, PurchaseDto>(
                    $"{_uriPart}/{purchase.Id}", purchaseDto);

            // Assert
            result.Should().NotBeNull();
            result.Data.Description.Should().Be(purchaseDto.Description);
            result.Errors.Should().BeEmpty();

            result.Should().NotBeNull();
            result.Data.Description.Should().Be(purchaseDto.Description);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarEmEstadoInvalido")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Update))]
        public async Task PurchasesController_Update_DeveRetornarErro400QuandoTentarAtualizarEmEstadoInvalido()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.Generate();

            await _testsFixture.MockInDatabase(purchase);

            AddOrUpdatePurchaseDto purchaseDto = new() {Description = string.Empty};

            // Act
            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdatePurchaseDto, PurchaseDto>(
                    $"{_uriPart}/{purchase.Id}", purchaseDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarUmItemInexistente")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Update))]
        public async Task PurchasesController_Update_DeveRetornarErro400QuandoTentarAtualizarUmItemInexistente()
        {
            // Arrange
            AddOrUpdatePurchaseDto purchaseDto = AddOrUpdatePurchaseDtoFaker.GenerateValid();

            // Act
            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdatePurchaseDto, PurchaseDto>(
                    $"{_uriPart}/{Guid.NewGuid()}", purchaseDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseNotFoundById));
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteQuandoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Delete))]
        public async Task PurchasesController_Delete_DeveRemoverCorretamenteQuandoEstiverCadastrado()
        {
            // Arrange
            IList<Domain.Entities.Purchase> purchases = PurchaseFaker.Generate(10);

            Domain.Entities.Purchase purchase = purchases.First();

            await _testsFixture.MockInDatabase(purchases);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{purchase.Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode404QuandoRemoverENaoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Delete))]
        public async Task PurchasesController_Delete_DeveRetornarStatusCode404QuandoRemoverENaoEstiverCadastrado()
        {
            // Arrange && Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{Guid.NewGuid()}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverETiverDependentes")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Delete))]
        public async Task PurchasesController_Delete_DeveRetornarStatusCode400QuandoRemoverETiverDependentes()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(10);

            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{purchase.Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseHasItems));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverETiverFechado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Delete))]
        public async Task PurchasesController_Delete_DeveRetornarStatusCode400QuandoRemoverETiverFechado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.Generate();
            purchase.Close();

            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{purchase.Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseIsClosed));
        }

        [Fact(DisplayName = "DeveFecharCorretamenteQuandoNaoEstiverFechado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Close))]
        public async Task PurchasesController_Close_DeveFecharCorretamenteQuandoNaoEstiverFechado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendPutEmptyBodyRequest($"{_uriPart}/{purchase.Id}/close");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoFecharENaoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Close))]
        public async Task PurchasesController_Close_DeveRetornarStatusCode400QuandoFecharENaoEstiverCadastrado()
        {
            // Arrange && Act
            TestApiResponseBase result =
                await _testsFixture.SendPutEmptyBodyRequest($"{_uriPart}/{Guid.NewGuid()}/close");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoFecharEJaEstiverFechado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.Close))]
        public async Task PurchasesController_Close_DeveRetornarStatusCode400QuandoFecharEJaEstiverFechado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.Generate();
            purchase.Close();

            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendPutEmptyBodyRequest($"{_uriPart}/{purchase.Id}/close");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseIsClosed));
        }

        [Fact(DisplayName = "DeveBuscarCorretamenteOsItensPeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.FindItems))]
        public async Task
            PurchasesController_FindItems_DeveBuscarCorretamenteOsItensPeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            int total = 10;

            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(total);

            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseOperationGet<IList<PurchaseItemDto>> result =
                await _testsFixture.SendGetRequest<IList<PurchaseItemDto>>(
                    $"{_uriPart}/{purchase.Id}/items");

            // Assert
            result.Should().NotBeNull();
            result.Content.Should().HaveCount(total);
        }

        [Fact(DisplayName = "DeveRetornarErro404QuandoBuscarOsItensCujoPaiNaoExiste")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.FindItems))]
        public async Task PurchasesController_FindItems_DeveRetornarErro404QuandoBuscarOsItensCujoPaiNaoExiste()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseOperationGet<IList<PurchaseItemDto>> result =
                await _testsFixture.SendGetRequest<IList<PurchaseItemDto>>(
                    $"{_uriPart}/{Guid.NewGuid()}/items");

            result.Status.Should().Be(StatusCodes.Status404NotFound);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact(DisplayName = "DeveBuscarCorretamenteOItemPeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.FindItem))]
        public async Task PurchasesController_FindItem_DeveBuscarCorretamenteOItemPeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseOperationGet<PurchaseItemDto> result =
                await _testsFixture.SendGetRequest<PurchaseItemDto>(
                    $"{_uriPart}/{purchase.Id}/items/{purchase.Items.First().Id}");

            // Assert
            result.Should().NotBeNull();
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoBuscarUmItemCujoPaiNaoExiste")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.FindItem))]
        public async Task PurchasesController_FindItem_DeveRetornarErro404QuandoBuscarUmItemCujoPaiNaoExiste()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseOperationGet<PurchaseItemDto> result =
                await _testsFixture.SendGetRequest<PurchaseItemDto>(
                    $"{_uriPart}/{Guid.NewGuid()}/items/{purchase.Items.First().Id}");

            // Assert
            result.Status.Should().Be(StatusCodes.Status404NotFound);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact(DisplayName = "DeveSalvarCorretamenteOItemQuandoEmEstadoValido")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.CreateItem))]
        public async Task PurchasesController_CreateItem_DeveSalvarCorretamenteOItemQuandoEmEstadoValido()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.Generate();
            Domain.Entities.Product product = ProductFaker.Generate();

            await _testsFixture.MockInDatabase(purchase);
            await _testsFixture.MockInDatabase(product);

            AddOrUpdatePurchaseItemDto purchaseItemDto =
                AddOrUpdatePurchaseItemDtoFaker.GenerateValid(product.Id);

            // Act
            TestApiResponseOperations<PurchaseItemDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdatePurchaseItemDto, PurchaseItemDto>(
                    $"{_uriPart}/{purchase.Id}/items", purchaseItemDto);

            TestApiResponseOperationGet<PurchaseItemDto> resultGet =
                await _testsFixture.SendGetRequest<PurchaseItemDto>(
                    result.HttpResponse.Headers.Location?.ToString());

            // Assert
            result.Should().NotBeNull();
            result.Data.Amount.Should().Be(purchaseItemDto.Amount);
            result.Data.Value.Should().Be(purchaseItemDto.Value);
            result.Errors.Should().BeEmpty();

            resultGet.Should().NotBeNull();
            resultGet.Content.Amount.Should().Be(purchaseItemDto.Amount);
            resultGet.Content.Value.Should().BeApproximately(purchaseItemDto.Value, (decimal)0.01);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarCriarItemEmEstadoInvalido")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.CreateItem))]
        public async Task PurchasesController_CreateItem_DeveRetornarErro400QuandoTentarCriarItemEmEstadoInvalido()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.Generate();

            await _testsFixture.MockInDatabase(purchase);

            AddOrUpdatePurchaseItemDto purchaseDto = new() {Amount = 0, Value = 0, ProductId = Guid.NewGuid()};

            // Act
            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdatePurchaseItemDto, PurchaseDto>(
                    $"{_uriPart}/{purchase.Id}/items", purchaseDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoCadastrarOItemEPaiNaoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.CreateItem))]
        public async Task
            PurchasesController_CreateItem_DeveRetornarStatusCode400QuandoCadastrarOItemEPaiNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.Generate();

            await _testsFixture.MockInDatabase(purchase);

            AddOrUpdatePurchaseItemDto purchaseDto =
                AddOrUpdatePurchaseItemDtoFaker.GenerateValid(Guid.NewGuid());

            // Act
            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdatePurchaseItemDto, PurchaseDto>(
                    $"{_uriPart}/{Guid.NewGuid()}/items", purchaseDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoCriarEOPaiEstiverFechado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.CreateItem))]
        public async Task PurchasesController_CreateItem_DeveRetornarStatusCode400QuandoCriarEOPaiEstiverFechado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.Generate();
            purchase.Close();

            await _testsFixture.MockInDatabase(purchase);

            AddOrUpdatePurchaseItemDto purchaseItemDto =
                AddOrUpdatePurchaseItemDtoFaker.GenerateValid(Guid.NewGuid());

            // Act
            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdatePurchaseItemDto, PurchaseDto>(
                    $"{_uriPart}/{purchase.Id}/items", purchaseItemDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseIsClosed));
        }


        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoCriarItemComProdutoExistenteEmOutroItemNoPai")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.CreateItem))]
        public async Task
            PurchasesController_CreateItem_DeveRetornarStatusCode400QuandoCriarItemComProdutoExistenteEmOutroItemNoPai()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(purchase);

            AddOrUpdatePurchaseItemDto purchaseItemDto = new()
            {
                ProductId = purchase.Items.First().Product.Id, Amount = 10, Value = 20
            };

            // Act
            TestApiResponseOperations<PurchaseDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdatePurchaseItemDto, PurchaseDto>(
                    $"{_uriPart}/{purchase.Id}/items", purchaseItemDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteOItemQuandoEmEstadoValido")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.UpdateItem))]
        public async Task PurchasesController_UpdateItem_DeveAtualizarCorretamenteOItemQuandoEmEstadoValido()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(purchase);

            PurchaseItem purchaseItem = purchase.Items.First();

            AddOrUpdatePurchaseItemDto purchaseItemDto =
                new() {Amount = 10, Value = 20, ProductId = purchaseItem.Product.Id};

            // Act
            TestApiResponseOperations<PurchaseItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdatePurchaseItemDto, PurchaseItemDto>(
                    $"{_uriPart}/{purchase.Id}/items/{purchaseItem.Id}", purchaseItemDto);

            TestApiResponseOperationGet<PurchaseItemDto> resultGet =
                await _testsFixture.SendGetRequest<PurchaseItemDto>(
                    $"{_uriPart}/{purchase.Id}/items/{purchaseItem.Id}");

            // Assert
            result.Should().NotBeNull();
            result.Data.Amount.Should().Be(purchaseItemDto.Amount);
            result.Data.Value.Should().Be(purchaseItemDto.Value);
            result.Errors.Should().BeEmpty();

            resultGet.Should().NotBeNull();
            resultGet.Content.Amount.Should().Be(purchaseItemDto.Amount);
            resultGet.Content.Value.Should().Be(purchaseItemDto.Value);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarItemEmEstadoInvalido")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.UpdateItem))]
        public async Task PurchasesController_UpdateItem_DeveRetornarErro400QuandoTentarAtualizarItemEmEstadoInvalido()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(purchase);

            PurchaseItem purchaseItem = purchase.Items.First();

            AddOrUpdatePurchaseItemDto purchaseItemDto =
                new() {Amount = 0, Value = 0, ProductId = Guid.NewGuid()};

            // Act
            TestApiResponseOperations<PurchaseItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdatePurchaseItemDto, PurchaseItemDto>(
                    $"{_uriPart}/{purchase.Id}/items/{purchaseItem.Id}", purchaseItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.ProductNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoAtualizarOItemEPaiNaoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.UpdateItem))]
        public async Task
            PurchasesController_UpdateItem_DeveRetornarStatusCode400QuandoAtualizarOItemEPaiNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(purchase);

            PurchaseItem purchaseItem = purchase.Items.First();

            AddOrUpdatePurchaseItemDto purchaseItemDto = new();

            // Act
            TestApiResponseOperations<PurchaseItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdatePurchaseItemDto, PurchaseItemDto>(
                    $"{_uriPart}/{Guid.NewGuid()}/items/{purchaseItem.Id}", purchaseItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoAtualizarEOPaiEstiverFechado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.UpdateItem))]
        public async Task PurchasesController_UpdateItem_DeveRetornarStatusCode400QuandoAtualizarEOPaiEstiverFechado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(2);
            purchase.Close();

            await _testsFixture.MockInDatabase(purchase);

            PurchaseItem purchaseItem = purchase.Items.First();

            AddOrUpdatePurchaseItemDto purchaseItemDto = new();

            // Act
            TestApiResponseOperations<PurchaseItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdatePurchaseItemDto, PurchaseItemDto>(
                    $"{_uriPart}/{purchase.Id}/items/{purchaseItem.Id}", purchaseItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseIsClosed));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoAtualizarOItemEOMesmoNaoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.UpdateItem))]
        public async Task
            PurchasesController_UpdateItem_DeveRetornarStatusCode400QuandoAtualizarOItemEOMesmoNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(purchase);

            AddOrUpdatePurchaseItemDto purchaseItemDto = new();

            // Act
            TestApiResponseOperations<PurchaseItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdatePurchaseItemDto, PurchaseItemDto>(
                    $"{_uriPart}/{purchase.Id}/items/{Guid.NewGuid()}", purchaseItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should()
                .Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseItemNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoAtualizarItemComProdutoExistenteEmOutroItemNoPai")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.UpdateItem))]
        public async Task
            PurchasesController_UpdateItem_DeveRetornarStatusCode400QuandoAtualizarItemComProdutoExistenteEmOutroItemNoPai()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(purchase);

            PurchaseItem purchaseItem = purchase.Items.First();

            AddOrUpdatePurchaseItemDto purchaseItemDto =
                new() {Amount = 10, Value = 20, ProductId = purchase.Items.Last().Product.Id};

            // Act
            TestApiResponseOperations<PurchaseItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdatePurchaseItemDto, PurchaseItemDto>(
                    $"{_uriPart}/{purchase.Id}/items/{purchaseItem.Id}", purchaseItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteOItenmPeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.DeleteItem))]
        public async Task
            PurchasesController_DeleteItem_DeveRemoverCorretamenteOItemPeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(10);

            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseBase result = await _testsFixture.SendDeleteRequest(
                $"{_uriPart}/{purchase.Id}/items/{purchase.Items.First().Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverOItemEPaiNaoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.DeleteItem))]
        public async Task
            PurchasesController_DeleteItem_DeveRetornarStatusCode400QuandoRemoverOItemEPaiNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(10);

            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseBase result = await _testsFixture.SendDeleteRequest(
                $"{_uriPart}/{Guid.NewGuid()}/items/{purchase.Items.First().Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverOItemEOMesmoNaoEstiverCadastrado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.DeleteItem))]
        public async Task
            PurchasesController_DeleteItem_DeveRetornarStatusCode400QuandoRemoverOItemEOMesmoNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(10);

            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseBase result = await _testsFixture.SendDeleteRequest(
                $"{_uriPart}/{purchase.Id}/items/{Guid.NewGuid()}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should()
                .Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseItemNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverEOPaiEstiverFechado")]
        [Trait(nameof(PurchasesController), nameof(PurchasesController.DeleteItem))]
        public async Task
            PurchasesController_DeleteItem_DeveRetornarStatusCode400QuandoRemoverEOPaiEstiverFechado()
        {
            // Arrange
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItems(10);
            purchase.Close();

            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseBase result = await _testsFixture.SendDeleteRequest(
                $"{_uriPart}/{purchase.Id}/items/{purchase.Items.First().Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.PurchaseIsClosed));
        }
    }
}
