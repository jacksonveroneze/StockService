using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using Xunit;
using UtilCommon = JacksonVeroneze.StockService.Common.Fakers.Util;

namespace JacksonVeroneze.StockService.Domain.Tests.Entities
{
    public class OutputTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("Output", "Validate")]
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
            Output output = OutputFaker.GenerateFaker().Generate();

            // Act
            IList<OutputItem> itemsMock = OutputItemFaker.GenerateFaker(output).Generate(10);

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
            Output output = OutputFaker.GenerateFaker().Generate();

            OutputItem item1 = OutputItemFaker.GenerateFaker(output).Generate();

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
            Output output = OutputFaker.GenerateFaker().Generate();

            output.Close();

            OutputItem item1 = OutputItemFaker.GenerateFaker(output).Generate();

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
            Output output = OutputFaker.GenerateFaker().Generate();

            OutputItem outputItem = OutputItemFaker.GenerateFaker(output).Generate();

            // Act
            Action act = () => output.UpdateItem(outputItem);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteUmItemQuandoOMesmoExistir")]
        [Trait("Output", "UpdateItemAsync")]
        public void Output_UpdateItem_DeveAtualizarCorretamenteUmItemQuandoOMesmoExistir()
        {
            // Arange
            Output output = OutputFaker.GenerateFaker().Generate();

            OutputItem outputItem = OutputItemFaker.GenerateFaker(output).Generate();

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
            Output output = OutputFaker.GenerateFaker().Generate();

            IList<OutputItem> itemsMock = OutputItemFaker.GenerateFaker(output).Generate(2);

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
            Output output = OutputFaker.GenerateFaker().Generate();

            IList<OutputItem> itemsMock = OutputItemFaker.GenerateFaker(output).Generate(2);

            output.AddItem(itemsMock.First());

            // Act
            Action act = () => output.RemoveItem(itemsMock.Last());

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveSomarCorretamenteValorTotal")]
        [Trait("Output", "CalculateTotalValue")]
        public void Output_CalculateTotalValue_DeveSomarCorretamenteValorTotal()
        {
            // Arange && Act
            Output output = OutputFaker.GenerateFaker().Generate();

            IList<OutputItem> itemsMock = OutputItemFaker.GenerateFaker(output).Generate(10);

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
            Output output = OutputFaker.GenerateFaker().Generate();

            // Act
            output.Close();

            // Assert
            output.State.Should().Be(OutputStateEnum.Closed);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionAoFecharORegistroQueEstaFechado")]
        [Trait("Output", "CloseAsync")]
        public void Output_RemoveItem_DeveGerarDomainExceptionAoFecharORegistroQueEstaFechado()
        {
            // Arange
            Output output = OutputFaker.GenerateFaker().Generate();

            output.Close();

            // Act
            Action act = () => output.Close();

            // Assert
            act.Should().Throw<DomainException>();
        }
    }
}
