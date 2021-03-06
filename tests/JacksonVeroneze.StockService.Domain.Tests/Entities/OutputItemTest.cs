using System;
using FluentAssertions;
using JacksonVeroneze.NET.Commons.Exceptions;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Domain.Entities;
using Xunit;

namespace JacksonVeroneze.StockService.Domain.Tests.Entities
{
    public class OutputItemTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("OutputItem", "ValidateAsync")]
        public void OutputItem_Validate_DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos()
        {
            // Arange && Act
            Output output = OutputFaker.Generate();
            Product product = ProductFaker.Generate();

            Func<OutputItem> func1 = () => new OutputItem(0, output, product);
            Func<OutputItem> func2 = () => new OutputItem(-1, output, product);

            // Assert
            func1.Should().Throw<DomainException>();
            func2.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarAtualizarComValoresInvalidos")]
        [Trait("OutputItem", "Update")]
        public void OutputItem_Update_DeveRetornarDomainExceptionAoTentarAtualizarComValoresInvalidos()
        {
            // Arange && Act
            Output output = OutputFaker.Generate();
            Product product = ProductFaker.Generate();
            Product newProduct = ProductFaker.Generate();

            OutputItem item = new OutputItem(10, output, product);

            // Act
            Action action1 = () => item.Update(0, newProduct);
            Action action2 = () => item.Update(-1, newProduct);

            // Assert
            action1.Should().Throw<DomainException>();
            action2.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteOItemQuandoInformadoValoresValidos")]
        [Trait("OutputItem", "Update")]
        public void OutputItem_Update_DeveAtualizarCorretamenteOItemQuandoInformadoValoresValidos()
        {
            // Arange && Act
            Output output = OutputFaker.Generate();
            Product product = ProductFaker.Generate();
            Product newProduct = ProductFaker.Generate();

            OutputItem item = new OutputItem(10, output, product);

            // Act
            item.Update(10, newProduct);

            // Assert
            item.Amount.Should().Be(10);
        }
    }
}
