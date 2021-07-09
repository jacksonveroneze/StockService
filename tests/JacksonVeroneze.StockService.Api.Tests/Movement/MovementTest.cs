using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JacksonVeroneze.StockService.Api.Controllers.v1;
using JacksonVeroneze.StockService.Api.Tests.Configuration;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Common.Integration;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Events.Adjustment;
using JacksonVeroneze.StockService.Domain.Events.Output;
using JacksonVeroneze.StockService.Domain.Events.Purchase;
using JacksonVeroneze.StockService.Domain.Models;
using Xunit;

namespace JacksonVeroneze.StockService.Api.Tests.Movement
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class MovementTest
    {
        private const string _uriPart = "/api/v1/movements";

        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        private readonly IList<Domain.Entities.Product> _listProducts = ProductFaker.Generate(2);
        private readonly Domain.Entities.Adjustment _adjustment = AdjustmentFaker.Generate();
        private readonly Domain.Entities.Purchase _purchase = PurchaseFaker.Generate();
        private readonly Domain.Entities.Output _output = OutputFaker.Generate();

        public MovementTest(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;

            _testsFixture.DropDatabase().Wait();
            _testsFixture.CreateDatabase().Wait();
        }

        [Fact(DisplayName = "DeveFiltarOsDadosTotalizadosCorretamente")]
        [Trait(nameof(MovementsController), nameof(MovementsController.Filter))]
        public async Task MovementsController_Filter_DeveFiltarOsDadosTotalizadosCorretamente()
        {
            // Arrange
            await MockData();

            // Act
            TestApiResponseOperationGet<Pageable<MovementModel>> result =
                await _testsFixture.SendGetRequest<Pageable<MovementModel>>($"{_uriPart}/filter");

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();

            foreach (Domain.Entities.Product product in _listProducts)
            {
                int totalAmmountAdjustment = _adjustment.Items.Where(x => x.Product == product).Sum(x => x.Amount);
                int totalAmmountPurchase = _purchase.Items.Where(x => x.Product == product).Sum(x => x.Amount);
                int totalAmmountOutput = _output.Items.Where(x => x.Product == product).Sum(x => x.Amount);

                int totalAmmmountProduct = totalAmmountAdjustment + totalAmmountPurchase - totalAmmountOutput;

                result.Content.Data.First(x => x.ProductId == product.Id).Ammount.Should().Be(totalAmmmountProduct);
            }
        }

        [Fact(DisplayName = "DeveFiltarOsDadosPorProdutoTotalizadosCorretamente")]
        [Trait(nameof(MovementsController), nameof(MovementsController.Filter))]
        public async Task MovementsController_Filter_DeveFiltarOsDadosPorProdutoTotalizadosCorretamente()
        {
            // Arrange
            await MockData();

            Domain.Entities.Product product = _listProducts.First();

            // Act
            TestApiResponseOperationGet<Pageable<MovementModel>> result =
                await _testsFixture.SendGetRequest<Pageable<MovementModel>>($"{_uriPart}/filter/?ProductId={product.Id}");

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();

            int totalAmmountAdjustment = _adjustment.Items.Where(x => x.Product == product).Sum(x => x.Amount);
            int totalAmmountPurchase = _purchase.Items.Where(x => x.Product == product).Sum(x => x.Amount);
            int totalAmmountOutput = _output.Items.Where(x => x.Product == product).Sum(x => x.Amount);

            int totalAmmmountProduct = totalAmmountAdjustment + totalAmmountPurchase - totalAmmountOutput;

            result.Content.Data.First(x => x.ProductId == product.Id).Ammount.Should().Be(totalAmmmountProduct);
        }

        private async Task MockData()
        {
            await _testsFixture.MockInDatabase(_listProducts);

            foreach (Domain.Entities.Product product in _listProducts)
            {
                _adjustment.AddItem(AdjustmentItemFaker.Generate(_adjustment, product));
                _purchase.AddItem(PurchaseItemFaker.Generate(_purchase, product));
                _output.AddItem(OutputItemFaker.Generate(_output, product));
            }

            _adjustment.AddEvent(new AdjustmentClosedEvent(_adjustment.Id));
            _purchase.AddEvent(new PurchaseClosedEvent(_purchase.Id));
            _output.AddEvent(new OutputClosedEvent(_output.Id));

            _adjustment.Close();
            _purchase.Close();
            _output.Close();

            await _testsFixture.MockInDatabase(_adjustment);
            await _testsFixture.MockInDatabase(_purchase);
            await _testsFixture.MockInDatabase(_output);
        }
    }
}
