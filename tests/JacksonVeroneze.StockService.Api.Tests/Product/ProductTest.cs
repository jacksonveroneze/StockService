using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using JacksonVeroneze.StockService.Api.Controllers.v1;
using JacksonVeroneze.StockService.Api.Tests.Configuration;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Validations;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Common.Integration;
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

            Task.Run(async () => await _testsFixture.ClearDatabase());
        }

        [Fact(DisplayName = "DeveFiltrarEPaginarOsDadosComSkipTakeCorretamente")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Filter))]
        public async Task ProductController_Filter_DeveFiltrarEPaginarOsDadosComSkipTakeCorretamente()
        {
            // Arrange
            int skip = 1;
            int take = 5;

            IList<Domain.Entities.Product> products = ProductFaker
                .GenerateFaker()
                .Generate(30)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            foreach (Domain.Entities.Product product in products.Skip(0).Take(15))
                product.Inactivate();

            await _testsFixture.MockInDatabase(products);

            // Act
            HttpResponseMessage response =
                await _testsFixture.Client.GetAsync($"{_uriPart}?skip={skip}&take={take}&description=a&isActive=true");

            TestApiResponsePageable<ProductDto> result =
                await _testsFixture.DeserializeObject<TestApiResponsePageable<ProductDto>>(response);

            // Assert
            IList<Domain.Entities.Product> productsFiltered =
                products.Where(x => x.Description.Contains("a") && x.IsActive).Skip(skip).Take(take).ToList();

            int total = products.Count(x => x.IsActive && x.Description.Contains("a"));

            result.Should().NotBeNull();
            result.Total.Should().Be(total);
            result.Pages.Should().Be((int)Math.Ceiling(total / (decimal)(take)));
            result.CurrentPage.Should().Be(skip);
            result.Data.Should().HaveCount(productsFiltered.Count);
        }

        [Fact(DisplayName = "DeveBuscarCorretamentePeloIdQuandoOMesmoEstiverCadastrado")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Find))]
        public async Task ProductController_Find_DeveBuscarCorretamentePeloIdQuandoOMesmoEstiverCadastrado()
        {
            // Arrange
            IList<Domain.Entities.Product> products = ProductFaker.GenerateFaker().Generate(10);

            Domain.Entities.Product product = products.First();

            await _testsFixture.MockInDatabase(products);

            // Act
            HttpResponseMessage response = await _testsFixture.Client.GetAsync($"{_uriPart}/{product.Id}");

            ProductDto result = await _testsFixture.DeserializeObject<ProductDto>(response);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(product.Id);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode404QuandoNaoEstiverCadastrado")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Find))]
        public async Task ProductController_Find_DeveRetornarStatusCode404QuandoNaoEstiverCadastrado()
        {
            // Arrange && Act
            HttpResponseMessage response =
                await _testsFixture.Client.GetAsync($"{_uriPart}/{Guid.NewGuid()}");

            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<ProductDto>>(response);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Status.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact(DisplayName = "DeveSalvarCorretamenteQuandoEmEstadoValido")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Create))]
        public async Task ProductController_Create_DeveSalvarCorretamenteQuandoEmEstadoValido()
        {
            await _testsFixture.ClearDatabase();

            // Arrange
            AddOrUpdateProductDto productDto = AddOrUpdateProductDtoFaker.GenerateValidFaker().Generate();

            // Act
            HttpResponseMessage response = await _testsFixture.Client.PostAsJsonAsync($"{_uriPart}/", productDto);

            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<ProductDto>>(response);

            Uri location = response.Headers.Location;

            HttpResponseMessage responseGet = await _testsFixture.Client.GetAsync(location);

            ProductDto resultGet = await _testsFixture.DeserializeObject<ProductDto>(responseGet);

            // Assert
            result.Should().NotBeNull();
            result.Data.Description.Should().Be(productDto.Description);
            result.Data.IsActive.Should().BeTrue();
            result.Errors.Should().BeEmpty();

            resultGet.Should().NotBeNull();
            resultGet.Description.Should().Be(productDto.Description);
            resultGet.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarSalvarEmEstadoInvalido")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Create))]
        public async Task ProductController_Create_DeveRetornarErro400QuandoTentarSalvarEmEstadoInvalido()
        {
            // Arrange
            AddOrUpdateProductDto productDto = AddOrUpdateProductDtoFaker.GenerateInvalidFaker().Generate();

            // Act
            HttpResponseMessage response = await _testsFixture.Client.PostAsJsonAsync($"{_uriPart}/", productDto);

            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<ProductDto>>(response);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarSalvarUmItemJaCadastrado")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Create))]
        public async Task ProductController_Create_DeveRetornarErro400QuandoTentarSalvarUmItemJaCadastrado()
        {
            // Arrange
            Domain.Entities.Product product = ProductFaker.GenerateFaker().Generate();

            AddOrUpdateProductDto productDto = new() {Description = product.Description, IsActive = true};

            await _testsFixture.MockInDatabase(product);

            // Act
            HttpResponseMessage response = await _testsFixture.Client.PostAsJsonAsync($"{_uriPart}/", productDto);

            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<ProductDto>>(response);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.ProductFoundByDescription));
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteQuandoEmEstadoValido")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Update))]
        public async Task ProductController_Update_DeveAtualizarCorretamenteQuandoEmEstadoValido()
        {
            // Arrange
            Domain.Entities.Product product = ProductFaker.GenerateFaker().Generate();

            await _testsFixture.MockInDatabase(product);

            AddOrUpdateProductDto productDto = new()
            {
                Description = $"{product.Description}_atualizado", IsActive = true
            };

            // Act
            HttpResponseMessage response =
                await _testsFixture.Client.PutAsJsonAsync($"{_uriPart}/{product.Id}", productDto);

            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<ProductDto>>(response);

            HttpResponseMessage responseGet = await _testsFixture.Client.GetAsync($"{_uriPart}/{product.Id}");

            ProductDto resultGet = await _testsFixture.DeserializeObject<ProductDto>(responseGet);

            // Assert
            result.Should().NotBeNull();
            result.Data.Description.Should().Be(productDto.Description);
            result.Data.IsActive.Should().BeTrue();
            result.Errors.Should().BeEmpty();

            resultGet.Should().NotBeNull();
            resultGet.Description.Should().Be(productDto.Description);
            resultGet.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarEmEstadoInvalido")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Update))]
        public async Task ProductController_Update_DeveRetornarErro400QuandoTentarAtualizarEmEstadoInvalido()
        {
            // Arrange
            Domain.Entities.Product product = ProductFaker.GenerateFaker().Generate();

            await _testsFixture.MockInDatabase(product);

            AddOrUpdateProductDto productDto = new() {Description = string.Empty, IsActive = true};

            // Act
            HttpResponseMessage response =
                await _testsFixture.Client.PutAsJsonAsync($"{_uriPart}/{product.Id}", productDto);

            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<ProductDto>>(response);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarUmItemComDescricaoExistente")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Update))]
        public async Task ProductController_Update_DeveRetornarErro400QuandoTentarAtualizarUmItemComDescricaoExistente()
        {
            // Arrange
            IList<Domain.Entities.Product> products = ProductFaker.GenerateFaker().Generate(2);

            await _testsFixture.MockInDatabase(products);

            AddOrUpdateProductDto productDto = new() {Description = products.First().Description, IsActive = true};

            // Act
            HttpResponseMessage response =
                await _testsFixture.Client.PutAsJsonAsync($"{_uriPart}/{products.Last().Id}", productDto);

            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<ProductDto>>(response);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.ProductFoundByDescription));
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarErro400QuandoTentarAtualizarUmItemInexistente")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Update))]
        public async Task ProductsController_Update_DeveRetornarErro400QuandoTentarAtualizarUmItemInexistente()
        {
            // Arrange
            AddOrUpdateProductDto productDto = AddOrUpdateProductDtoFaker.GenerateValidFaker().Generate();

            // Act
            HttpResponseMessage response =
                await _testsFixture.Client.PutAsJsonAsync($"{_uriPart}/{Guid.NewGuid()}", productDto);

            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<ProductDto>>(response);

            // Assert
            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
            result.Errors.Should().Contain(x => x.Message.Equals(ApplicationValidationMessages.ProductNotFoundById));
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteQuandoEstiverCadastrado")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Delete))]
        public async Task ProductController_Delete_DeveRemoverCorretamenteQuandoEstiverCadastrado()
        {
            // Arrange
            IList<Domain.Entities.Product> products = ProductFaker.GenerateFaker().Generate(10);

            Domain.Entities.Product product = products.First();

            await _testsFixture.MockInDatabase(products);

            // Act
            HttpResponseMessage response = await _testsFixture.Client.DeleteAsync($"{_uriPart}/{product.Id}");

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode404QuandoRemoverENaoEstiverCadastrado")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Delete))]
        public async Task ProductController_Delete_DeveRetornarStatusCode404QuandoRemoverENaoEstiverCadastrado()
        {
            // Arrange && Act
            HttpResponseMessage response =
                await _testsFixture.Client.DeleteAsync($"{_uriPart}/{Guid.NewGuid()}");

            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<ProductDto>>(response);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "DeveRetornarStatusCode400QuandoRemoverETiverDependentes")]
        [Trait(nameof(ProductsController), nameof(ProductsController.Delete))]
        public async Task ProductController_Delete_DeveRetornarStatusCode400QuandoRemoverETiverDependentes()
        {
            // Arrange
            Domain.Entities.Product product = ProductFaker.GenerateFaker().Generate();
            Domain.Entities.Purchase purchase = PurchaseFaker.GenerateFaker().Generate();
            Domain.Entities.PurchaseItem purchaseItem = PurchaseItemFaker.GenerateFaker(purchase, product).Generate();

            purchase.AddItem(purchaseItem);

            await _testsFixture.MockInDatabase(purchase);

            // Act
            HttpResponseMessage response = await _testsFixture.Client.DeleteAsync($"{_uriPart}/{product.Id}");

            TestApiResponseOperations<ProductDto> result =
                await _testsFixture.DeserializeObject<TestApiResponseOperations<ProductDto>>(response);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Status.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
