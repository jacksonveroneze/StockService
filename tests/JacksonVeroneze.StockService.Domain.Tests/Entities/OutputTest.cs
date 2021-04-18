using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JacksonVeroneze.NET.Commons.Exceptions;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Enums;
using Xunit;
using UtilCommon = JacksonVeroneze.StockService.Common.Fakers.Util;

namespace JacksonVeroneze.StockService.Domain.Tests.Entities
{
    public class OutputTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("Output", "ValidateAsync")]
        public void Output_Validate_DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos()
        {
            // Arange && Act
            Func<Output> func1 = () => new Output(string.Empty, DateTime.Now);
            Func<Output> func2 = () => new Output(UtilCommon.GenerateStringFaker(101), DateTime.Now);
            Func<Output> func3 = () => new Output("descrição", DateTime.Now.AddDays(1));

            // Assert
            func1.Should().Throw<DomainException>();
            func2.Should().Throw<DomainException>();
            func3.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAdicionarOsItensCorretamenteQuandoValidos")]
        [Trait("Output", "AddItemAsync")]
        public void Output_AddItem_DeveAdicionarOsItensCorretamenteQuandoValidos()
        {
            // Arange
            Output output = OutputFaker.Generate();

            // Act
            IList<OutputItem> itemsMock = OutputItemFaker.Generate(output, 10);

            foreach (OutputItem itemMock in itemsMock)
                output.AddItem(itemMock);

            // Assert
            output.Items.Should().HaveCount(10);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemEOMesmoJaExistir")]
        [Trait("Output", "AddItemAsync")]
        public void Output_AddItem_DeveGerarDomainExceptionQuandoAdicionarUmItemEOMesmoJaExistir()
        {
            // Arange
            Output output = OutputFaker.Generate();

            OutputItem item1 = OutputItemFaker.Generate(output);

            output.AddItem(item1);

            // Act
            Action act = () => output.AddItem(item1);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemEEstiverFechado")]
        [Trait("Output", "AddItemAsync")]
        public void Output_AddItem_DeveGerarDomainExceptionQuandoAdicionarUmItemEEstiverFechado()
        {
            // Arange
            Output output = OutputFaker.Generate();

            output.Close();

            OutputItem item1 = OutputItemFaker.Generate(output);

            // Act
            Action act = () => output.AddItem(item1);

            // Assert
            act.Should().Throw<DomainException>();
        }


        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoTentarAtualizarUmItemInexistente")]
        [Trait("Output", "UpdateItemAsync")]
        public void Output_UpdateItem_DeveGerarDomainExceptionQuandoTentarAtualizarUmItemInexistente()
        {
            // Arange
            Output output = OutputFaker.Generate();

            OutputItem outputItem = OutputItemFaker.Generate(output);

            // Act
            Action act = () => output.UpdateItem(outputItem);

            // Assert
            act.Should().Throw<NotFoundException>();
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemComProdutoQueJaExisteNaLista")]
        [Trait("Output", "AddItem")]
        public void Output_AddItem_DeveGerarDomainExceptionQuandoAdicionarUmItemComProdutoQueJaExisteNaLista()
        {
            // Arange
            Output output = OutputFaker.Generate();

            Product product = ProductFaker.Generate();

            OutputItem outputItem1 = OutputItemFaker.Generate(output, product);
            OutputItem outputItem2 = OutputItemFaker.Generate(output, product);

            output.AddItem(outputItem1);

            // Act
            Action act = () => output.AddItem(outputItem2);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemComProdutoQueJaExisteNaLista")]
        [Trait("Output", "UpdateItem")]
        public void Output_UpdateItem_DeveGerarDomainExceptionQuandoAtualizarUmItemComProdutoQueJaExisteNaLista()
        {
            // Arange
            Output output = OutputFaker.Generate();

            OutputItem outputItem1 = OutputItemFaker.Generate(output);
            OutputItem outputItem2 = OutputItemFaker.Generate(output);

            output.AddItem(outputItem1);
            output.AddItem(outputItem2);

            OutputItem outputItem3 = (OutputItem)outputItem2.ShallowCopy();

            outputItem3.Update(outputItem1.Amount, outputItem1.Value, outputItem1.Product);

            // Act
            Action act = () => output.UpdateItem(outputItem3);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteUmItemQuandoOMesmoExistir")]
        [Trait("Output", "UpdateItemAsync")]
        public void Output_UpdateItem_DeveAtualizarCorretamenteUmItemQuandoOMesmoExistir()
        {
            // Arange
            Output output = OutputFaker.Generate();

            OutputItem outputItem = OutputItemFaker.Generate(output);

            output.AddItem(outputItem);

            // Act
            output.UpdateItem(outputItem);

            // Assert
            output.Items.Should().HaveCount(1);
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteQuandoItemExistir")]
        [Trait("Output", "RemoveItemAsync")]
        public void Output_RemoveItem_DeveRemoverCorretamenteQuandoItemExistir()
        {
            // Arange
            Output output = OutputFaker.Generate();

            IList<OutputItem> itemsMock = OutputItemFaker.Generate(output, 2);

            output.AddItem(itemsMock.First());
            output.AddItem(itemsMock.Last());

            // Act
            output.RemoveItem(itemsMock.First());

            // Assert
            output.Items.Should().HaveCount(1);
            output.Items.Should().NotContain(x => x.Id == itemsMock.First().Id);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoRemoverUmItemInexistente")]
        [Trait("Output", "RemoveItemAsync")]
        public void Output_RemoveItem_DeveGerarDomainExceptionQuandoRemoverUmItemInexistente()
        {
            // Arange
            Output output = OutputFaker.Generate();

            IList<OutputItem> itemsMock = OutputItemFaker.Generate(output, 2);

            output.AddItem(itemsMock.First());

            // Act
            Action act = () => output.RemoveItem(itemsMock.Last());

            // Assert
            act.Should().Throw<NotFoundException>();
        }

        [Fact(DisplayName = "DeveSomarCorretamenteValorTotal")]
        [Trait("Output", "CalculateTotalValue")]
        public void Output_CalculateTotalValue_DeveSomarCorretamenteValorTotal()
        {
            // Arange && Act
            Output output = OutputFaker.Generate();

            IList<OutputItem> itemsMock = OutputItemFaker.Generate(output, 10);

            foreach (OutputItem itemMock in itemsMock)
                output.AddItem(itemMock);

            // Assert
            output.TotalValue.Should().Be(itemsMock.Sum(x => x.CalculteValue()));
        }

        [Fact(DisplayName = "DeveSetarOStatusParaFechadoCorretamenteSeEstiverAberto")]
        [Trait("Output", "CloseAsync")]
        public void Output_AddItem_DeveSetarOStatusParaFechadoCorretamenteSeEstiverAberto()
        {
            // Arange
            Output output = OutputFaker.Generate();

            // Act
            output.Close();

            // Assert
            output.State.Should().Be(OutputState.Closed);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionAoFecharORegistroQueEstaFechado")]
        [Trait("Output", "CloseAsync")]
        public void Output_RemoveItem_DeveGerarDomainExceptionAoFecharORegistroQueEstaFechado()
        {
            // Arange
            Output output = OutputFaker.Generate();

            output.Close();

            // Act
            Action act = () => output.Close();

            // Assert
            act.Should().Throw<DomainException>();
        }
    }
}
