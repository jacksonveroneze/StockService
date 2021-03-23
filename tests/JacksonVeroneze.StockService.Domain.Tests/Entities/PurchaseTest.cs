using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Core.DomainObjects.Exceptions;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Enums;
using Xunit;
using UtilCommon = JacksonVeroneze.StockService.Common.Fakers.Util;

namespace JacksonVeroneze.StockService.Domain.Tests.Entities
{
    public class PurchaseTest
    {
        [Fact(DisplayName = "DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos")]
        [Trait("Purchase", "ValidateAsync")]
        public void Purchase_Validate_DeveRetornarDomainExceptionAoTentarCriarComValoresInvalidos()
        {
            // Arange && Act
            Func<Purchase> func1 = () => new Purchase(string.Empty, DateTime.Now);
            Func<Purchase> func2 = () => new Purchase(UtilCommon.GenerateStringFaker(101), DateTime.Now);
            Func<Purchase> func3 = () => new Purchase("descrição", DateTime.Now.AddDays(1));

            // Assert
            func1.Should().Throw<DomainException>();
            func2.Should().Throw<DomainException>();
            func3.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAdicionarOsItensCorretamenteQuandoValidos")]
        [Trait("Purchase", "AddItemAsync")]
        public void Purchase_AddItem_DeveAdicionarOsItensCorretamenteQuandoValidos()
        {
            // Arange
            Purchase purchase = PurchaseFaker.Generate();

            // Act
            IList<PurchaseItem> itemsMock = PurchaseItemFaker.Generate(purchase, 10);

            foreach (PurchaseItem itemMock in itemsMock)
                purchase.AddItem(itemMock);

            // Assert
            purchase.Items.Should().HaveCount(10);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemEOMesmoJaExistir")]
        [Trait("Purchase", "AddItemAsync")]
        public void Purchase_AddItem_DeveGerarDomainExceptionQuandoAdicionarUmItemEOMesmoJaExistir()
        {
            // Arange
            Purchase purchase = PurchaseFaker.Generate();

            PurchaseItem item1 = PurchaseItemFaker.Generate(purchase);

            purchase.AddItem(item1);

            // Act
            Action act = () => purchase.AddItem(item1);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemEEstiverFechado")]
        [Trait("Purchase", "AddItemAsync")]
        public void Purchase_AddItem_DeveGerarDomainExceptionQuandoAdicionarUmItemEEstiverFechado()
        {
            // Arange
            Purchase purchase = PurchaseFaker.Generate();

            purchase.Close();

            PurchaseItem item1 = PurchaseItemFaker.Generate(purchase);

            // Act
            Action act = () => purchase.AddItem(item1);

            // Assert
            act.Should().Throw<DomainException>();
        }


        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoTentarAtualizarUmItemInexistente")]
        [Trait("Purchase", "UpdateItemAsync")]
        public void Purchase_UpdateItem_DeveGerarDomainExceptionQuandoTentarAtualizarUmItemInexistente()
        {
            // Arange
            Purchase purchase = PurchaseFaker.Generate();

            PurchaseItem purchaseItem = PurchaseItemFaker.Generate(purchase);

            // Act
            Action act = () => purchase.UpdateItem(purchaseItem);

            // Assert
            act.Should().Throw<NotFoundException>();
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemComProdutoQueJaExisteNaLista")]
        [Trait("Purchase", "AddItem")]
        public void Purchase_AddItem_DeveGerarDomainExceptionQuandoAdicionarUmItemComProdutoQueJaExisteNaLista()
        {
            // Arange
            Purchase purchase = PurchaseFaker.Generate();

            Product product = ProductFaker.Generate();

            PurchaseItem purchaseItem1 = PurchaseItemFaker.Generate(purchase, product);
            PurchaseItem purchaseItem2 = PurchaseItemFaker.Generate(purchase, product);

            purchase.AddItem(purchaseItem1);

            // Act
            Action act = () => purchase.AddItem(purchaseItem2);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoAdicionarUmItemComProdutoQueJaExisteNaLista")]
        [Trait("Purchase", "UpdateItem")]
        public void Purchase_UpdateItem_DeveGerarDomainExceptionQuandoAtualizarUmItemComProdutoQueJaExisteNaLista()
        {
            // Arange
            Purchase purchase = PurchaseFaker.Generate();

            PurchaseItem purchaseItem1 = PurchaseItemFaker.Generate(purchase);
            PurchaseItem purchaseItem2 = PurchaseItemFaker.Generate(purchase);

            purchase.AddItem(purchaseItem1);
            purchase.AddItem(purchaseItem2);

            PurchaseItem purchaseItem3 = (PurchaseItem)purchaseItem2.ShallowCopy();

            purchaseItem3.Update(purchaseItem1.Amount, purchaseItem1.Value, purchaseItem1.Product);

            // Act
            Action act = () => purchase.UpdateItem(purchaseItem3);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "DeveAtualizarCorretamenteUmItemQuandoOMesmoExistir")]
        [Trait("Purchase", "UpdateItemAsync")]
        public void Purchase_UpdateItem_DeveAtualizarCorretamenteUmItemQuandoOMesmoExistir()
        {
            // Arange
            Purchase purchase = PurchaseFaker.Generate();

            PurchaseItem purchaseItem = PurchaseItemFaker.Generate(purchase);

            purchase.AddItem(purchaseItem);

            // Act
            purchase.UpdateItem(purchaseItem);

            // Assert
            purchase.Items.Should().HaveCount(1);
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteQuandoItemExistir")]
        [Trait("Purchase", "RemoveItemAsync")]
        public void Purchase_RemoveItem_DeveRemoverCorretamenteQuandoItemExistir()
        {
            // Arange
            Purchase purchase = PurchaseFaker.Generate();

            IList<PurchaseItem> itemsMock = PurchaseItemFaker.Generate(purchase, 2);

            purchase.AddItem(itemsMock.First());
            purchase.AddItem(itemsMock.Last());

            // Act
            purchase.RemoveItem(itemsMock.First());

            // Assert
            purchase.Items.Should().HaveCount(1);
            purchase.Items.Should().NotContain(x => x.Id == itemsMock.First().Id);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoRemoverUmItemInexistente")]
        [Trait("Purchase", "RemoveItemAsync")]
        public void Purchase_RemoveItem_DeveGerarDomainExceptionQuandoRemoverUmItemInexistente()
        {
            // Arange
            Purchase purchase = PurchaseFaker.Generate();

            IList<PurchaseItem> itemsMock = PurchaseItemFaker.Generate(purchase, 2);

            purchase.AddItem(itemsMock.First());

            // Act
            Action act = () => purchase.RemoveItem(itemsMock.Last());

            // Assert
            act.Should().Throw<NotFoundException>();
        }

        [Fact(DisplayName = "DeveSomarCorretamenteValorTotal")]
        [Trait("Purchase", "CalculateTotalValue")]
        public void Purchase_CalculateTotalValue_DeveSomarCorretamenteValorTotal()
        {
            // Arange && Act
            Purchase purchase = PurchaseFaker.Generate();

            IList<PurchaseItem> itemsMock = PurchaseItemFaker.Generate(purchase, 10);

            foreach (PurchaseItem itemMock in itemsMock)
                purchase.AddItem(itemMock);

            // Assert
            purchase.TotalValue.Should().Be(itemsMock.Sum(x => x.CalculteValue()));
        }

        [Fact(DisplayName = "DeveSetarOStatusParaFechadoCorretamenteSeEstiverAberto")]
        [Trait("Purchase", "CloseAsync")]
        public void Purchase_AddItem_DeveSetarOStatusParaFechadoCorretamenteSeEstiverAberto()
        {
            // Arange
            Purchase purchase = PurchaseFaker.Generate();

            // Act
            purchase.Close();

            // Assert
            purchase.State.Should().Be(PurchaseState.Closed);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionAoFecharORegistroQueEstaFechado")]
        [Trait("Purchase", "CloseAsync")]
        public void Purchase_RemoveItem_DeveGerarDomainExceptionAoFecharORegistroQueEstaFechado()
        {
            // Arange
            Purchase purchase = PurchaseFaker.Generate();

            purchase.Close();

            // Act
            Action act = () => purchase.Close();

            // Assert
            act.Should().Throw<DomainException>();
        }
    }
}
