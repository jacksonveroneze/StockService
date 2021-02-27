using System;
using FluentAssertions;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.DomainObjects.Exceptions;
using JacksonVeroneze.StockService.Domain.Entities;
using Xunit;

namespace JacksonVeroneze.StockService.Domain.Tests.Entities
{
    public class AdjustmentItemTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("AdjustmentItem", "Validate")]
        public void AdjustmentItem_Validate_DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos()
        {
            // Arange && Act
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();
            Product product = ProductFaker.GenerateFaker().Generate();

            Func<AdjustmentItem> func1 = () => new AdjustmentItem(0, 1, adjustment, product);
            Func<AdjustmentItem> func2 = () => new AdjustmentItem(1, 0, adjustment, product);

            // Assert
            func1.Should().Throw<DomainException>();
            func2.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveCalculaCorretamenteOValorDoItem")]
        [Trait("AdjustmentItem", "CalculteValue")]
        public void AdjustmentItem_CalculteValue_DeveCalculaCorretamenteOValorDoItem()
        {
            // Arange && Act
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();
            Product product = ProductFaker.GenerateFaker().Generate();

            AdjustmentItem item = new AdjustmentItem(10, 2, adjustment, product);

            // Act
            decimal value = item.CalculteValue();

            // Assert
            value.Should().Be(20);
        }

        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarAtualizarComValoresInvalidos")]
        [Trait("AdjustmentItem", "Update")]
        public void AdjustmentItem_Update_DeveRetornarDomainExceptionAoTentarAtualizarComValoresInvalidos()
        {
            // Arange && Act
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();
            Product product = ProductFaker.GenerateFaker().Generate();
            Product newProduct = ProductFaker.GenerateFaker().Generate();

            AdjustmentItem item = new AdjustmentItem(10, 2, adjustment, product);

            // Act
            Action action1 = () => item.Update(0, 5, newProduct);
            Action action2 = () => item.Update(1, 0, newProduct);

            // Assert
            action1.Should().Throw<DomainException>();
            action2.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteOItemQuandoInformadoValoresValidos")]
        [Trait("AdjustmentItem", "Update")]
        public void AdjustmentItem_Update_DeveAtualizarCorretamenteOItemQuandoInformadoValoresValidos()
        {
            // Arange && Act
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();
            Product product = ProductFaker.GenerateFaker().Generate();
            Product newProduct = ProductFaker.GenerateFaker().Generate();

            AdjustmentItem item = new AdjustmentItem(10, 2, adjustment, product);

            // Act
            item.Update(10, 5, newProduct);

            // Assert
            item.Amount.Should().Be(10);
            item.Value.Should().Be(5);
        }
    }
}
