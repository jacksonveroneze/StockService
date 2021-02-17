using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Services;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Bus.Mediator;
using JacksonVeroneze.StockService.Common.Fakers;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Purchase;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using JacksonVeroneze.StockService.Domain.Services;
using JacksonVeroneze.StockService.Mapper;
using Moq;
using Xunit;

namespace JacksonVeroneze.StockService.Application.Tests.Services
{
    public class PurchaseApplicationServiceTest
    {
        private IPurchaseApplicationService _purchaseApplicationService;

        //
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IPurchaseRepository> _purchaseRepositoryMock;
        private Mock<IBusHandler> _busHandlerMock;

        [Fact(DisplayName = "DeveBuscarORegistroERetornarODTOCorretamente")]
        [Trait("PurchaseApplicationService", "FindAsync")]
        public async Task PurchaseApplicationService_FindAsync_DeveBuscarORegistroERetornarODTOCorretamente()
        {
            // Arange
            ConfigureMock();
            FactoryService();

            // Act
            PurchaseDto purchaseDto = await _purchaseApplicationService.FindAsync(Guid.NewGuid());

            // Assert
            purchaseDto.Should().NotBeNull();
        }

        [Fact(DisplayName = "DeveBuscarOsRegistrosERetornarOsDTOsCorretamente")]
        [Trait("PurchaseApplicationService", "FindAllAsync")]
        public async Task PurchaseApplicationService_FindAllAsync_DeveBuscarOsRegistrosERetornarOsDTOsCorretamente()
        {
            // Arange
            ConfigureMock();
            FactoryService();

            // Act
            IList<PurchaseDto> listPurchaseDto = await _purchaseApplicationService.FindAllAsync();

            // Assert
            listPurchaseDto.Should().NotBeNull();
            listPurchaseDto.Should().HaveCount(100);
        }

        [Fact(DisplayName = "DeveCriarCorretamenteORegistroQuandoEmEstadoValido")]
        [Trait("PurchaseApplicationService", "AddAsync")]
        public async Task PurchaseApplicationService_AddAsync_DeveCriarCorretamenteORegistroQuandoEmEstadoValido()
        {
            // Arange
            ConfigureMock();
            FactoryService();

            AddOrUpdatePurchaseDto addOrUpdatePurchaseDto =
                AddOrUpdatePurchaseDtoFaker.GenerateValidFaker().Generate();

            // Act
            ApplicationDataResult<PurchaseDto> result =
                await _purchaseApplicationService.AddAsync(addOrUpdatePurchaseDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.Data.Should().NotBeNull();

            _purchaseRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Purchase>()), Times.Once);
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "DeveRetornarErrosDeValidacaoQuandoEmEstadoInvalido")]
        [Trait("PurchaseApplicationService", "AddAsync")]
        public async Task PurchaseApplicationService_AddAsync_DeveRetornarErrosDeValidacaoQuandoEmEstadoInvalido()
        {
            // Arange
            ConfigureMock();
            FactoryService();

            AddOrUpdatePurchaseDto addOrUpdatePurchaseDto =
                AddOrUpdatePurchaseDtoFaker.GenerateInvalidFaker().Generate();

            // Act
            ApplicationDataResult<PurchaseDto> result =
                await _purchaseApplicationService.AddAsync(addOrUpdatePurchaseDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
            result.Data.Should().BeNull();

            _purchaseRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Purchase>()), Times.Never);
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Never);
        }


        [Fact(DisplayName = "DeveAtualizarCorretamenteORegistroQuandoEmEstadoValido")]
        [Trait("PurchaseApplicationService", "UpdateAsync")]
        public async Task PurchaseApplicationService_UpdateAsync_DeveAtualizarCorretamenteORegistroQuandoEmEstadoValido()
        {
            // Arange
            ConfigureMock();
            FactoryService();

            AddOrUpdatePurchaseDto addOrUpdatePurchaseDto =
                AddOrUpdatePurchaseDtoFaker.GenerateValidFaker().Generate();

            // Act
            ApplicationDataResult<PurchaseDto> result =
                await _purchaseApplicationService.UpdateAsync(Guid.NewGuid(), addOrUpdatePurchaseDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.Data.Should().NotBeNull();

            _purchaseRepositoryMock.Verify(x => x.Update(It.IsAny<Purchase>()), Times.Once);
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "DeveRetornarErrosDeValidacaoQuandoEmEstadoInvalido")]
        [Trait("PurchaseApplicationService", "UpdateAsync")]
        public async Task PurchaseApplicationService_UpdateAsync_DeveRetornarErrosDeValidacaoQuandoEmEstadoInvalido()
        {
            // Arange
            ConfigureMock();
            FactoryService();

            AddOrUpdatePurchaseDto addOrUpdatePurchaseDto =
                AddOrUpdatePurchaseDtoFaker.GenerateInvalidFaker().Generate();

            // Act
            ApplicationDataResult<PurchaseDto> result =
                await _purchaseApplicationService.UpdateAsync(Guid.NewGuid(), addOrUpdatePurchaseDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
            result.Data.Should().BeNull();

            _purchaseRepositoryMock.Verify(x => x.Update(It.IsAny<Purchase>()), Times.Never);
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteORegistroQuandoEncontrarMesmo")]
        [Trait("PurchaseApplicationService", "RemoveAsync")]
        public void PurchaseApplicationService_RemoveAsync_DeveRemoverCorretamenteORegistroQuandoEncontrarMesmo()
        {
            // Arange
            ConfigureMock();
            FactoryService();

            // Act
            Func<Task> func = async () =>
                await _purchaseApplicationService.RemoveAsync(Guid.NewGuid());

            // Assert
            func.Should().NotThrow<DomainException>();
            _purchaseRepositoryMock.Verify(x => x.Remove(It.IsAny<Purchase>()), Times.Once);
            _unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "DeveRemoverCorretamenteORegistroQuandoEncontrarMesmo")]
        [Trait("PurchaseApplicationService", "CloseAsync")]
        public void PurchaseApplicationService_CloseAsync_DeveFecharCorretamenteORegistroQuandoEncontrarMesmo()
        {
            // Arange
            ConfigureMock();
            FactoryService();

            // Act
            Func<Task> func = async () =>
                await _purchaseApplicationService.CloseAsync(Guid.NewGuid());

            // Assert
            func.Should().NotThrow<DomainException>();
        }

        private void ConfigureMock()
        {
            _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _busHandlerMock = new Mock<IBusHandler>();

            _purchaseRepositoryMock.SetupProperty(x => x.UnitOfWork, _unitOfWork.Object);

            _unitOfWork.Setup(x => x.CommitAsync())
                .Returns(Task.FromResult(true));

            _purchaseRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(PurchaseFaker.GenerateFaker().Generate()));

            _purchaseRepositoryMock.Setup(x => x.FindAllAsync())
                .Returns(Task.FromResult(PurchaseFaker.GenerateFaker().Generate(100)));

            _purchaseRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Purchase>()))
                .Returns(Task.CompletedTask);

            _purchaseRepositoryMock.Setup(x => x.Update(It.IsAny<Purchase>()));

            _purchaseRepositoryMock.Setup(x => x.Remove(It.IsAny<Purchase>()));

            _busHandlerMock.Setup(x => x.PublishDomainEvent<PurchaseItemAdded>(It.IsAny<PurchaseItemAdded>()))
                .Returns(Task.CompletedTask);

            _busHandlerMock.Setup(x => x.PublishDomainEvent<PurchaseItemRemoved>(It.IsAny<PurchaseItemRemoved>()))
                .Returns(Task.CompletedTask);

            _busHandlerMock.Setup(x => x.PublishDomainEvent<PurchaseClosed>(It.IsAny<PurchaseClosed>()))
                .Returns(Task.CompletedTask);
        }

        private void FactoryService()
        {
            IMapper mapper =
                new MapperConfiguration(cfg => cfg.AddProfile<ProfileMapStock>())
                    .CreateMapper();

            IPurchaseService purchaseService =
                new PurchaseService(_purchaseRepositoryMock.Object, _busHandlerMock.Object);

            _purchaseApplicationService =
                new PurchaseApplicationService(mapper, purchaseService, _purchaseRepositoryMock.Object);
        }
    }
}
