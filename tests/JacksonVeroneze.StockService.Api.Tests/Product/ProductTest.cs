using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JacksonVeroneze.StockService.Api.Controllers.v1;
using JacksonVeroneze.StockService.Api.Tests.Configuration;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Validations;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Common.Integration;
using JacksonVeroneze.StockService.Core.Data;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace JacksonVeroneze.StockService.Api.Tests.Product
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class ProductTest
    {
        private const string _uriPart = "/api/v1/products";

        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public ProductTest(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;

            _testsFixture.ClearDatabase().Wait();
            _testsFixture.RunMigrations().Wait();
        }

        [Fact(DisplayName = "DeveFiltrarEPaginarOsDadosComSkipTakeCorretamente", Skip = "Refatorar teste")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Filter))]
        public async Task ProductController_Filter_DeveFiltrarEPaginarOsDadosComSkipTakeCorretamente()
        {
            // Arrange
            const int skip = 1;
            const int take = 5;

            IList<Domain.Entities.Product> products = ProductFaker.Generate(30)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            foreach (Domain.Entities.Product product in products.Skip(0).Take(15))
                product.Inactivate();

            await _testsFixture.MockInDatabase(products);

            // Act
            TestApiResponseOperationGet<Pageable<ProductDto>> result =
                await _testsFixture.SendGetRequest<Pageable<ProductDto>>(
                    $"{_uriPart}?skip={skip}&take={take}&description=a&isActive=true");

            // Assert
            IList<Domain.Entities.Product> productsFiltered =
                products.Where(x => x.Description.Contains("a") && x.IsActive).Skip(skip).Take(take).ToList();

            int total = products.Count(x => x.IsActive && x.Description.StartsWith("a"));

            result.Should().NotBeNull();
            result.Content.Total.Should().Be(total);
            result.Content.Pages.Should().Be((int)Math.Ceiling(total / (decimal)(take)));
            result.Content.CurrentPage.Should().Be(skip);
            result.Content.Data.Should().HaveCount(productsFiltered.Count);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact(DisplayName = "DeveBuscarCorretamentePeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Find))]
        public async Task ProductController_Find_DeveBuscarCorretamentePeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            IList<Domain.Entities.Product> products = ProductFaker.Generate(10);

            Domain.Entities.Product product = products.First();

            await _testsFixture.MockInDatabase(products);

            // Act
            TestApiResponseOperationGet<ProductDto> result =
                await _testsFixture.SendGetRequest<ProductDto>($"{_uriPart}/{product.Id}");

            // Assert
            result.Should().NotBeNull();
            result.Content.Id.Should().Be(product.Id);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode404QuandoNaoEstiverCadastrado")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Find))]
        public async Task ProductController_Find_DeveRetornarStatusCode404QuandoNaoEstiverCadastrado()
        {
            // Arrange && Act
            TestApiResponseOperationGet<ProductDto> result =
                await _testsFixture.SendGetRequest<ProductDto>($"{_uriPart}/{Guid.NewGuid()}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Status.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact(DisplayName = "DeveSalvarCorretamenteQuandoEmEstadoValido")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Create))]
        public async Task ProductController_Create_DeveSalvarCorretamenteQuandoEmEstadoValido()
        {
            // Arrange
            AddOrUpdateProductDto productDto = AddOrUpdateProductDtoFaker.GenerateValid();

            // Act
            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateProductDto, ProductDto>(
                    $"{_uriPart}/", productDto);

            TestApiResponseOperationGet<ProductDto> responseGet =
                await _testsFixture.SendGetRequest<ProductDto>(result.HttpResponse.Headers.Location?.ToString());

            // Assert
            result.Should().NotBeNull();
            result.Data.Description.Should().Be(productDto.Description);
            result.Data.IsActive.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status201Created);

            responseGet.Content.Should().NotBeNull();
            responseGet.Content.Description.Should().Be(productDto.Description);
            responseGet.Content.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarSalvarEmEstadoInvalido")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Create))]
        public async Task ProductController_Create_DeveRetornarErro400QuandoTentarSalvarEmEstadoInvalido()
        {
            // Arrange
            AddOrUpdateProductDto productDto = AddOrUpdateProductDtoFaker.GenerateInvalid();

            // Act
            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateProductDto, ProductDto>(
                    $"{_uriPart}/", productDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarSalvarUmItemJaCadastrado")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Create))]
        public async Task ProductController_Create_DeveRetornarErro400QuandoTentarSalvarUmItemJaCadastrado()
        {
            // Arrange
            Domain.Entities.Product product = ProductFaker.Generate();

            AddOrUpdateProductDto productDto = new() {Description = product.Description, IsActive = true};

            await _testsFixture.MockInDatabase(product);

            // Act
            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.SendPostRequest<AddOrUpdateProductDto, ProductDto>(
                    $"{_uriPart}/", productDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should()
                .Contain(x => x.Message.Equals(ApplicationValidationMessages.ProductFoundByDescription));
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteQuandoEmEstadoValido")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Update))]
        public async Task ProductController_Update_DeveAtualizarCorretamenteQuandoEmEstadoValido()
        {
            // Arrange
            Domain.Entities.Product product = ProductFaker.Generate();

            await _testsFixture.MockInDatabase(product);

            AddOrUpdateProductDto productDto = new() {Description = $"{product.Description}_atualizado", IsActive = true};

            // Act
            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateProductDto, ProductDto>(
                    $"{_uriPart}/{product.Id}", productDto);

            TestApiResponseOperationGet<ProductDto> responseGet =
                await _testsFixture.SendGetRequest<ProductDto>($"{_uriPart}/{product.Id}");

            // Assert
            result.Should().NotBeNull();
            result.Data.Description.Should().Be(productDto.Description);
            result.Data.IsActive.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

            responseGet.Should().NotBeNull();
            responseGet.Content.Description.Should().Be(productDto.Description);
            responseGet.Content.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarEmEstadoInvalido")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Update))]
        public async Task ProductController_Update_DeveRetornarErro400QuandoTentarAtualizarEmEstadoInvalido()
        {
            // Arrange
            Domain.Entities.Product product = ProductFaker.Generate();

            await _testsFixture.MockInDatabase(product);

            AddOrUpdateProductDto productDto = new() {Description = string.Empty, IsActive = true};

            // Act
            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateProductDto, ProductDto>(
                    $"{_uriPart}/{product.Id}", productDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarUmItemComDescricaoExistente")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Update))]
        public async Task ProductController_Update_DeveRetornarErro400QuandoTentarAtualizarUmItemComDescricaoExistente()
        {
            // Arrange
            IList<Domain.Entities.Product> products = ProductFaker.Generate(2);

            await _testsFixture.MockInDatabase(products);

            AddOrUpdateProductDto productDto = new() {Description = products.First().Description, IsActive = true};

            // Act
            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateProductDto, ProductDto>(
                    $"{_uriPart}/{products.Last().Id}", productDto);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should()
                .Contain(x => x.Message.Equals(ApplicationValidationMessages.ProductFoundByDescription));
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarUmItemInexistente")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Update))]
        public async Task ProductsController_Update_DeveRetornarErro400QuandoTentarAtualizarUmItemInexistente()
        {
            // Arrange
            AddOrUpdateProductDto productDto = AddOrUpdateProductDtoFaker.GenerateValid();

            // Act
            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.SendPutRequest<AddOrUpdateProductDto, ProductDto>($"{_uriPart}/{Guid.NewGuid()}",
                    productDto);

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.ProductNotFoundById));
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteQuandoEstiverCadastrado")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Delete))]
        public async Task ProductController_Delete_DeveRemoverCorretamenteQuandoEstiverCadastrado()
        {
            // Arrange
            Domain.Entities.Product product = ProductFaker.Generate();

            await _testsFixture.MockInDatabase(product);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{product.Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode404QuandoRemoverENaoEstiverCadastrado")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Delete))]
        public async Task ProductController_Delete_DeveRetornarStatusCode404QuandoRemoverENaoEstiverCadastrado()
        {
            // Arrange && Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{Guid.NewGuid()}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverETiverDependentes")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Delete))]
        public async Task ProductController_Delete_DeveRetornarStatusCode400QuandoRemoverETiverDependentes()
        {
            // Arrange
            Domain.Entities.Product product = ProductFaker.Generate();
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateWithItem(product);

            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseBase result =
                await _testsFixture.SendDeleteRequest($"{_uriPart}/{product.Id}");

            // Assert
            result.HttpResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
