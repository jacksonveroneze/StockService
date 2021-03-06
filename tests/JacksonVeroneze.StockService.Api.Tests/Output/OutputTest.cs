using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JacksonVeroneze.StockService.Api.Controllers.v1;
using JacksonVeroneze.StockService.Api.Tests.Configuration;
using JacksonVeroneze.StockService.Application.DTO.Output;
using JacksonVeroneze.StockService.Application.DTO.OutputItem;
using JacksonVeroneze.StockService.Application.Validations;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Common.Integration;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Adjustment;
using JacksonVeroneze.StockService.Domain.Events.Output;
using JacksonVeroneze.StockService.Domain.Events.Purchase;
using JacksonVeroneze.StockService.Domain.Models;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace JacksonVeroneze.StockService.Api.Tests.Output
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class OutputTest
    {
        private const string _uriPart = "/api/v1/outputs";

        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public OutputTest(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;

            _testsFixture.DropDatabase().Wait();
            _testsFixture.CreateDatabase().Wait();
        }

        [Fact(DisplayName = "DeveFiltrarEPaginarOsDadosComSkipTakeCorretamente")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Filter))]
        public async Task OutputsController_Filter_DeveFiltrarEPaginarOsDadosComSkipTakeCorretamente()
        {
            // Arrange
            int skip = 1;
            int take = 5;

            IList<Domain.Entities.Output> products = OutputFaker
                .Generate(30)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            await _testsFixture.MockInDatabase(products);

            // Act
            TestApiResponseOperationGet<Pageable<OutputDto>> result =
                await _testsFixture.SendGetRequest<Pageable<OutputDto>>(
                    $"{_uriPart}?skip={skip}&take={take}&description=a&state=1&dateInitial=2020-01-01&dateEnd=2022-01-01");

            // Assert
            IList<Domain.Entities.Output> productsFiltered =
                products.Where(x => x.Description.Contains("a")).Skip(skip).Take(take).ToList();

            int total = products.Count(x => x.Description.Contains("a"));

            result.Should().NotBeNull();
            result.Content.Total.Should().Be(total);
            result.Content.Pages.Should().Be((int)Math.Ceiling(total / (decimal)take));
            result.Content.CurrentPage.Should().Be(skip);
            result.Content.Data.Should().HaveCount(productsFiltered.Count);
        }

        [Fact(DisplayName = "DeveBuscarCorretamentePeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Find))]
        public async Task OutputsController_Find_DeveBuscarCorretamentePeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            IList<Domain.Entities.Output> outputs = OutputFaker.Generate(10);

            Domain.Entities.Output output = outputs.First();

            await _testsFixture.MockInDatabase(outputs);

            // Act
            TestApiResponseOperationGet<OutputDto> result =
                await _testsFixture.SendGetRequest<OutputDto>($"{_uriPart}/{output.Id}");

            // Assert
            result.Should().NotBeNull();
            result.Content.Id.Should().Be(output.Id);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode404QuandoNaoEstiverCadastrado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Find))]
        public async Task OutputsController_Find_DeveRetornarStatusCode404QuandoNaoEstiverCadastrado()
        {
            // Arrange && Act
            TestApiResponseOperationGet<OutputDto> result =
                await _testsFixture.SendGetRequest<OutputDto>($"{_uriPart}/{Guid.NewGuid()}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Status.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact(DisplayName = "DeveSalvarCorretamenteQuandoEmEstadoValido")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Create))]
        public async Task OutputsController_Create_DeveSalvarCorretamenteQuandoEmEstadoValido()
        {
            // Arrange
            AddOrUpdateOutputDto outputDto = AddOrUpdateOutputDtoFaker.GenerateValid();

            // Act
            TestApiResponseOperations<OutputDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateOutputDto, OutputDto>(
                    $"{_uriPart}/", outputDto);

            TestApiResponseOperationGet<OutputDto> resultGet =
                await _testsFixture.SendGetRequest<OutputDto>(result.HttpResponse.Headers.Location?.ToString());

            // Assert
            result.Should().NotBeNull();
            result.Data.Description.Should().Be(outputDto.Description);
            result.Errors.Should().BeEmpty();

            resultGet.Should().NotBeNull();
            resultGet.Content.Description.Should().Be(outputDto.Description);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarSalvarEmEstadoInvalido")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Create))]
        public async Task OutputsController_Create_DeveRetornarErro400QuandoTentarSalvarEmEstadoInvalido()
        {
            // Arrange
            AddOrUpdateOutputDto outputDto = AddOrUpdateOutputDtoFaker.GenerateInvalid();

            // Act
            TestApiResponseOperations<OutputDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateOutputDto, OutputDto>(
                    $"{_uriPart}/", outputDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteQuandoEmEstadoValido")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Update))]
        public async Task OutputsController_Update_DeveAtualizarCorretamenteQuandoEmEstadoValido()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.Generate();

            await _testsFixture.MockInDatabase(output);

            AddOrUpdateOutputDto outputDto = new() {Description = $"{output.Description}_atualizado"};

            // Act
            TestApiResponseOperations<OutputDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateOutputDto, OutputDto>(
                    $"{_uriPart}/{output.Id}", outputDto);

            // Assert
            result.Should().NotBeNull();
            result.Data.Description.Should().Be(outputDto.Description);
            result.Errors.Should().BeEmpty();

            result.Should().NotBeNull();
            result.Data.Description.Should().Be(outputDto.Description);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarEmEstadoInvalido")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Update))]
        public async Task OutputsController_Update_DeveRetornarErro400QuandoTentarAtualizarEmEstadoInvalido()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.Generate();

            await _testsFixture.MockInDatabase(output);

            AddOrUpdateOutputDto outputDto = new() {Description = string.Empty};

            // Act
            TestApiResponseOperations<OutputDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateOutputDto, OutputDto>(
                    $"{_uriPart}/{output.Id}", outputDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarUmItemInexistente")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Update))]
        public async Task OutputsController_Update_DeveRetornarErro400QuandoTentarAtualizarUmItemInexistente()
        {
            // Arrange
            AddOrUpdateOutputDto outputDto = AddOrUpdateOutputDtoFaker.GenerateValid();

            // Act
            TestApiResponseOperations<OutputDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateOutputDto, OutputDto>(
                    $"{_uriPart}/{Guid.NewGuid()}", outputDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputNotFoundById));
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteQuandoEstiverCadastrado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Delete))]
        public async Task OutputsController_Delete_DeveRemoverCorretamenteQuandoEstiverCadastrado()
        {
            // Arrange
            IList<Domain.Entities.Output> outputs = OutputFaker.Generate(10);

            Domain.Entities.Output output = outputs.First();

            await _testsFixture.MockInDatabase(outputs);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{output.Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode404QuandoRemoverENaoEstiverCadastrado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Delete))]
        public async Task OutputsController_Delete_DeveRetornarStatusCode404QuandoRemoverENaoEstiverCadastrado()
        {
            // Arrange && Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{Guid.NewGuid()}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverETiverDependentes")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Delete))]
        public async Task OutputsController_Delete_DeveRetornarStatusCode400QuandoRemoverETiverDependentes()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(10);

            await _testsFixture.MockInDatabase(output);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{output.Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputHasItems));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverETiverFechado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Delete))]
        public async Task OutputsController_Delete_DeveRetornarStatusCode400QuandoRemoverETiverFechado()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.Generate();
            output.Close();

            await _testsFixture.MockInDatabase(output);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{output.Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputIsClosed));
        }

        [Fact(DisplayName = "DeveFecharCorretamenteQuandoNaoEstiverFechado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Close))]
        public async Task OutputsController_Close_DeveFecharCorretamenteQuandoNaoEstiverFechado()
        {
            // Arrange
            Domain.Entities.Product product = ProductFaker.Generate();

            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItem(product);

            purchase.Close();
            purchase.AddEvent(new PurchaseClosedEvent(purchase.Id));

            Domain.Entities.Output output = OutputFaker.GenerateWithItem(product);

            await _testsFixture.MockInDatabase(product);
            await _testsFixture.MockInDatabase(purchase);
            await _testsFixture.MockInDatabase(output);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendPutEmptyBodyRequest($"{_uriPart}/{output.Id}/close");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoFecharENaoEstiverCadastrado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Close))]
        public async Task OutputsController_Close_DeveRetornarStatusCode400QuandoFecharENaoEstiverCadastrado()
        {
            // Arrange && Act
            TestApiResponseBase result =
                await _testsFixture.SendPutEmptyBodyRequest($"{_uriPart}/{Guid.NewGuid()}/close");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoFecharEJaEstiverFechado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.Close))]
        public async Task OutputsController_Close_DeveRetornarStatusCode400QuandoFecharEJaEstiverFechado()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.Generate();
            output.Close();

            await _testsFixture.MockInDatabase(output);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendPutEmptyBodyRequest($"{_uriPart}/{output.Id}/close");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputIsClosed));
        }

        [Fact(DisplayName = "DeveBuscarCorretamenteOsItensPeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.FindItems))]
        public async Task
            OutputsController_FindItems_DeveBuscarCorretamenteOsItensPeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            int total = 10;

            Domain.Entities.Output output = OutputFaker.GenerateWithItems(total);

            await _testsFixture.MockInDatabase(output);

            // Act
            TestApiResponseOperationGet<IList<OutputItemDto>> result =
                await _testsFixture.SendGetRequest<IList<OutputItemDto>>(
                    $"{_uriPart}/{output.Id}/items");

            // Assert
            result.Should().NotBeNull();
            result.Content.Should().HaveCount(total);
        }

        [Fact(DisplayName = "DeveRetornarErro404QuandoBuscarOsItensCujoPaiNaoExiste")]
        [Trait(nameof(OutputsController), nameof(OutputsController.FindItems))]
        public async Task OutputsController_FindItems_DeveRetornarErro404QuandoBuscarOsItensCujoPaiNaoExiste()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(output);

            // Act
            TestApiResponseOperationGet<IList<OutputItemDto>> result =
                await _testsFixture.SendGetRequest<IList<OutputItemDto>>(
                    $"{_uriPart}/{Guid.NewGuid()}/items");

            result.Status.Should().Be(StatusCodes.Status404NotFound);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact(DisplayName = "DeveBuscarCorretamenteOItemPeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.FindItem))]
        public async Task OutputsController_FindItem_DeveBuscarCorretamenteOItemPeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(output);

            // Act
            TestApiResponseOperationGet<OutputItemDto> result =
                await _testsFixture.SendGetRequest<OutputItemDto>(
                    $"{_uriPart}/{output.Id}/items/{output.Items.First().Id}");

            // Assert
            result.Should().NotBeNull();
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoBuscarUmItemCujoPaiNaoExiste")]
        [Trait(nameof(OutputsController), nameof(AdjustmentsController.FindItem))]
        public async Task OutputsController_FindItem_DeveRetornarErro404QuandoBuscarUmItemCujoPaiNaoExiste()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(output);

            // Act
            TestApiResponseOperationGet<OutputItemDto> result =
                await _testsFixture.SendGetRequest<OutputItemDto>(
                    $"{_uriPart}/{Guid.NewGuid()}/items/{output.Items.First().Id}");

            // Assert
            result.Status.Should().Be(StatusCodes.Status404NotFound);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact(DisplayName = "DeveSalvarCorretamenteOItemQuandoEmEstadoValido")]
        [Trait(nameof(OutputsController), nameof(OutputsController.CreateItem))]
        public async Task OutputsController_CreateItem_DeveSalvarCorretamenteOItemQuandoEmEstadoValido()
        {
            // Arrange
            Domain.Entities.Product product = ProductFaker.Generate();
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItem(product);
            purchase.Close();
            purchase.AddEvent(new PurchaseClosedEvent(purchase.Id));

            Domain.Entities.Output output = OutputFaker.Generate();

            await _testsFixture.MockInDatabase(product);
            await _testsFixture.MockInDatabase(purchase);
            await _testsFixture.MockInDatabase(output);

            AddOrUpdateOutputItemDto outputItemDto =
                AddOrUpdateOutputItemDtoFaker.GenerateValid(product.Id);

            // Act
            TestApiResponseOperations<OutputItemDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateOutputItemDto, OutputItemDto>(
                    $"{_uriPart}/{output.Id}/items", outputItemDto);

            TestApiResponseOperationGet<OutputItemDto> resultGet =
                await _testsFixture.SendGetRequest<OutputItemDto>(
                    result.HttpResponse.Headers.Location?.ToString());

            // Assert
            result.Should().NotBeNull();
            result.Data.Amount.Should().Be(outputItemDto.Amount);
            result.Errors.Should().BeEmpty();

            resultGet.Should().NotBeNull();
            resultGet.Content.Amount.Should().Be(outputItemDto.Amount);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarCriarItemEmEstadoInvalido")]
        [Trait(nameof(OutputsController), nameof(OutputsController.CreateItem))]
        public async Task OutputsController_CreateItem_DeveRetornarErro400QuandoTentarCriarItemEmEstadoInvalido()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.Generate();

            await _testsFixture.MockInDatabase(output);

            AddOrUpdateOutputItemDto outputDto = new() {Amount = 0, ProductId = Guid.NewGuid()};

            // Act
            TestApiResponseOperations<OutputDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateOutputItemDto, OutputDto>(
                    $"{_uriPart}/{output.Id}/items", outputDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoCadastrarOItemEPaiNaoEstiverCadastrado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.CreateItem))]
        public async Task
            OutputsController_CreateItem_DeveRetornarStatusCode400QuandoCadastrarOItemEPaiNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.Generate();

            await _testsFixture.MockInDatabase(output);

            AddOrUpdateOutputItemDto outputDto =
                AddOrUpdateOutputItemDtoFaker.GenerateValid(Guid.NewGuid());

            // Act
            TestApiResponseOperations<OutputDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateOutputItemDto, OutputDto>(
                    $"{_uriPart}/{Guid.NewGuid()}/items", outputDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoCriarEOPaiEstiverFechado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.CreateItem))]
        public async Task OutputsController_CreateItem_DeveRetornarStatusCode400QuandoCriarEOPaiEstiverFechado()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.Generate();
            output.Close();

            await _testsFixture.MockInDatabase(output);

            AddOrUpdateOutputItemDto outputItemDto =
                AddOrUpdateOutputItemDtoFaker.GenerateValid(Guid.NewGuid());

            // Act
            TestApiResponseOperations<OutputDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateOutputItemDto, OutputDto>(
                    $"{_uriPart}/{output.Id}/items", outputItemDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputIsClosed));
        }


        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoCriarItemComProdutoExistenteEmOutroItemNoPai")]
        [Trait(nameof(OutputsController), nameof(OutputsController.CreateItem))]
        public async Task
            OutputsController_CreateItem_DeveRetornarStatusCode400QuandoCriarItemComProdutoExistenteEmOutroItemNoPai()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(output);

            AddOrUpdateOutputItemDto outputItemDto =
                new() {ProductId = output.Items.First().Product.Id, Amount = 10};

            // Act
            TestApiResponseOperations<OutputDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateOutputItemDto, OutputDto>(
                    $"{_uriPart}/{output.Id}/items", outputItemDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteOItemQuandoEmEstadoValido")]
        [Trait(nameof(OutputsController), nameof(OutputsController.UpdateItem))]
        public async Task OutputsController_UpdateItem_DeveAtualizarCorretamenteOItemQuandoEmEstadoValido()
        {
            // Arrange
            Domain.Entities.Product product = ProductFaker.Generate();
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItem(product);
            purchase.Close();
            purchase.AddEvent(new PurchaseClosedEvent(purchase.Id));

            Domain.Entities.Output output = OutputFaker.GenerateWithItem(product);

            await _testsFixture.MockInDatabase(product);
            await _testsFixture.MockInDatabase(purchase);
            await _testsFixture.MockInDatabase(output);

            OutputItem outputItem = output.Items.First();

            AddOrUpdateOutputItemDto outputItemDto =
                new() {Amount = 10, ProductId = outputItem.Product.Id};

            // Act
            TestApiResponseOperations<OutputItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateOutputItemDto, OutputItemDto>(
                    $"{_uriPart}/{output.Id}/items/{outputItem.Id}", outputItemDto);

            TestApiResponseOperationGet<OutputItemDto> resultGet =
                await _testsFixture.SendGetRequest<OutputItemDto>(
                    $"{_uriPart}/{output.Id}/items/{outputItem.Id}");

            // Assert
            result.Should().NotBeNull();
            result.Data.Amount.Should().Be(outputItemDto.Amount);
            result.Errors.Should().BeEmpty();

            resultGet.Should().NotBeNull();
            resultGet.Content.Amount.Should().Be(outputItemDto.Amount);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarItemEmEstadoInvalido")]
        [Trait(nameof(OutputsController), nameof(OutputsController.UpdateItem))]
        public async Task OutputsController_UpdateItem_DeveRetornarErro400QuandoTentarAtualizarItemEmEstadoInvalido()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(output);

            OutputItem outputItem = output.Items.First();

            AddOrUpdateOutputItemDto outputItemDto =
                new() {Amount = 0, ProductId = Guid.NewGuid()};

            // Act
            TestApiResponseOperations<OutputItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateOutputItemDto, OutputItemDto>(
                    $"{_uriPart}/{output.Id}/items/{outputItem.Id}", outputItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.ProductNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoAtualizarOItemEPaiNaoEstiverCadastrado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.UpdateItem))]
        public async Task
            OutputsController_UpdateItem_DeveRetornarStatusCode400QuandoAtualizarOItemEPaiNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(output);

            OutputItem outputItem = output.Items.First();

            AddOrUpdateOutputItemDto outputItemDto = new();

            // Act
            TestApiResponseOperations<OutputItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateOutputItemDto, OutputItemDto>(
                    $"{_uriPart}/{Guid.NewGuid()}/items/{outputItem.Id}", outputItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoAtualizarEOPaiEstiverFechado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.UpdateItem))]
        public async Task OutputsController_UpdateItem_DeveRetornarStatusCode400QuandoAtualizarEOPaiEstiverFechado()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(2);
            output.Close();

            await _testsFixture.MockInDatabase(output);

            OutputItem outputItem = output.Items.First();

            AddOrUpdateOutputItemDto outputItemDto = new();

            // Act
            TestApiResponseOperations<OutputItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateOutputItemDto, OutputItemDto>(
                    $"{_uriPart}/{output.Id}/items/{outputItem.Id}", outputItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputIsClosed));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoAtualizarOItemEOMesmoNaoEstiverCadastrado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.UpdateItem))]
        public async Task
            OutputsController_UpdateItem_DeveRetornarStatusCode400QuandoAtualizarOItemEOMesmoNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(output);

            AddOrUpdateOutputItemDto outputItemDto = new();

            // Act
            TestApiResponseOperations<OutputItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateOutputItemDto, OutputItemDto>(
                    $"{_uriPart}/{output.Id}/items/{Guid.NewGuid()}", outputItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should()
                .Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputItemNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoAtualizarItemComProdutoExistenteEmOutroItemNoPai")]
        [Trait(nameof(OutputsController), nameof(OutputsController.UpdateItem))]
        public async Task
            OutputsController_UpdateItem_DeveRetornarStatusCode400QuandoAtualizarItemComProdutoExistenteEmOutroItemNoPai()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(output);

            OutputItem outputItem = output.Items.First();

            AddOrUpdateOutputItemDto outputItemDto =
                new() {Amount = 10, ProductId = output.Items.Last().Product.Id};

            // Act
            TestApiResponseOperations<OutputItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateOutputItemDto, OutputItemDto>(
                    $"{_uriPart}/{output.Id}/items/{outputItem.Id}", outputItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteOItenmPeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.DeleteItem))]
        public async Task
            OutputsController_DeleteItem_DeveRemoverCorretamenteOItemPeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(10);

            await _testsFixture.MockInDatabase(output);

            // Act
            TestApiResponseBase result = await _testsFixture.SendDeleteRequest(
                $"{_uriPart}/{output.Id}/items/{output.Items.First().Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverOItemEPaiNaoEstiverCadastrado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.DeleteItem))]
        public async Task
            OutputsController_DeleteItem_DeveRetornarStatusCode400QuandoRemoverOItemEPaiNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(10);

            await _testsFixture.MockInDatabase(output);

            // Act
            TestApiResponseBase result = await _testsFixture.SendDeleteRequest(
                $"{_uriPart}/{Guid.NewGuid()}/items/{output.Items.First().Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverOItemEOMesmoNaoEstiverCadastrado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.DeleteItem))]
        public async Task
            OutputsController_DeleteItem_DeveRetornarStatusCode400QuandoRemoverOItemEOMesmoNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(10);

            await _testsFixture.MockInDatabase(output);

            // Act
            TestApiResponseBase result = await _testsFixture.SendDeleteRequest(
                $"{_uriPart}/{output.Id}/items/{Guid.NewGuid()}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should()
                .Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputItemNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverEOPaiEstiverFechado")]
        [Trait(nameof(OutputsController), nameof(OutputsController.DeleteItem))]
        public async Task
            OutputsController_DeleteItem_DeveRetornarStatusCode400QuandoRemoverEOPaiEstiverFechado()
        {
            // Arrange
            Domain.Entities.Output output = OutputFaker.GenerateWithItems(10);
            output.Close();

            await _testsFixture.MockInDatabase(output);

            // Act
            TestApiResponseBase result = await _testsFixture.SendDeleteRequest(
                $"{_uriPart}/{output.Id}/items/{output.Items.First().Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.OutputIsClosed));
        }

        [Fact(DisplayName = "DeveDesfazerCorretamenteOItemQuandoEmEstadoValido", Skip = "Refatorar")]
        [Trait(nameof(OutputsController), nameof(OutputsController.UndoItem))]
        public async Task OutputsController_UndoItem_DeveDesfazerCorretamenteOItemQuandoEmEstadoValido()
        {
            // Arrange
            IList<Domain.Entities.Product> products = ProductFaker.Generate(5);

            await _testsFixture.MockInDatabase(products);

            Domain.Entities.Adjustment adjustment = AdjustmentFaker.Generate();
            IList<Domain.Entities.Output> outputs = OutputFaker.Generate(3);

            await MockUndoItemAjustmentTestAsync(products, adjustment);
            await MockUndoItemOutputTestAsync(products, outputs);

            Domain.Entities.Output outputUndo = outputs.First();
            OutputItem outputItemUndo = outputs.First().Items.Last();
            Domain.Entities.Product productTest = outputItemUndo.Product;

            // Act
            TestApiResponseOperationGet<MovementModel> resultMovement =
                await _testsFixture.SendGetRequest<MovementModel>(
                    $"/api/v1/movements/find-by-product/{productTest.Id}");

            TestApiResponseBase resultUndo = await _testsFixture.SendPutEmptyBodyRequest(
                $"{_uriPart}/{outputUndo.Id}/items/{outputItemUndo.Id}/undo");

            TestApiResponseOperationGet<MovementModel> resultMovement1 =
                await _testsFixture.SendGetRequest<MovementModel>(
                    $"/api/v1/movements/find-by-product/{productTest.Id}");

            // Assert
            int totalAmmountAdjustment = adjustment.Items.First(x => x.Product == productTest).Amount;
            int totalAmmountOutput = outputs
                .SelectMany(x => x.Items.Where(x => x.Product == productTest)
                    .Select(x => x.Amount)).Sum(x => x);

            int totalAmmountInStockBeforeUndo = totalAmmountAdjustment - totalAmmountOutput;
            int totalAmmountInStockAfterUndo = totalAmmountAdjustment - totalAmmountOutput + 1;

            resultMovement.HttpResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            resultMovement1.HttpResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            resultUndo.HttpResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            resultMovement.Content.Ammount.Should().Be(totalAmmountInStockBeforeUndo);
            resultMovement1.Content.Ammount.Should().Be(totalAmmountInStockAfterUndo);
        }

        [Fact(DisplayName = "DeveDesfazerCorretamenteOItemQuandoEmEstadoValidoAposAjustment", Skip = "Refatorar")]
        [Trait(nameof(OutputsController), nameof(OutputsController.UndoItem))]
        public async Task OutputsController_UndoItem_DeveDesfazerCorretamenteOItemQuandoEmEstadoValidoAposAjustment()
        {
            // Arrange
            IList<Domain.Entities.Product> products = ProductFaker.Generate(5);

            await _testsFixture.MockInDatabase(products);

            Domain.Entities.Adjustment adjustment = AdjustmentFaker.Generate();
            IList<Domain.Entities.Output> outputs = OutputFaker.Generate(3);
            Domain.Entities.Adjustment adjustment1 = AdjustmentFaker.Generate();

            await MockUndoItemAjustmentTestAsync(products, adjustment);
            await MockUndoItemOutputTestAsync(products, outputs);
            await MockUndoItemAjustmentTestAsync(products, adjustment1);

            Domain.Entities.Output outputUndo = outputs.First();
            OutputItem outputItemUndo = outputs.First().Items.Last();
            Domain.Entities.Product productTest = outputItemUndo.Product;

            // Act
            TestApiResponseBase resultUndo = await _testsFixture.SendPutEmptyBodyRequest(
                $"{_uriPart}/{outputUndo.Id}/items/{outputItemUndo.Id}/undo");

            TestApiResponseOperationGet<MovementModel> resultMovement1 =
                await _testsFixture.SendGetRequest<MovementModel>(
                    $"/api/v1/movements/find-by-product/{productTest.Id}");

            // Assert
            int totalAmmountAdjustment = adjustment.Items.First(x => x.Product == productTest).Amount;
            int totalAmmountAdjustment1 = adjustment1.Items.First(x => x.Product == productTest).Amount;
            int totalAmmountOutput = outputs
                .SelectMany(x => x.Items.Where(x => x.Product == productTest)
                    .Select(x => x.Amount)).Sum(x => x);

            int totalAmmountInStockBeforeUndo = totalAmmountAdjustment - totalAmmountOutput;
            int totalAmmountInStockAfterUndo = totalAmmountAdjustment1;

            resultMovement1.HttpResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            resultUndo.HttpResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            resultMovement1.Content.Ammount.Should().NotBe(totalAmmountInStockBeforeUndo);
            resultMovement1.Content.Ammount.Should().Be(totalAmmountInStockAfterUndo);
        }

        private async Task MockUndoItemAjustmentTestAsync(IList<Domain.Entities.Product> products,
            Domain.Entities.Adjustment adjustment)
        {
            foreach (Domain.Entities.Product product in products)
                adjustment.AddItem(AdjustmentItemFaker.Generate(adjustment, product));

            adjustment.AddEvent(new AdjustmentClosedEvent(adjustment.Id));
            adjustment.Close();

            await _testsFixture.MockInDatabase(adjustment);
        }

        private async Task MockUndoItemOutputTestAsync(IList<Domain.Entities.Product> products,
            IList<Domain.Entities.Output> outputs)
        {
            foreach (Domain.Entities.Output output in outputs)
            {
                foreach (Domain.Entities.Product product in products)
                    output.AddItem(OutputItemFaker.Generate(output, product, 1));

                output.AddEvent(new OutputClosedEvent(output.Id));
                output.Close();

                await _testsFixture.MockInDatabase(output);
            }
        }
    }
}
