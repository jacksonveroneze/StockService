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
using JacksonVeroneze.StockService.Domain.Events.Output;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using JacksonVeroneze.StockService.Domain.Services;
using Moq;
using Xunit;

namespace JacksonVeroneze.StockService.Domain.Tests.Services
{
    public class OutputServiceTest
    {
        private IOutputService _outputService;
        //
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IOutputRepository> _outputRepositoryMock;
        private Mock<IBusHandler> _busHandlerMock;

        [Fact(DisplayName = "DeveAdicionarOsItensQuandoEmEstadoValid")]
        [Trait("OutputService", "AddItemAsync")]
        public void OutputService_AddItemAsync_DeveAdicionarOsItensQuandoEmEstadoValido()
        {
            // Arange
            int totalItens = 5;

            Output output = OutputFaker.GenerateFaker().Generate();
            IList<OutputItem> outputItens = OutputItemFaker.GenerateFaker(output).Generate(totalItens);

            ConfigureMock();

            FactoryService();

            // Act
            foreach (OutputItem outputItem in outputItens)
                _outputService.AddItemAsync(output, outputItem);

            // Assert
            output.Items.Should().HaveCount(totalItens);
            output.TotalValue.Should().Be(outputItens.Sum(x => x.CalculteValue()));
            _outputRepositoryMock.Verify(x => x.Update(It.IsAny<Output>()), Times.Exactly(totalItens));
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<OutputItemAdded>()), Times.Exactly(totalItens));
        }

        [Fact(DisplayName = "DeveAtualizarOsItensQuandoEmEstadoValido")]
        [Trait("OutputService", "UpdateItemAsync")]
        public void OutputService_UpdateItemAsync_DeveAtualizarOsItensQuandoEmEstadoValido()
        {
            // Arange
            int totalItens = 5;

            Output output = OutputFaker.GenerateFaker().Generate();
            IList<OutputItem> outputItens = OutputItemFaker.GenerateFaker(output).Generate(totalItens);

            ConfigureMock();

            FactoryService();

            foreach (OutputItem outputItem in outputItens)
                _outputService.AddItemAsync(output, outputItem);

            // Act
            _outputService.UpdateItemAsync(output, outputItens.First());

            // Assert
            output.Items.Should().HaveCount(totalItens);
            output.TotalValue.Should().Be(outputItens.Sum(x => x.CalculteValue()));
            _outputRepositoryMock.Verify(x => x.Update(It.IsAny<Output>()), Times.Exactly(totalItens + 1));
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens + 1));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<OutputItemUpdated>()), Times.Once);
        }

        [Fact(DisplayName = "DeveRemoverOItemQuandoOMesmoExistir")]
        [Trait("OutputService", "RemoveItemAsync")]
        public void OutputService_RemoveItemAsync_DeveRemoverOItemQuandoOMesmoExistir()
        {
            // Arange
            int totalItens = 5;

            Output output = OutputFaker.GenerateFaker().Generate();
            IList<OutputItem> outputItens = OutputItemFaker.GenerateFaker(output).Generate(totalItens);

            ConfigureMock();

            FactoryService();

            foreach (OutputItem outputItem in outputItens)
                _outputService.AddItemAsync(output, outputItem);

            // Act
            _outputService.RemoveItemAsync(output, outputItens.First());

            // Assert
            output.Items.Should().HaveCount(totalItens - 1);
            output.TotalValue.Should().Be(outputItens.Skip(1).Sum(x => x.CalculteValue()));
            _outputRepositoryMock.Verify(x => x.Remove(It.IsAny<Output>()), Times.Once);
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens + 1));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<OutputItemRemoved>()), Times.Once);
        }

        [Fact(DisplayName = "DeveMudarOEstadoParaFechadoCorretamenteQuandoEstiverAberto")]
        [Trait("OutputService", "CloseAsync")]
        public void OutputService_CloseAsync_DeveMudarOEstadoParaFechadoCorretamenteQuandoEstiverAberto()
        {
            // Arange
            int totalItens = 5;

            Output output = OutputFaker.GenerateFaker().Generate();
            IList<OutputItem> outputItens = OutputItemFaker.GenerateFaker(output).Generate(totalItens);

            ConfigureMock();

            FactoryService();

            foreach (OutputItem outputItem in outputItens)
                _outputService.AddItemAsync(output, outputItem);

            // Act
            _outputService.CloseAsync(output);

            // Assert
            output.State.Should().Be(OutputState.Closed);
            output.Items.Should().HaveCount(totalItens);
            output.TotalValue.Should().Be(outputItens.Sum(x => x.CalculteValue()));
            _outputRepositoryMock.Verify(x => x.Update(It.IsAny<Output>()), Times.Exactly(totalItens + 1));
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Exactly(totalItens + 1));
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<OutputClosedEvent>()), Times.Once);
        }

        [Fact(DisplayName = "DeveGerarDomainExceptionQuandoQuandoEstiverFechadoETentarFecharNovamente")]
        [Trait("OutputService", "CloseAsync")]
        public void OutputService_CloseAsync_DeveGerarDomainExceptionQuandoQuandoEstiverFechadoETentarFecharNovamente()
        {
            // Arange
            Output output = OutputFaker.GenerateFaker().Generate();

            output.Close();

            ConfigureMock();

            FactoryService();

            // Act
            Action act = () => output.Close();

            // Assert
            act.Should().Throw<DomainException>();
            _outputRepositoryMock.Verify(x => x.Update(It.IsAny<Output>()), Times.Never);
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Never);
            _busHandlerMock.Verify(x => x.PublishDomainEvent(It.IsAny<OutputClosedEvent>()), Times.Never);
        }

        private void ConfigureMock()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _outputRepositoryMock = new Mock<IOutputRepository>();
            _busHandlerMock = new Mock<IBusHandler>();

            _outputRepositoryMock.SetupProperty(x => x.UnitOfWork, _unitOfWork.Object);

            _unitOfWork.Setup(x => x.CommitAsync())
                .Returns(Task.FromResult(true));

            _busHandlerMock.Setup(x => x.PublishDomainEvent<OutputItemAdded>(It.IsAny<OutputItemAdded>()))
                .Returns(Task.CompletedTask);

            _busHandlerMock.Setup(x => x.PublishDomainEvent<OutputItemRemoved>(It.IsAny<OutputItemRemoved>()))
                .Returns(Task.CompletedTask);

            _busHandlerMock.Setup(x => x.PublishDomainEvent<OutputClosedEvent>(It.IsAny<OutputClosedEvent>()))
                .Returns(Task.CompletedTask);

            _outputRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Output>()))
                .Returns(Task.CompletedTask);

            _outputRepositoryMock.Setup(x => x.Update(It.IsAny<Output>()));
        }

        private void FactoryService()
            => _outputService = new OutputService(_outputRepositoryMock.Object, _busHandlerMock.Object);
    }
}
