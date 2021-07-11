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
    public class DevolutionTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("Devolution", "ValidateAsync")]
        public void Devolution_Validate_DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos()
        {
            // Arange && Act
            Func<Devolution> func1 = () => new Devolution(string.Empty, DateTime.Now);
            Func<Devolution> func2 = () => new Devolution(UtilCommon.GenerateStringFaker(201), DateTime.Now);
            Func<Devolution> func3 = () => new Devolution("descrição", DateTime.Now.AddDays(1));

            // Assert
            func1.Should().Throw<DomainException>();
            func2.Should().Throw<DomainException>();
            func3.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAdicionarOsItensCorretamenteQuandoValidos")]
        [Trait("Devolution", "AddItemAsync")]
        public void Devolution_AddItem_DeveAdicionarOsItensCorretamenteQuandoValidos()
        {
            // Arange
            Devolution devolution = DevolutionFaker.Generate();

            // Act
            IList<DevolutionItem> itemsMock = DevolutionItemFaker.Generate(devolution, 10);

            foreach (DevolutionItem itemMock in itemsMock)
                devolution.AddItem(itemMock);

            // Assert
            devolution.Items.Should().HaveCount(10);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemEOMesmoJaExistir")]
        [Trait("Devolution", "AddItemAsync")]
        public void Devolution_AddItem_DeveGerarDomainExceptionQuandoAdicionarUmItemEOMesmoJaExistir()
        {
            // Arange
            Devolution devolution = DevolutionFaker.Generate();

            DevolutionItem item1 = DevolutionItemFaker.Generate(devolution);

            devolution.AddItem(item1);

            // Act
            Action act = () => devolution.AddItem(item1);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemEEstiverFechado")]
        [Trait("Devolution", "AddItemAsync")]
        public void Devolution_AddItem_DeveGerarDomainExceptionQuandoAdicionarUmItemEEstiverFechado()
        {
            // Arange
            Devolution devolution = DevolutionFaker.Generate();

            devolution.Close();

            DevolutionItem item1 = DevolutionItemFaker.Generate(devolution);

            // Act
            Action act = () => devolution.AddItem(item1);

            // Assert
            act.Should().Throw<DomainException>();
        }


        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoTentarAtualizarUmItemInexistente")]
        [Trait("Devolution", "UpdateItemAsync")]
        public void Devolution_UpdateItem_DeveGerarDomainExceptionQuandoTentarAtualizarUmItemInexistente()
        {
            // Arange
            Devolution devolution = DevolutionFaker.Generate();

            DevolutionItem devolutionItem = DevolutionItemFaker.Generate(devolution);

            // Act
            Action act = () => devolution.UpdateItem(devolutionItem);

            // Assert
            act.Should().Throw<NotFoundException>();
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemComProdutoQueJaExisteNaLista")]
        [Trait("Devolution", "AddItem")]
        public void Devolution_AddItem_DeveGerarDomainExceptionQuandoAdicionarUmItemComProdutoQueJaExisteNaLista()
        {
            // Arange
            Devolution devolution = DevolutionFaker.Generate();

            Product product = ProductFaker.Generate();

            DevolutionItem devolutionItem1 = DevolutionItemFaker.Generate(devolution, product);
            DevolutionItem devolutionItem2 = DevolutionItemFaker.Generate(devolution, product);

            devolution.AddItem(devolutionItem1);

            // Act
            Action act = () => devolution.AddItem(devolutionItem2);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemComProdutoQueJaExisteNaLista")]
        [Trait("Devolution", "UpdateItem")]
        public void Devolution_UpdateItem_DeveGerarDomainExceptionQuandoAtualizarUmItemComProdutoQueJaExisteNaLista()
        {
            // Arange
            Devolution devolution = DevolutionFaker.Generate();

            DevolutionItem devolutionItem1 = DevolutionItemFaker.Generate(devolution);
            DevolutionItem devolutionItem2 = DevolutionItemFaker.Generate(devolution);

            devolution.AddItem(devolutionItem1);
            devolution.AddItem(devolutionItem2);

            DevolutionItem devolutionItem3 = (DevolutionItem)devolutionItem2.ShallowCopy();

            devolutionItem3.Update(devolutionItem1.Amount, devolutionItem1.Product);

            // Act
            Action act = () => devolution.UpdateItem(devolutionItem3);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteUmItemQuandoOMesmoExistir")]
        [Trait("Devolution", "UpdateItemAsync")]
        public void Devolution_UpdateItem_DeveAtualizarCorretamenteUmItemQuandoOMesmoExistir()
        {
            // Arange
            Devolution devolution = DevolutionFaker.Generate();

            DevolutionItem devolutionItem = DevolutionItemFaker.Generate(devolution);

            devolution.AddItem(devolutionItem);

            // Act
            devolution.UpdateItem(devolutionItem);

            // Assert
            devolution.Items.Should().HaveCount(1);
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteQuandoItemExistir")]
        [Trait("Devolution", "RemoveItemAsync")]
        public void Devolution_RemoveItem_DeveRemoverCorretamenteQuandoItemExistir()
        {
            // Arange
            Devolution devolution = DevolutionFaker.Generate();

            IList<DevolutionItem> itemsMock = DevolutionItemFaker.Generate(devolution, 2);

            devolution.AddItem(itemsMock.First());
            devolution.AddItem(itemsMock.Last());

            // Act
            devolution.RemoveItem(itemsMock.First());

            // Assert
            devolution.Items.Should().HaveCount(1);
            devolution.Items.Should().NotContain(x => x.Id == itemsMock.First().Id);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoRemoverUmItemInexistente")]
        [Trait("Devolution", "RemoveItemAsync")]
        public void Devolution_RemoveItem_DeveGerarDomainExceptionQuandoRemoverUmItemInexistente()
        {
            // Arange
            Devolution devolution = DevolutionFaker.Generate();

            IList<DevolutionItem> itemsMock = DevolutionItemFaker.Generate(devolution, 2);

            devolution.AddItem(itemsMock.First());

            // Act
            Action act = () => devolution.RemoveItem(itemsMock.Last());

            // Assert
            act.Should().Throw<NotFoundException>();
        }

        [Fact(DisplayName = "DeveSetarOStatusParaFechadoCorretamenteSeEstiverAberto")]
        [Trait("Devolution", "CloseAsync")]
        public void Devolution_AddItem_DeveSetarOStatusParaFechadoCorretamenteSeEstiverAberto()
        {
            // Arange
            Devolution devolution = DevolutionFaker.Generate();

            // Act
            devolution.Close();

            // Assert
            devolution.State.Should().Be(DevolutionState.Closed);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionAoFecharORegistroQueEstaFechado")]
        [Trait("Devolution", "CloseAsync")]
        public void Devolution_RemoveItem_DeveGerarDomainExceptionAoFecharORegistroQueEstaFechado()
        {
            // Arange
            Devolution devolution = DevolutionFaker.Generate();

            devolution.Close();

            // Act
            Action act = () => devolution.Close();

            // Assert
            act.Should().Throw<DomainException>();
        }
    }
}
