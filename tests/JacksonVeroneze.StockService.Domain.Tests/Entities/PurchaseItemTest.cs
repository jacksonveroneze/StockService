using System;
using FluentAssertions;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using Xunit;

namespace JacksonVeroneze.StockService.Domain.Tests.Entities
{
    public class PurchaseItemTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("PurchaseItem", "Validate")]
        public void PurchaseItem_Validate_DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos()
        {
            // Arange && Act
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();
            Product product = ProductFaker.GenerateFaker().Generate();

            Func<PurchaseItem> func1 = () => new PurchaseItem(0, 1, purchase, product);
            Func<PurchaseItem> func2 = () => new PurchaseItem(1, 0, purchase, product);

            // Assert
            func1.Should().Throw<DomainException>();
            func2.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("PurchaseItem", "Validate")]
        public void PurchaseItem_Validate_DeveCalculaCorretamenteOValorDoItem()
        {
            // Arange && Act
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();
            Product product = ProductFaker.GenerateFaker().Generate();

            PurchaseItem item = new PurchaseItem(10, 2, purchase, product);

            // Act
            decimal value = item.CalculteValue();

            // Assert
            value.Should().Be(20);
        }

        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("PurchaseItem", "Validate")]
        public void PurchaseItem_Validate_DeveAtualizarCorretamenteOItemQuandoInformadoValoresValidos()
        {
            // Arange && Act
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();
            Product product = ProductFaker.GenerateFaker().Generate();

            PurchaseItem item = new PurchaseItem(10, 2, purchase, product);

            // Act
            item.Update(10,5);

            // Assert
            item.Amount.Should().Be(10);
            item.Value.Should().Be(5);
        }
    }
}