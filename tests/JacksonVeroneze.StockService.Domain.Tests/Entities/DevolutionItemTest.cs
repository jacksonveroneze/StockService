using System;
using FluentAssertions;
using JacksonVeroneze.NET.Commons.Exceptions;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Domain.Entities;
using Xunit;

namespace JacksonVeroneze.StockService.Domain.Tests.Entities
{
    public class DevolutionItemTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("DevolutionItem", "ValidateAsync")]
        public void DevolutionItem_Validate_DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos()
        {
            // Arange && Act
            Devolution devolution = DevolutionFaker.Generate();
            Product product = ProductFaker.Generate();

            Func<DevolutionItem> func1 = () => new DevolutionItem(0, devolution, product);
            Func<DevolutionItem> func2 = () => new DevolutionItem(-1, devolution, product);

            // Assert
            func1.Should().Throw<DomainException>();
            func2.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarAtualizarComValoresInvalidos")]
        [Trait("DevolutionItem", "Update")]
        public void DevolutionItem_Update_DeveRetornarDomainExceptionAoTentarAtualizarComValoresInvalidos()
        {
            // Arange && Act
            Devolution devolution = DevolutionFaker.Generate();
            Product product = ProductFaker.Generate();
            Product newProduct = ProductFaker.Generate();

            DevolutionItem item = new DevolutionItem(10, devolution, product);

            // Act
            Action action1 = () => item.Update(0, newProduct);
            Action action2 = () => item.Update(-1, newProduct);

            // Assert
            action1.Should().Throw<DomainException>();
            action2.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteOItemQuandoInformadoValoresValidos")]
        [Trait("DevolutionItem", "Update")]
        public void DevolutionItem_Update_DeveAtualizarCorretamenteOItemQuandoInformadoValoresValidos()
        {
            // Arange && Act
            Devolution devolution = DevolutionFaker.Generate();
            Product product = ProductFaker.Generate();
            Product newProduct = ProductFaker.Generate();

            DevolutionItem item = new DevolutionItem(10, devolution, product);

            // Act
            item.Update(10, newProduct);

            // Assert
            item.Amount.Should().Be(10);
        }
    }
}
