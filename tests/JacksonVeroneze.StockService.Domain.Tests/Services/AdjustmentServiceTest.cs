using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JacksonVeroneze.StockService.Bus;
using JacksonVeroneze.StockService.Bus.Mediator;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.DomainObjects.Exceptions;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Events.Adjustment;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using JacksonVeroneze.StockService.Domain.Services;
using Moq;
using Xunit;

namespace JacksonVeroneze.StockService.Domain.Tests.Services
{
    public class AdjustmentServiceTest
    {
        private IAdjustmentService _adjustmentService;
        //
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IAdjustmentRepository> _adjustmentRepositoryMock;
        private Mock<IBus> _busHandlerMock;

        [Fact(DisplayName = "DeveAdicionarOsItensQuandoEmEstadoValid")]
        [Trait("AdjustmentService", "AddItemAsync")]
        public void AdjustmentService_AddItem_DeveAdicionarOsItensQuandoEmEstadoValido()
        {
            // Arange
            int totalItens = 5;

            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();
            IList<AdjustmentItem> adjustmentItens = AdjustmentItemFaker.GenerateFaker(adjustment).Generate(totalItens);

            ConfigureMock();

            FactoryService();

            // Act
            foreach (AdjustmentItem adjustmentItem in adjustmentItens)
                _adjustmentService.AddItemAsync(adjustment, adjustmentItem);

            // Assert
            adjustment.Items.Should().HaveCount(totalItens);
            adjustment.TotalValue.Should().Be(adjustmentItens.Sum(x => x.CalculteValue()));
            _adjustmentRepositoryMock.Verify(x => x.Update(It.IsAny<Adjustment>()), Times.Exactly(totalItens));
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<AdjustmentItemAdded>()), Times.Exactly(totalItens));
        }

        [Fact(DisplayName = "DeveAtualizarOsItensQuandoEmEstadoValido")]
        [Trait("AdjustmentService", "UpdateItemAsync")]
        public void AdjustmentService_UpdateItem_DeveAtualizarOsItensQuandoEmEstadoValido()
        {
            // Arange
            int totalItens = 5;

            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();
            IList<AdjustmentItem> adjustmentItens = AdjustmentItemFaker.GenerateFaker(adjustment).Generate(totalItens);

            ConfigureMock();

            FactoryService();

            foreach (AdjustmentItem adjustmentItem in adjustmentItens)
                _adjustmentService.AddItemAsync(adjustment, adjustmentItem);

            // Act
            _adjustmentService.UpdateItemAsync(adjustment, adjustmentItens.First());

            // Assert
            adjustment.Items.Should().HaveCount(totalItens);
            adjustment.TotalValue.Should().Be(adjustmentItens.Sum(x => x.CalculteValue()));
            _adjustmentRepositoryMock.Verify(x => x.Update(It.IsAny<Adjustment>()), Times.Exactly(totalItens + 1));
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens + 1));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<AdjustmentItemUpdated>()), Times.Once);
        }

        [Fact(DisplayName = "DeveRemoverOItemQuandoOMesmoExistir")]
        [Trait("AdjustmentService", "RemoveItemAsync")]
        public void AdjustmentService_RemoveItem_DeveRemoverOItemQuandoOMesmoExistir()
        {
            // Arange
            int totalItens = 5;

            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();
            IList<AdjustmentItem> adjustmentItens = AdjustmentItemFaker.GenerateFaker(adjustment).Generate(totalItens);

            ConfigureMock();

            FactoryService();

            foreach (AdjustmentItem adjustmentItem in adjustmentItens)
                _adjustmentService.AddItemAsync(adjustment, adjustmentItem);

            // Act
            _adjustmentService.RemoveItemAsync(adjustment, adjustmentItens.First());

            // Assert
            adjustment.Items.Should().HaveCount(totalItens - 1);
            adjustment.TotalValue.Should().Be(adjustmentItens.Skip(1).Sum(x => x.CalculteValue()));
            _adjustmentRepositoryMock.Verify(x => x.Remove(It.IsAny<Adjustment>()), Times.Once);
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens + 1));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<AdjustmentItemRemoved>()), Times.Once);
        }

        [Fact(DisplayName = "DeveMudarOEstadoParaFechadoCorretamenteQuandoEstiverAberto")]
        [Trait("AdjustmentService", "CloseAsync")]
        public void AdjustmentService_Close_DeveMudarOEstadoParaFechadoCorretamenteQuandoEstiverAberto()
        {
            // Arange
            int totalItens = 5;

            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();
            IList<AdjustmentItem> adjustmentItens = AdjustmentItemFaker.GenerateFaker(adjustment).Generate(totalItens);

            ConfigureMock();

            FactoryService();

            foreach (AdjustmentItem adjustmentItem in adjustmentItens)
                _adjustmentService.AddItemAsync(adjustment, adjustmentItem);

            // Act
            _adjustmentService.CloseAsync(adjustment);

            // Assert
            adjustment.State.Should().Be(AdjustmentState.Closed);
            adjustment.Items.Should().HaveCount(totalItens);
            adjustment.TotalValue.Should().Be(adjustmentItens.Sum(x => x.CalculteValue()));
            _adjustmentRepositoryMock.Verify(x => x.Update(It.IsAny<Adjustment>()), Times.Exactly(totalItens + 1));
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens + 1));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<AdjustmentClosedEvent>()), Times.Once);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoQuandoEstiverFechadoETentarFecharNovamente")]
        [Trait("AdjustmentService", "CloseAsync")]
        public void AdjustmentService_Close_DeveGerarDomainExceptionQuandoQuandoEstiverFechadoETentarFecharNovamente()
        {
            // Arange
            Adjustment adjustment = AdjustmentFaker.GenerateFaker().Generate();

            adjustment.Close();

            ConfigureMock();

            FactoryService();

            // Act
            Action act = () => adjustment.Close();

            // Assert
            act.Should().Throw<DomainException>();
            _adjustmentRepositoryMock.Verify(x => x.Update(It.IsAny<Adjustment>()), Times.Never);
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Never);
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<AdjustmentClosedEvent>()), Times.Never);
        }

        private void ConfigureMock()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _adjustmentRepositoryMock = new Mock<IAdjustmentRepository>();
            _busHandlerMock = new Mock<IBus>();

            _adjustmentRepositoryMock.SetupProperty(x => x.UnitOfWork, _unitOfWork.Object);

            _unitOfWork.Setup(x => x.CommitAsync())
                .Returns(Task.FromResult(true));

            _busHandlerMock.Setup(x => x.PublishDomainEvent<AdjustmentItemAdded>(It.IsAny<AdjustmentItemAdded>()))
                .Returns(Task.CompletedTask);

            _busHandlerMock.Setup(x => x.PublishDomainEvent<AdjustmentItemRemoved>(It.IsAny<AdjustmentItemRemoved>()))
                .Returns(Task.CompletedTask);

            _busHandlerMock.Setup(x => x.PublishDomainEvent<AdjustmentClosedEvent>(It.IsAny<AdjustmentClosedEvent>()))
                .Returns(Task.CompletedTask);

            _adjustmentRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Adjustment>()))
                .Returns(Task.CompletedTask);

            _adjustmentRepositoryMock.Setup(x => x.Update(It.IsAny<Adjustment>()));
        }

        private void FactoryService()
            => _adjustmentService = new AdjustmentService(_adjustmentRepositoryMock.Object, _busHandlerMock.Object);
    }
}
