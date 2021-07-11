using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JacksonVeroneze.NET.Commons.Exceptions;
using JacksonVeroneze.StockService.Infra.Bus;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Events.Devolution;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using JacksonVeroneze.StockService.Domain.Services;
using Moq;
using Xunit;

namespace JacksonVeroneze.StockService.Domain.Tests.Services
{
    public class DevolutionServiceTest
    {
        private IDevolutionService _devolutionService;
        //
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IDevolutionRepository> _devolutionRepositoryMock;
        private Mock<IBus> _busHandlerMock;

        [Fact(DisplayName = "DeveAdicionarOsItensQuandoEmEstadoValid")]
        [Trait("DevolutionService", "AddItemAsync")]
        public void DevolutionService_AddItemAsync_DeveAdicionarOsItensQuandoEmEstadoValido()
        {
            // Arange
            int totalItens = 5;

            Devolution devolution = DevolutionFaker.Generate();
            IList<DevolutionItem> devolutionItens = DevolutionItemFaker.Generate(devolution, totalItens);

            ConfigureMock();

            FactoryService();

            // Act
            foreach (DevolutionItem devolutionItem in devolutionItens)
                _devolutionService.AddItemAsync(devolution, devolutionItem);

            // Assert
            devolution.Items.Should().HaveCount(totalItens);
            _devolutionRepositoryMock.Verify(x => x.Update(It.IsAny<Devolution>()), Times.Exactly(totalItens));
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<DevolutionItemAdded>()), Times.Exactly(totalItens));
        }

        [Fact(DisplayName = "DeveAtualizarOsItensQuandoEmEstadoValido")]
        [Trait("DevolutionService", "UpdateItemAsync")]
        public void DevolutionService_UpdateItemAsync_DeveAtualizarOsItensQuandoEmEstadoValido()
        {
            // Arange
            int totalItens = 5;

            Devolution devolution = DevolutionFaker.Generate();
            IList<DevolutionItem> devolutionItens = DevolutionItemFaker.Generate(devolution, totalItens);

            ConfigureMock();

            FactoryService();

            foreach (DevolutionItem devolutionItem in devolutionItens)
                _devolutionService.AddItemAsync(devolution, devolutionItem);

            // Act
            _devolutionService.UpdateItemAsync(devolution, devolutionItens.First());

            // Assert
            devolution.Items.Should().HaveCount(totalItens);
            _devolutionRepositoryMock.Verify(x => x.Update(It.IsAny<Devolution>()), Times.Exactly(totalItens + 1));
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens + 1));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<DevolutionItemUpdated>()), Times.Once);
        }

        [Fact(DisplayName = "DeveRemoverOItemQuandoOMesmoExistir")]
        [Trait("DevolutionService", "RemoveItemAsync")]
        public void DevolutionService_RemoveItemAsync_DeveRemoverOItemQuandoOMesmoExistir()
        {
            // Arange
            int totalItens = 5;

            Devolution devolution = DevolutionFaker.Generate();
            IList<DevolutionItem> devolutionItens = DevolutionItemFaker.Generate(devolution, totalItens);

            ConfigureMock();

            FactoryService();

            foreach (DevolutionItem devolutionItem in devolutionItens)
                _devolutionService.AddItemAsync(devolution, devolutionItem);

            // Act
            _devolutionService.RemoveItemAsync(devolution, devolutionItens.First());

            // Assert
            devolution.Items.Should().HaveCount(totalItens - 1);
            _devolutionRepositoryMock.Verify(x => x.Update(It.IsAny<Devolution>()), Times.Exactly(totalItens + 1));
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens + 1));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<DevolutionItemRemoved>()), Times.Once);
        }

        [Fact(DisplayName = "DeveMudarOEstadoParaFechadoCorretamenteQuandoEstiverAberto")]
        [Trait("DevolutionService", "CloseAsync")]
        public void DevolutionService_CloseAsync_DeveMudarOEstadoParaFechadoCorretamenteQuandoEstiverAberto()
        {
            // Arange
            int totalItens = 5;

            Devolution devolution = DevolutionFaker.Generate();
            IList<DevolutionItem> devolutionItens = DevolutionItemFaker.Generate(devolution, totalItens);

            ConfigureMock();

            FactoryService();

            foreach (DevolutionItem devolutionItem in devolutionItens)
                _devolutionService.AddItemAsync(devolution, devolutionItem);

            // Act
            _devolutionService.CloseAsync(devolution);

            // Assert
            devolution.State.Should().Be(DevolutionState.Closed);
            devolution.Items.Should().HaveCount(totalItens);
            _devolutionRepositoryMock.Verify(x => x.Update(It.IsAny<Devolution>()), Times.Exactly(totalItens + 1));
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens + 1));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<DevolutionClosedEvent>()), Times.Once);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoQuandoEstiverFechadoETentarFecharNovamente")]
        [Trait("DevolutionService", "CloseAsync")]
        public void DevolutionService_CloseAsync_DeveGerarDomainExceptionQuandoQuandoEstiverFechadoETentarFecharNovamente()
        {
            // Arange
            Devolution devolution = DevolutionFaker.Generate();

            devolution.Close();

            ConfigureMock();

            FactoryService();

            // Act
            Action act = () => devolution.Close();

            // Assert
            act.Should().Throw<DomainException>();
            _devolutionRepositoryMock.Verify(x => x.Update(It.IsAny<Devolution>()), Times.Never);
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Never);
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<DevolutionClosedEvent>()), Times.Never);
        }

        private void ConfigureMock()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _devolutionRepositoryMock = new Mock<IDevolutionRepository>();
            _busHandlerMock = new Mock<IBus>();

            _devolutionRepositoryMock.SetupProperty(x => x.UnitOfWork, _unitOfWork.Object);

            _unitOfWork.Setup(x => x.CommitAsync())
                .Returns(Task.FromResult(true));

            _busHandlerMock.Setup(x => x.PublishDomainEvent<DevolutionItemAdded>(It.IsAny<DevolutionItemAdded>()))
                .Returns(Task.CompletedTask);

            _busHandlerMock.Setup(x => x.PublishDomainEvent<DevolutionItemRemoved>(It.IsAny<DevolutionItemRemoved>()))
                .Returns(Task.CompletedTask);

            _busHandlerMock.Setup(x => x.PublishDomainEvent<DevolutionClosedEvent>(It.IsAny<DevolutionClosedEvent>()))
                .Returns(Task.CompletedTask);

            _devolutionRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Devolution>()))
                .Returns(Task.CompletedTask);

            _devolutionRepositoryMock.Setup(x => x.Update(It.IsAny<Devolution>()));
        }

        private void FactoryService()
            => _devolutionService = new DevolutionService(_devolutionRepositoryMock.Object, _busHandlerMock.Object);
    }
}
