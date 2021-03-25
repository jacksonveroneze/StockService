using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JacksonVeroneze.StockService.Api.Controllers.v1;
using JacksonVeroneze.StockService.Api.Tests.Configuration;
using JacksonVeroneze.StockService.Application.DTO.Adjustment;
using JacksonVeroneze.StockService.Application.DTO.AdjustmentItem;
using JacksonVeroneze.StockService.Application.Validations;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Common.Integration;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace JacksonVeroneze.StockService.Api.Tests.Adjustment
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class AdjustmentTest
    {
        private const string _uriPart = "/api/v1/adjustments";

        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public AdjustmentTest(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;

            Task.Run(async () => await _testsFixture.ClearDatabase());
        }

        [Fact(DisplayName = "DeveFiltrarEPaginarOsDadosComSkipTakeCorretamente")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Filter))]
        public async Task AdjustmentsController_Filter_DeveFiltrarEPaginarOsDadosComSkipTakeCorretamente()
        {
            // Arrange
            int skip = 1;
            int take = 5;

            IList<Domain.Entities.Adjustment> products = AdjustmentFaker
                .Generate(30)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            await _testsFixture.MockInDatabase(products);

            // Act
            TestApiResponseOperationGet<Pageable<AdjustmentDto>> result =
                await _testsFixture.SendGetRequest<Pageable<AdjustmentDto>>(
                    $"{_uriPart}?skip={skip}&take={take}&description=a&state=1&dateInitial=2020-01-01&dateEnd=2022-01-01");

            // Assert
            IList<Domain.Entities.Adjustment> productsFiltered =
                products.Where(x => x.Description.Contains("a")).Skip(skip).Take(take).ToList();

            int total = products.Count(x => x.Description.Contains("a"));

            result.Should().NotBeNull();
            result.Content.Total.Should().Be(total);
            result.Content.Pages.Should().Be((int)Math.Ceiling(total / (decimal)take));
            result.Content.CurrentPage.Should().Be(skip);
            result.Content.Data.Should().HaveCount(productsFiltered.Count);
        }

        [Fact(DisplayName = "DeveBuscarCorretamentePeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Find))]
        public async Task AdjustmentsController_Find_DeveBuscarCorretamentePeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            IList<Domain.Entities.Adjustment> adjustments = AdjustmentFaker.Generate(10);

            Domain.Entities.Adjustment adjustment = adjustments.First();

            await _testsFixture.MockInDatabase(adjustments);

            // Act
            TestApiResponseOperationGet<AdjustmentDto> result =
                await _testsFixture.SendGetRequest<AdjustmentDto>($"{_uriPart}/{adjustment.Id}");

            // Assert
            result.Should().NotBeNull();
            result.Content.Id.Should().Be(adjustment.Id);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode404QuandoNaoEstiverCadastrado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Find))]
        public async Task AdjustmentsController_Find_DeveRetornarStatusCode404QuandoNaoEstiverCadastrado()
        {
            // Arrange && Act
            TestApiResponseOperationGet<AdjustmentDto> result =
                await _testsFixture.SendGetRequest<AdjustmentDto>($"{_uriPart}/{Guid.NewGuid()}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Status.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact(DisplayName = "DeveSalvarCorretamenteQuandoEmEstadoValido")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Create))]
        public async Task AdjustmentsController_Create_DeveSalvarCorretamenteQuandoEmEstadoValido()
        {
            // Arrange
            await _testsFixture.ClearDatabase();

            AddOrUpdateAdjustmentDto adjustmentDto = AddOrUpdateAdjustmentDtoFaker.GenerateValid();

            // Act
            TestApiResponseOperations<AdjustmentDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateAdjustmentDto, AdjustmentDto>(
                    $"{_uriPart}/", adjustmentDto);

            TestApiResponseOperationGet<AdjustmentDto> resultGet =
                await _testsFixture.SendGetRequest<AdjustmentDto>(result.HttpResponse.Headers.Location?.ToString());

            // Assert
            result.Should().NotBeNull();
            result.Data.Description.Should().Be(adjustmentDto.Description);
            result.Errors.Should().BeEmpty();

            resultGet.Should().NotBeNull();
            resultGet.Content.Description.Should().Be(adjustmentDto.Description);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarSalvarEmEstadoInvalido")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Create))]
        public async Task AdjustmentsController_Create_DeveRetornarErro400QuandoTentarSalvarEmEstadoInvalido()
        {
            // Arrange
            AddOrUpdateAdjustmentDto adjustmentDto = AddOrUpdateAdjustmentDtoFaker.GenerateInvalid();

            // Act
            TestApiResponseOperations<AdjustmentDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateAdjustmentDto, AdjustmentDto>(
                    $"{_uriPart}/", adjustmentDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteQuandoEmEstadoValido")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Update))]
        public async Task AdjustmentsController_Update_DeveAtualizarCorretamenteQuandoEmEstadoValido()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.Generate();

            await _testsFixture.MockInDatabase(adjustment);

            AddOrUpdateAdjustmentDto adjustmentDto = new() {Description = $"{adjustment.Description}_atualizado"};

            // Act
            TestApiResponseOperations<AdjustmentDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateAdjustmentDto, AdjustmentDto>(
                    $"{_uriPart}/{adjustment.Id}", adjustmentDto);

            // Assert
            result.Should().NotBeNull();
            result.Data.Description.Should().Be(adjustmentDto.Description);
            result.Errors.Should().BeEmpty();

            result.Should().NotBeNull();
            result.Data.Description.Should().Be(adjustmentDto.Description);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarEmEstadoInvalido")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Update))]
        public async Task AdjustmentsController_Update_DeveRetornarErro400QuandoTentarAtualizarEmEstadoInvalido()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.Generate();

            await _testsFixture.MockInDatabase(adjustment);

            AddOrUpdateAdjustmentDto adjustmentDto = new() {Description = string.Empty};

            // Act
            TestApiResponseOperations<AdjustmentDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateAdjustmentDto, AdjustmentDto>(
                    $"{_uriPart}/{adjustment.Id}", adjustmentDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarUmItemInexistente")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Update))]
        public async Task AdjustmentsController_Update_DeveRetornarErro400QuandoTentarAtualizarUmItemInexistente()
        {
            // Arrange
            AddOrUpdateAdjustmentDto adjustmentDto = AddOrUpdateAdjustmentDtoFaker.GenerateValid();

            // Act
            TestApiResponseOperations<AdjustmentDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateAdjustmentDto, AdjustmentDto>(
                    $"{_uriPart}/{Guid.NewGuid()}", adjustmentDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentNotFoundById));
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteQuandoEstiverCadastrado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Delete))]
        public async Task AdjustmentsController_Delete_DeveRemoverCorretamenteQuandoEstiverCadastrado()
        {
            // Arrange
            IList<Domain.Entities.Adjustment> adjustments = AdjustmentFaker.Generate(10);

            Domain.Entities.Adjustment adjustment = adjustments.First();

            await _testsFixture.MockInDatabase(adjustments);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{adjustment.Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode404QuandoRemoverENaoEstiverCadastrado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Delete))]
        public async Task AdjustmentsController_Delete_DeveRetornarStatusCode404QuandoRemoverENaoEstiverCadastrado()
        {
            // Arrange && Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{Guid.NewGuid()}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverETiverDependentes")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Delete))]
        public async Task AdjustmentsController_Delete_DeveRetornarStatusCode400QuandoRemoverETiverDependentes()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(10);

            await _testsFixture.MockInDatabase(adjustment);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{adjustment.Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentHasItems));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverETiverFechado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Delete))]
        public async Task AdjustmentsController_Delete_DeveRetornarStatusCode400QuandoRemoverETiverFechado()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.Generate();
            adjustment.Close();

            await _testsFixture.MockInDatabase(adjustment);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{adjustment.Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentIsClosed));
        }

        [Fact(DisplayName = "DeveFecharCorretamenteQuandoNaoEstiverFechado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Close))]
        public async Task AdjustmentsController_Close_DeveFecharCorretamenteQuandoNaoEstiverFechado()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(adjustment);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendPutEmptyBodyRequest($"{_uriPart}/{adjustment.Id}/close");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoFecharENaoEstiverCadastrado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Close))]
        public async Task AdjustmentsController_Close_DeveRetornarStatusCode400QuandoFecharENaoEstiverCadastrado()
        {
            // Arrange && Act
            TestApiResponseBase result =
                await _testsFixture.SendPutEmptyBodyRequest($"{_uriPart}/{Guid.NewGuid()}/close");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoFecharEJaEstiverFechado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.Close))]
        public async Task AdjustmentsController_Close_DeveRetornarStatusCode400QuandoFecharEJaEstiverFechado()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.Generate();
            adjustment.Close();

            await _testsFixture.MockInDatabase(adjustment);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendPutEmptyBodyRequest($"{_uriPart}/{adjustment.Id}/close");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentIsClosed));
        }

        [Fact(DisplayName = "DeveBuscarCorretamenteOsItensPeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.FindItems))]
        public async Task
            AdjustmentsController_FindItems_DeveBuscarCorretamenteOsItensPeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            int total = 10;

            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(total);

            await _testsFixture.MockInDatabase(adjustment);

            // Act
            TestApiResponseOperationGet<IList<AdjustmentItemDto>> result =
                await _testsFixture.SendGetRequest<IList<AdjustmentItemDto>>(
                    $"{_uriPart}/{adjustment.Id}/items");

            // Assert
            result.Should().NotBeNull();
            result.Content.Should().HaveCount(total);
        }

        [Fact(DisplayName = "DeveRetornarErro404QuandoBuscarOsItensCujoPaiNaoExiste")]
        [Trait(nameof(OutputsController), nameof(AdjustmentsController.FindItems))]
        public async Task AdjustmentsController_FindItems_DeveRetornarErro404QuandoBuscarOsItensCujoPaiNaoExiste()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(adjustment);

            // Act
            TestApiResponseOperationGet<IList<AdjustmentItemDto>> result =
                await _testsFixture.SendGetRequest<IList<AdjustmentItemDto>>(
                    $"{_uriPart}/{Guid.NewGuid()}/items");

            result.Status.Should().Be(StatusCodes.Status404NotFound);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact(DisplayName = "DeveBuscarCorretamenteOItemPeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.FindItem))]
        public async Task AdjustmentsController_FindItem_DeveBuscarCorretamenteOItemPeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(adjustment);

            // Act
            TestApiResponseOperationGet<AdjustmentItemDto> result =
                await _testsFixture.SendGetRequest<AdjustmentItemDto>(
                    $"{_uriPart}/{adjustment.Id}/items/{adjustment.Items.First().Id}");

            // Assert
            result.Should().NotBeNull();
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoBuscarUmItemCujoPaiNaoExiste")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.FindItem))]
        public async Task AdjustmentsController_FindItem_DeveRetornarErro404QuandoBuscarUmItemCujoPaiNaoExiste()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(adjustment);

            // Act
            TestApiResponseOperationGet<AdjustmentItemDto> result =
                await _testsFixture.SendGetRequest<AdjustmentItemDto>(
                    $"{_uriPart}/{Guid.NewGuid()}/items/{adjustment.Items.First().Id}");

            // Assert
            result.Status.Should().Be(StatusCodes.Status404NotFound);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact(DisplayName = "DeveSalvarCorretamenteOItemQuandoEmEstadoValido")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.CreateItem))]
        public async Task AdjustmentsController_CreateItem_DeveSalvarCorretamenteOItemQuandoEmEstadoValido()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.Generate();
            Domain.Entities.Product product = ProductFaker.Generate();

            await _testsFixture.MockInDatabase(adjustment);
            await _testsFixture.MockInDatabase(product);

            AddOrUpdateAdjustmentItemDto adjustmentItemDto =
                AddOrUpdateAdjustmentItemDtoFaker.GenerateValid(product.Id);

            // Act
            TestApiResponseOperations<AdjustmentItemDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateAdjustmentItemDto, AdjustmentItemDto>(
                    $"{_uriPart}/{adjustment.Id}/items", adjustmentItemDto);

            TestApiResponseOperationGet<AdjustmentItemDto> resultGet =
                await _testsFixture.SendGetRequest<AdjustmentItemDto>(
                    result.HttpResponse.Headers.Location?.ToString());

            // Assert
            result.Should().NotBeNull();
            result.Data.Amount.Should().Be(adjustmentItemDto.Amount);
            result.Data.Value.Should().Be(adjustmentItemDto.Value);
            result.Errors.Should().BeEmpty();

            resultGet.Should().NotBeNull();
            resultGet.Content.Amount.Should().Be(adjustmentItemDto.Amount);
            resultGet.Content.Value.Should().Be(adjustmentItemDto.Value);
        }


        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarCriarItemEmEstadoInvalido")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.CreateItem))]
        public async Task AdjustmentsController_CreateItem_DeveRetornarErro400QuandoTentarCriarItemEmEstadoInvalido()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.Generate();

            await _testsFixture.MockInDatabase(adjustment);

            AddOrUpdateAdjustmentItemDto adjustmentDto = new() {Amount = 0, Value = 0, ProductId = Guid.NewGuid()};

            // Act
            TestApiResponseOperations<AdjustmentDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateAdjustmentItemDto, AdjustmentDto>(
                    $"{_uriPart}/{adjustment.Id}/items", adjustmentDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoCadastrarOItemEPaiNaoEstiverCadastrado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.CreateItem))]
        public async Task
            AdjustmentsController_CreateItem_DeveRetornarStatusCode400QuandoCadastrarOItemEPaiNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.Generate();

            await _testsFixture.MockInDatabase(adjustment);

            AddOrUpdateAdjustmentItemDto adjustmentDto =
                AddOrUpdateAdjustmentItemDtoFaker.GenerateValid(Guid.NewGuid());

            // Act
            TestApiResponseOperations<AdjustmentDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateAdjustmentItemDto, AdjustmentDto>(
                    $"{_uriPart}/{Guid.NewGuid()}/items", adjustmentDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoCriarEOPaiEstiverFechado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.CreateItem))]
        public async Task AdjustmentsController_CreateItem_DeveRetornarStatusCode400QuandoCriarEOPaiEstiverFechado()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.Generate();
            adjustment.Close();

            await _testsFixture.MockInDatabase(adjustment);

            AddOrUpdateAdjustmentItemDto adjustmentItemDto =
                AddOrUpdateAdjustmentItemDtoFaker.GenerateValid(Guid.NewGuid());

            // Act
            TestApiResponseOperations<AdjustmentDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateAdjustmentItemDto, AdjustmentDto>(
                    $"{_uriPart}/{adjustment.Id}/items", adjustmentItemDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentIsClosed));
        }


        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoCriarItemComProdutoExistenteEmOutroItemNoPai")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.CreateItem))]
        public async Task
            AdjustmentsController_CreateItem_DeveRetornarStatusCode400QuandoCriarItemComProdutoExistenteEmOutroItemNoPai()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(5);

            await _testsFixture.MockInDatabase(adjustment);

            AddOrUpdateAdjustmentItemDto adjustmentItemDto = new()
            {
                ProductId = adjustment.Items.First().Product.Id, Amount = 10, Value = 20
            };

            // Act
            TestApiResponseOperations<AdjustmentDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateAdjustmentItemDto, AdjustmentDto>(
                    $"{_uriPart}/{adjustment.Id}/items", adjustmentItemDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteOItemQuandoEmEstadoValido")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.UpdateItem))]
        public async Task AdjustmentsController_UpdateItem_DeveAtualizarCorretamenteOItemQuandoEmEstadoValido()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(adjustment);

            AdjustmentItem adjustmentItem = adjustment.Items.First();

            AddOrUpdateAdjustmentItemDto adjustmentItemDto =
                new() {Amount = 10, Value = 20, ProductId = adjustmentItem.Product.Id};

            // Act
            TestApiResponseOperations<AdjustmentItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateAdjustmentItemDto, AdjustmentItemDto>(
                    $"{_uriPart}/{adjustment.Id}/items/{adjustmentItem.Id}", adjustmentItemDto);

            TestApiResponseOperationGet<AdjustmentItemDto> resultGet =
                await _testsFixture.SendGetRequest<AdjustmentItemDto>(
                    $"{_uriPart}/{adjustment.Id}/items/{adjustmentItem.Id}");

            // Assert
            result.Should().NotBeNull();
            result.Data.Amount.Should().Be(adjustmentItemDto.Amount);
            result.Data.Value.Should().Be(adjustmentItemDto.Value);
            result.Errors.Should().BeEmpty();

            resultGet.Should().NotBeNull();
            resultGet.Content.Amount.Should().Be(adjustmentItemDto.Amount);
            resultGet.Content.Value.Should().Be(adjustmentItemDto.Value);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarItemEmEstadoInvalido")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.UpdateItem))]
        public async Task AdjustmentsController_UpdateItem_DeveRetornarErro400QuandoTentarAtualizarItemEmEstadoInvalido()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(adjustment);

            AdjustmentItem adjustmentItem = adjustment.Items.First();

            AddOrUpdateAdjustmentItemDto adjustmentItemDto =
                new() {Amount = 0, Value = 0, ProductId = Guid.NewGuid()};

            // Act
            TestApiResponseOperations<AdjustmentItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateAdjustmentItemDto, AdjustmentItemDto>(
                    $"{_uriPart}/{adjustment.Id}/items/{adjustmentItem.Id}", adjustmentItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.ProductNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoAtualizarOItemEPaiNaoEstiverCadastrado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.UpdateItem))]
        public async Task
            AdjustmentsController_UpdateItem_DeveRetornarStatusCode400QuandoAtualizarOItemEPaiNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(adjustment);

            AdjustmentItem adjustmentItem = adjustment.Items.First();

            AddOrUpdateAdjustmentItemDto adjustmentItemDto = new();

            // Act
            TestApiResponseOperations<AdjustmentItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateAdjustmentItemDto, AdjustmentItemDto>(
                    $"{_uriPart}/{Guid.NewGuid()}/items/{adjustmentItem.Id}", adjustmentItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoAtualizarEOPaiEstiverFechado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.UpdateItem))]
        public async Task AdjustmentsController_UpdateItem_DeveRetornarStatusCode400QuandoAtualizarEOPaiEstiverFechado()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(2);
            adjustment.Close();

            await _testsFixture.MockInDatabase(adjustment);

            AdjustmentItem adjustmentItem = adjustment.Items.First();

            AddOrUpdateAdjustmentItemDto adjustmentItemDto = new();

            // Act
            TestApiResponseOperations<AdjustmentItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateAdjustmentItemDto, AdjustmentItemDto>(
                    $"{_uriPart}/{adjustment.Id}/items/{adjustmentItem.Id}", adjustmentItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentIsClosed));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoAtualizarOItemEOMesmoNaoEstiverCadastrado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.UpdateItem))]
        public async Task
            AdjustmentsController_UpdateItem_DeveRetornarStatusCode400QuandoAtualizarOItemEOMesmoNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(adjustment);

            AddOrUpdateAdjustmentItemDto adjustmentItemDto = new();

            // Act
            TestApiResponseOperations<AdjustmentItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateAdjustmentItemDto, AdjustmentItemDto>(
                    $"{_uriPart}/{adjustment.Id}/items/{Guid.NewGuid()}", adjustmentItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should()
                .Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentItemNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoAtualizarItemComProdutoExistenteEmOutroItemNoPai")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.UpdateItem))]
        public async Task
            AdjustmentsController_UpdateItem_DeveRetornarStatusCode400QuandoAtualizarItemComProdutoExistenteEmOutroItemNoPai()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(2);

            await _testsFixture.MockInDatabase(adjustment);

            AdjustmentItem adjustmentItem = adjustment.Items.First();

            AddOrUpdateAdjustmentItemDto adjustmentItemDto =
                new() {Amount = 10, Value = 20, ProductId = adjustment.Items.Last().Product.Id};

            // Act
            TestApiResponseOperations<AdjustmentItemDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateAdjustmentItemDto, AdjustmentItemDto>(
                    $"{_uriPart}/{adjustment.Id}/items/{adjustmentItem.Id}", adjustmentItemDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteOItenmPeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.DeleteItem))]
        public async Task
            AdjustmentsController_DeleteItem_DeveRemoverCorretamenteOItemPeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(10);

            await _testsFixture.MockInDatabase(adjustment);

            // Act
            TestApiResponseBase result = await _testsFixture.SendDeleteRequest(
                $"{_uriPart}/{adjustment.Id}/items/{adjustment.Items.First().Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverOItemEPaiNaoEstiverCadastrado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.DeleteItem))]
        public async Task
            AdjustmentsController_DeleteItem_DeveRetornarStatusCode400QuandoRemoverOItemEPaiNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(10);

            await _testsFixture.MockInDatabase(adjustment);

            // Act
            TestApiResponseBase result = await _testsFixture.SendDeleteRequest(
                $"{_uriPart}/{Guid.NewGuid()}/items/{adjustment.Items.First().Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverOItemEOMesmoNaoEstiverCadastrado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.DeleteItem))]
        public async Task
            AdjustmentsController_DeleteItem_DeveRetornarStatusCode400QuandoRemoverOItemEOMesmoNaoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(10);

            await _testsFixture.MockInDatabase(adjustment);

            // Act
            TestApiResponseBase result = await _testsFixture.SendDeleteRequest(
                $"{_uriPart}/{adjustment.Id}/items/{Guid.NewGuid()}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should()
                .Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentItemNotFoundById));
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverEOPaiEstiverFechado")]
        [Trait(nameof(AdjustmentsController), nameof(AdjustmentsController.DeleteItem))]
        public async Task
            AdjustmentsController_DeleteItem_DeveRetornarStatusCode400QuandoRemoverEOPaiEstiverFechado()
        {
            // Arrange
            Domain.Entities.Adjustment adjustment = AdjustmentFaker.GenerateWithItems(10);
            adjustment.Close();

            await _testsFixture.MockInDatabase(adjustment);

            // Act
            TestApiResponseBase result = await _testsFixture.SendDeleteRequest(
                $"{_uriPart}/{adjustment.Id}/items/{adjustment.Items.First().Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.AdjustmentIsClosed));
        }
    }
}
