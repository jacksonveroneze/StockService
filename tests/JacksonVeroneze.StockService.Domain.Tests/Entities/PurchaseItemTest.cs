using System;
using FluentAssertions;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.DomainObjects.Exceptions;
using JacksonVeroneze.StockService.Domain.Entities;
using Xunit;

namespace JacksonVeroneze.StockService.Domain.Tests.Entities
{
    public class PurchaseItemTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("PurchaseItem", "ValidateAsync")]
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

        [Fact(DisplayName = "DeveCalculaCorretamenteOValorDoItem")]
        [Trait("PurchaseItem", "CalculteValue")]
        public void PurchaseItem_CalculteValue_DeveCalculaCorretamenteOValorDoItem()
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

        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarAtualizarComValoresInvalidos")]
        [Trait("PurchaseItem", "Update")]
        public void PurchaseItem_Update_DeveRetornarDomainExceptionAoTentarAtualizarComValoresInvalidos()
        {
            // Arange && Act
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();
            Product product = ProductFaker.GenerateFaker().Generate();
            Product newProduct = ProductFaker.GenerateFaker().Generate();

            PurchaseItem item = new PurchaseItem(10, 2, purchase, product);

            // Act
            Action action1 = () => item.Update(0, 5, newProduct);
            Action action2 = () => item.Update(1, 0, newProduct);

            // Assert
            action1.Should().Throw<DomainException>();
            action2.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteOItemQuandoInformadoValoresValidos")]
        [Trait("PurchaseItem", "Update")]
        public void PurchaseItem_Update_DeveAtualizarCorretamenteOItemQuandoInformadoValoresValidos()
        {
            // Arange && Act
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();
            Product product = ProductFaker.GenerateFaker().Generate();
            Product newProduct = ProductFaker.GenerateFaker().Generate();

            PurchaseItem item = new PurchaseItem(10, 2, purchase, product);

            // Act
            item.Update(10, 5, newProduct);

            // Assert
            item.Amount.Should().Be(10);
            item.Value.Should().Be(5);
        }
    }
}
