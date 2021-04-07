using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using JacksonVeroneze.StockService.Api.Controllers.v1;
using JacksonVeroneze.StockService.Api.Tests.Configuration;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Common.Integration;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Events.Adjustment;
using JacksonVeroneze.StockService.Domain.Models;
using Xunit;

namespace JacksonVeroneze.StockService.Api.Tests.Movement
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class MovementTest
    {
        private const string _uriPart = "/api/v1/movements";

        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public MovementTest(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;

            _testsFixture.ClearDatabase().Wait();
            _testsFixture.RunMigrations().Wait();
        }

        [Fact(DisplayName = "DeveFiltarOsDadosTotalizadosCorretamente", Skip = "Terminar")]
        [Trait(nameof(MovementsController), nameof(MovementsController.Filter))]
        public async Task MovementsController_Filter_DeveFiltarOsDadosTotalizadosCorretamente()
        {
            // Arrange
            IList<Domain.Entities.Product> listProducts = ProductFaker.Generate(10);

            await _testsFixture.MockInDatabase(listProducts);

            Domain.Entities.Adjustment adjustment = AdjustmentFaker.Generate();
            Domain.Entities.Output output = OutputFaker.Generate();
            Domain.Entities.Purchase purchase = PurchaseFaker.Generate();

            foreach (Domain.Entities.Product product in listProducts)
            {
                adjustment.AddItem(AdjustmentItemFaker.Generate(adjustment, product));
                output.AddItem(OutputItemFaker.Generate(output, product));
                purchase.AddItem(PurchaseItemFaker.Generate(purchase, product));
            }

            // Chamar Close ????
            adjustment.AddEvent(new AdjustmentClosedEvent(adjustment.Id));

            // adjustment.Close();
            // output.Close();
            // purchase.Close();

            await _testsFixture.MockInDatabase(adjustment);
            await _testsFixture.MockInDatabase(output);
            await _testsFixture.MockInDatabase(purchase);

            // Act
            TestApiResponseOperationGet<Pageable<MovementModel>> result =
                await _testsFixture.SendGetRequest<Pageable<MovementModel>>($"{_uriPart}/");

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
        }
    }
}
