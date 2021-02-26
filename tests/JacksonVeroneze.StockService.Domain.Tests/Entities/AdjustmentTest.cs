using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Enums;
using Xunit;
using UtilCommon = JacksonVeroneze.StockService.Common.Fakers.Util;

namespace JacksonVeroneze.StockService.Domain.Tests.Entities
{
    public class AdjustmentTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("Adjustment", "Validate")]
        public void Adjustment_Validate_DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos()
        {
            // Arange && Act
            Func<Adjustment> func1 = () => new Adjustment(string.Empty, DateTime.Now);
            Func<Adjustment> func2 = () => new Adjustment(UtilCommon.GenerateStringFaker(101), DateTime.Now);
            Func<Adjustment> func3 = () => new Adjustment("descrição", DateTime.Now.AddDays(1));

            // Assert
            func1.Should().Throw<DomainException>();
            func2.Should().Throw<DomainException>();
            func3.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAdicionarOsItensCorretamenteQuandoValidos")]
        [Trait("Adjustment", "AddItemAsync")]
        public void Adjustment_AddItem_DeveAdicionarOsItensCorretamenteQuandoValidos()
        {
            // Arange
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();

            // Act
            IList<AdjustmentItem> itemsMock = AdjustmentItemFaker.GenerateFaker(adjustment).Generate(10);

            foreach (AdjustmentItem itemMock in itemsMock)
                adjustment.AddItem(itemMock);

            // Assert
            adjustment.Items.Should().HaveCount(10);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemEOMesmoJaExistir")]
        [Trait("Adjustment", "AddItemAsync")]
        public void Adjustment_AddItem_DeveGerarDomainExceptionQuandoAdicionarUmItemEOMesmoJaExistir()
        {
            // Arange
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();

            AdjustmentItem item1 = AdjustmentItemFaker.GenerateFaker(adjustment).Generate();

            adjustment.AddItem(item1);

            // Act
            Action act = () => adjustment.AddItem(item1);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemEEstiverFechado")]
        [Trait("Adjustment", "AddItemAsync")]
        public void Adjustment_AddItem_DeveGerarDomainExceptionQuandoAdicionarUmItemEEstiverFechado()
        {
            // Arange
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();

            adjustment.Close();

            AdjustmentItem item1 = AdjustmentItemFaker.GenerateFaker(adjustment).Generate();

            // Act
            Action act = () => adjustment.AddItem(item1);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoTentarAtualizarUmItemInexistente")]
        [Trait("Adjustment", "UpdateItemAsync")]
        public void Adjustment_UpdateItem_DeveGerarDomainExceptionQuandoTentarAtualizarUmItemInexistente()
        {
            // Arange
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();

            AdjustmentItem adjustmentItem = AdjustmentItemFaker.GenerateFaker(adjustment).Generate();

            // Act
            Action act = () => adjustment.UpdateItem(adjustmentItem);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteUmItemQuandoOMesmoExistir")]
        [Trait("Adjustment", "UpdateItemAsync")]
        public void Adjustment_UpdateItem_DeveAtualizarCorretamenteUmItemQuandoOMesmoExistir()
        {
            // Arange
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();

            AdjustmentItem adjustmentItem = AdjustmentItemFaker.GenerateFaker(adjustment).Generate();

            adjustment.AddItem(adjustmentItem);

            // Act
            adjustment.UpdateItem(adjustmentItem);

            // Assert
            adjustment.Items.Should().HaveCount(1);
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteQuandoItemExistir")]
        [Trait("Adjustment", "RemoveItemAsync")]
        public void Adjustment_RemoveItem_DeveRemoverCorretamenteQuandoItemExistir()
        {
            // Arange
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();

            IList<AdjustmentItem> itemsMock = AdjustmentItemFaker.GenerateFaker(adjustment).Generate(2);

            adjustment.AddItem(itemsMock.First());
            adjustment.AddItem(itemsMock.Last());

            // Act
            adjustment.RemoveItem(itemsMock.First());

            // Assert
            adjustment.Items.Should().HaveCount(1);
            adjustment.Items.Should().NotContain(x => x.Id == itemsMock.First().Id);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoRemoverUmItemInexistente")]
        [Trait("Adjustment", "RemoveItemAsync")]
        public void Adjustment_RemoveItem_DeveGerarDomainExceptionQuandoRemoverUmItemInexistente()
        {
            // Arange
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();

            IList<AdjustmentItem> itemsMock = AdjustmentItemFaker.GenerateFaker(adjustment).Generate(2);

            adjustment.AddItem(itemsMock.First());

            // Act
            Action act = () => adjustment.RemoveItem(itemsMock.Last());

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveSomarCorretamenteValorTotal")]
        [Trait("Adjustment", "CalculateTotalValue")]
        public void Adjustment_CalculateTotalValue_DeveSomarCorretamenteValorTotal()
        {
            // Arange && Act
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();

            IList<AdjustmentItem> itemsMock = AdjustmentItemFaker.GenerateFaker(adjustment).Generate(10);

            foreach (AdjustmentItem itemMock in itemsMock)
                adjustment.AddItem(itemMock);

            // Assert
            adjustment.TotalValue.Should().Be(itemsMock.Sum(x => x.CalculteValue()));
        }

        [Fact(DisplayName = "DeveSetarOStatusParaFechadoCorretamenteSeEstiverAberto")]
        [Trait("Adjustment", "CloseAsync")]
        public void Adjustment_AddItem_DeveSetarOStatusParaFechadoCorretamenteSeEstiverAberto()
        {
            // Arange
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();

            // Act
            adjustment.Close();

            // Assert
            adjustment.State.Should().Be(AdjustmentState.Closed);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionAoFecharORegistroQueEstaFechado")]
        [Trait("Adjustment", "CloseAsync")]
        public void Adjustment_RemoveItem_DeveGerarDomainExceptionAoFecharORegistroQueEstaFechado()
        {
            // Arange
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();

            adjustment.Close();

            // Act
            Action act = () => adjustment.Close();

            // Assert
            act.Should().Throw<DomainException>();
        }
    }
}
