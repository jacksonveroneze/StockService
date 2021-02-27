using System;
using FluentAssertions;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.DomainObjects.Exceptions;
using JacksonVeroneze.StockService.Domain.Entities;
using Xunit;

namespace JacksonVeroneze.StockService.Domain.Tests.Entities
{
    public class OutputItemTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("OutputItem", "Validate")]
        public void OutputItem_Validate_DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos()
        {
            // Arange && Act
            Output output = OutputFaker.GenerateFaker().Generate();
            Product product = ProductFaker.GenerateFaker().Generate();

            Func<OutputItem> func1 = () => new OutputItem(0, 1, output, product);
            Func<OutputItem> func2 = () => new OutputItem(1, 0, output, product);

            // Assert
            func1.Should().Throw<DomainException>();
            func2.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveCalculaCorretamenteOValorDoItem")]
        [Trait("OutputItem", "CalculteValue")]
        public void OutputItem_CalculteValue_DeveCalculaCorretamenteOValorDoItem()
        {
            // Arange && Act
            Output output = OutputFaker.GenerateFaker().Generate();
            Product product = ProductFaker.GenerateFaker().Generate();

            OutputItem item = new OutputItem(10, 2, output, product);

            // Act
            decimal value = item.CalculteValue();

            // Assert
            value.Should().Be(20);
        }

        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarAtualizarComValoresInvalidos")]
        [Trait("OutputItem", "Update")]
        public void OutputItem_Update_DeveRetornarDomainExceptionAoTentarAtualizarComValoresInvalidos()
        {
            // Arange && Act
            Output output = OutputFaker.GenerateFaker().Generate();
            Product product = ProductFaker.GenerateFaker().Generate();
            Product newProduct = ProductFaker.GenerateFaker().Generate();

            OutputItem item = new OutputItem(10, 2, output, product);

            // Act
            Action action1 = () => item.Update(0, 5, newProduct);
            Action action2 = () => item.Update(1, 0, newProduct);

            // Assert
            action1.Should().Throw<DomainException>();
            action2.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteOItemQuandoInformadoValoresValidos")]
        [Trait("OutputItem", "Update")]
        public void OutputItem_Update_DeveAtualizarCorretamenteOItemQuandoInformadoValoresValidos()
        {
            // Arange && Act
            Output output = OutputFaker.GenerateFaker().Generate();
            Product product = ProductFaker.GenerateFaker().Generate();
            Product newProduct = ProductFaker.GenerateFaker().Generate();

            OutputItem item = new OutputItem(10, 2, output, product);

            // Act
            item.Update(10, 5, newProduct);

            // Assert
            item.Amount.Should().Be(10);
            item.Value.Should().Be(5);
        }
    }
}
