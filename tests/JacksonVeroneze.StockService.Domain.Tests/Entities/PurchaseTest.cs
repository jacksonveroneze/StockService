using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using Xunit;

namespace JacksonVeroneze.StockService.Domain.Tests.Entities
{
    public class PurchaseTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoreInvalidos")]
        [Trait("Purchase", "Validate")]
        public void Purchase_Validate_DeveRetornarDomainExceptionAoTentarCriarComValoreInvalidos()
        {
            // Arange && Act
            Func<Purchase> func = () => new Purchase(string.Empty, DateTime.Now.AddDays(1));

            // Assert
            func.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAdicionarOsItensCorretamenteQuandoValidos")]
        [Trait("Purchase", "AddItem")]
        public void Purchase_AddItem_DeveAdicionarOsItensCorretamenteQuandoValidos()
        {
            // Arange
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();

            // Act
            IList<PurchaseItem> itemsMock = PurchaseItemFaker.GenerateFaker(purchase).Generate(10);

            foreach (PurchaseItem itemMock in itemsMock)
                purchase.AddItem(itemMock);

            // Assert
            purchase.Items.Should().HaveCount(10);
        }

        [Fact(DisplayName = "DeveAtualizarUmItemAoTentarAdicionarEOMesmoJaExistir")]
        [Trait("Purchase", "AddItem")]
        public void Purchase_AddItem_DeveAtualizarUmItemAoTentarAdicionarEOMesmoJaExistir()
        {
            // Arange
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();

            PurchaseItem item = PurchaseItemFaker.GenerateFaker(purchase).Generate();

            purchase.AddItem(item);

            // Act
            purchase.AddItem(item);

            // Assert
            purchase.Items.Should().HaveCount(1);
            purchase.Items.First().Amount.Should().Be(item.Amount);
            purchase.Items.First().Value.Should().Be(item.Value);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemEPurchaseIsClosed")]
        [Trait("Purchase", "AddItem")]
        public void Purchase_AddItem_DeveGerarDomainExceptionQuandoAdicionarUmItemEPurchaseIsClosed()
        {
            // Arange
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();

            purchase.Close();

            PurchaseItem item1 = PurchaseItemFaker.GenerateFaker(purchase).Generate();

            // Act
            Action act = () => purchase.AddItem(item1);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteQuandoItemExistir")]
        [Trait("Purchase", "RemoveItem")]
        public void Purchase_RemoveItem_DeveRemoverCorretamenteQuandoItemExistir()
        {
            // Arange
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();

            IList<PurchaseItem> itemsMock = PurchaseItemFaker.GenerateFaker(purchase).Generate(2);

            PurchaseItem item1 = itemsMock.First();
            PurchaseItem item2 = itemsMock.Last();

            purchase.AddItem(item1);
            purchase.AddItem(item2);

            // Act
            purchase.RemoveItem(item1);

            // Assert
            purchase.Items.Should().HaveCount(1);
            purchase.Items.Should().NotContain(x => x.Id == item1.Id);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoRemoverUmItemInexistente")]
        [Trait("Purchase", "RemoveItem")]
        public void Purchase_RemoveItem_DeveGerarDomainExceptionQuandoRemoverUmItemInexistente()
        {
            // Arange
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();

            IList<PurchaseItem> itemsMock = PurchaseItemFaker.GenerateFaker(purchase).Generate(2);

            PurchaseItem item1 = itemsMock.First();
            PurchaseItem item2 = itemsMock.Last();

            purchase.AddItem(item1);

            // Act
            Action act = () => purchase.RemoveItem(item2);

            // Assert
            act.Should().Throw<DomainException>();
        }


        [Fact(DisplayName = "DeveSomarCorretamenteValorTotal")]
        [Trait("Purchase", "CalculateTotalValue")]
        public void Purchase_CalculateTotalValue_DeveSomarCorretamenteValorTotal()
        {
            // Arange && Act
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();

            IList<PurchaseItem> itemsMock = PurchaseItemFaker.GenerateFaker(purchase).Generate(10);

            foreach (PurchaseItem itemMock in itemsMock)
                purchase.AddItem(itemMock);

            // Assert
            purchase.TotalValue.Should().Be(itemsMock.Sum(x => x.CalculteValue()));
        }

        [Fact(DisplayName = "DeveGerarFecharACompraCorretamenteSeEstiverAberta")]
        [Trait("Purchase", "Close")]
        public void Purchase_AddItem_DeveGerarFecharACompraCorretamenteSeEstiverAberta()
        {
            // Arange
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();

            // Act
            purchase.Close();

            // Assert
            purchase.State.Should().Be(PurchaseStateEnum.Closed);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionAoFecharUmaCompraJaFechada")]
        [Trait("Purchase", "Close")]
        public void Purchase_RemoveItem_DeveGerarDomainExceptionAoFecharUmaCompraJaFechada()
        {
            // Arange
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();

            purchase.Close();

            // Act
            Action act = () => purchase.Close();

            // Assert
            act.Should().Throw<DomainException>();
        }
    }
}
