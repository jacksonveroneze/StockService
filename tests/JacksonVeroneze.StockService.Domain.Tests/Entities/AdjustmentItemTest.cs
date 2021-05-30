using System;
using FluentAssertions;
using JacksonVeroneze.NET.Commons.Exceptions;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Domain.Entities;
using Xunit;

namespace JacksonVeroneze.StockService.Domain.Tests.Entities
{
    public class AdjustmentItemTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("AdjustmentItem", "ValidateAsync")]
        public void AdjustmentItem_Validate_DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos()
        {
            // Arange && Act
            Adjustment adjustment = AdjustmentFaker.Generate();
            Product product = ProductFaker.Generate();

            Func<AdjustmentItem> func1 = () => new AdjustmentItem(0, adjustment, product);
            Func<AdjustmentItem> func2 = () => new AdjustmentItem(-1, adjustment, product);

            // Assert
            func1.Should().Throw<DomainException>();
            func2.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarAtualizarComValoresInvalidos")]
        [Trait("AdjustmentItem", "Update")]
        public void AdjustmentItem_Update_DeveRetornarDomainExceptionAoTentarAtualizarComValoresInvalidos()
        {
            // Arange && Act
            Adjustment adjustment = AdjustmentFaker.Generate();
            Product product = ProductFaker.Generate();
            Product newProduct = ProductFaker.Generate();

            AdjustmentItem item = new AdjustmentItem(10, adjustment, product);

            // Act
            Action action1 = () => item.Update(0, newProduct);
            Action action2 = () => item.Update(-1, newProduct);

            // Assert
            action1.Should().Throw<DomainException>();
            action2.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteOItemQuandoInformadoValoresValidos")]
        [Trait("AdjustmentItem", "Update")]
        public void AdjustmentItem_Update_DeveAtualizarCorretamenteOItemQuandoInformadoValoresValidos()
        {
            // Arange && Act
            Adjustment adjustment = AdjustmentFaker.Generate();
            Product product = ProductFaker.Generate();
            Product newProduct = ProductFaker.Generate();

            AdjustmentItem item = new AdjustmentItem(10, adjustment, product);

            // Act
            item.Update(10, newProduct);

            // Assert
            item.Amount.Should().Be(10);
        }
    }
}
