using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JacksonVeroneze.StockService.Bus.Mediator;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Events.Purchase;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using JacksonVeroneze.StockService.Domain.Services;
using Moq;
using Xunit;

namespace JacksonVeroneze.StockService.Domain.Tests.Services
{
    public class PurchaseServiceTest
    {
        private IPurchaseService _purchaseService;
        //
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IPurchaseRepository> _purchaseRepositoryMock;
        private Mock<IBusHandler> _busHandlerMock;

        [Fact(DisplayName = "DeveAdicionarOsItensQuandoEmEstadoValid")]
        [Trait("PurchaseService", "AddItemAsync")]
        public void PurchaseService_AddItemAsync_DeveAdicionarOsItensQuandoEmEstadoValido()
        {
            // Arange
            int totalItens = 5;

            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();
            IList<PurchaseItem> purchaseItens = PurchaseItemFaker.GenerateFaker(purchase).Generate(totalItens);

            ConfigureMock();

            FactoryService();

            // Act
            foreach (PurchaseItem purchaseItem in purchaseItens)
                _purchaseService.AddItemAsync(purchase, purchaseItem);

            // Assert
            purchase.Items.Should().HaveCount(totalItens);
            purchase.TotalValue.Should().Be(purchaseItens.Sum(x => x.CalculteValue()));
            _purchaseRepositoryMock.Verify(x => x.Update(It.IsAny<Purchase>()), Times.Exactly(totalItens));
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<PurchaseItemAdded>()), Times.Exactly(totalItens));
        }

        [Fact(DisplayName = "DeveAtualizarOsItensQuandoEmEstadoValido")]
        [Trait("PurchaseService", "UpdateItemAsync")]
        public void PurchaseService_UpdateItemAsync_DeveAtualizarOsItensQuandoEmEstadoValido()
        {
            // Arange
            int totalItens = 5;

            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();
            IList<PurchaseItem> purchaseItens = PurchaseItemFaker.GenerateFaker(purchase).Generate(totalItens);

            ConfigureMock();

            FactoryService();

            foreach (PurchaseItem purchaseItem in purchaseItens)
                _purchaseService.AddItemAsync(purchase, purchaseItem);

            // Act
            _purchaseService.UpdateItemAsync(purchase, purchaseItens.First());

            // Assert
            purchase.Items.Should().HaveCount(totalItens);
            purchase.TotalValue.Should().Be(purchaseItens.Sum(x => x.CalculteValue()));
            _purchaseRepositoryMock.Verify(x => x.Update(It.IsAny<Purchase>()), Times.Exactly(totalItens + 1));
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens + 1));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<PurchaseItemUpdated>()), Times.Once);
        }

        [Fact(DisplayName = "DeveRemoverOItemQuandoOMesmoExistir")]
        [Trait("PurchaseService", "RemoveItemAsync")]
        public void PurchaseService_RemoveItemAsync_DeveRemoverOItemQuandoOMesmoExistir()
        {
            // Arange
            int totalItens = 5;

            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();
            IList<PurchaseItem> purchaseItens = PurchaseItemFaker.GenerateFaker(purchase).Generate(totalItens);

            ConfigureMock();

            FactoryService();

            foreach (PurchaseItem purchaseItem in purchaseItens)
                _purchaseService.AddItemAsync(purchase, purchaseItem);

            // Act
            _purchaseService.RemoveItemAsync(purchase, purchaseItens.First());

            // Assert
            purchase.Items.Should().HaveCount(totalItens - 1);
            purchase.TotalValue.Should().Be(purchaseItens.Skip(1).Sum(x => x.CalculteValue()));
            _purchaseRepositoryMock.Verify(x => x.Remove(It.IsAny<Purchase>()), Times.Once);
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens + 1));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<PurchaseItemRemoved>()), Times.Once);
        }

        [Fact(DisplayName = "DeveMudarOEstadoParaFechadoCorretamenteQuandoEstiverAberto")]
        [Trait("PurchaseService", "CloseAsync")]
        public void PurchaseService_CloseAsync_DeveMudarOEstadoParaFechadoCorretamenteQuandoEstiverAberto()
        {
            // Arange
            int totalItens = 5;

            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();
            IList<PurchaseItem> purchaseItens = PurchaseItemFaker.GenerateFaker(purchase).Generate(totalItens);

            ConfigureMock();

            FactoryService();

            foreach (PurchaseItem purchaseItem in purchaseItens)
                _purchaseService.AddItemAsync(purchase, purchaseItem);

            // Act
            _purchaseService.CloseAsync(purchase);

            // Assert
            purchase.State.Should().Be(PurchaseState.Closed);
            purchase.Items.Should().HaveCount(totalItens);
            purchase.TotalValue.Should().Be(purchaseItens.Sum(x => x.CalculteValue()));
            _purchaseRepositoryMock.Verify(x => x.Update(It.IsAny<Purchase>()), Times.Exactly(totalItens + 1));
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens + 1));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<PurchaseClosedEvent>()), Times.Once);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoQuandoEstiverFechadoETentarFecharNovamente")]
        [Trait("PurchaseService", "CloseAsync")]
        public void PurchaseService_CloseAsync_DeveGerarDomainExceptionQuandoQuandoEstiverFechadoETentarFecharNovamente()
        {
            // Arange
            Purchase purchase = PurchaseFaker.GenerateFaker().Generate();

            purchase.Close();

            ConfigureMock();

            FactoryService();

            // Act
            Action act = () => purchase.Close();

            // Assert
            act.Should().Throw<DomainException>();
            _purchaseRepositoryMock.Verify(x => x.Update(It.IsAny<Purchase>()), Times.Never);
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Never);
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<PurchaseClosedEvent>()), Times.Never);
        }

        private void ConfigureMock()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
            _busHandlerMock = new Mock<IBusHandler>();

            _purchaseRepositoryMock.SetupProperty(x => x.UnitOfWork, _unitOfWork.Object);

            _unitOfWork.Setup(x => x.CommitAsync())
                .Returns(Task.FromResult(true));

            _busHandlerMock.Setup(x => x.PublishDomainEvent<PurchaseItemAdded>(It.IsAny<PurchaseItemAdded>()))
                .Returns(Task.CompletedTask);

            _busHandlerMock.Setup(x => x.PublishDomainEvent<PurchaseItemRemoved>(It.IsAny<PurchaseItemRemoved>()))
                .Returns(Task.CompletedTask);

            _busHandlerMock.Setup(x => x.PublishDomainEvent<PurchaseClosedEvent>(It.IsAny<PurchaseClosedEvent>()))
                .Returns(Task.CompletedTask);

            _purchaseRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Purchase>()))
                .Returns(Task.CompletedTask);

            _purchaseRepositoryMock.Setup(x => x.Update(It.IsAny<Purchase>()));
        }

        private void FactoryService()
            => _purchaseService = new PurchaseService(_purchaseRepositoryMock.Object, _busHandlerMock.Object);
    }
}
