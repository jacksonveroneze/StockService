using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using JacksonVeroneze.StockService.Domain.Services;
using Moq;

namespace JacksonVeroneze.StockService.Domain.Tests.Services
{
    public class MovementServiceTest
    {
        private IMovementService _movementService;
        //
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMovementRepository> _movementRepositoryMock;

        private void FactoryService()
            => _movementService = new MovementService(_movementRepositoryMock.Object);
    }
}
