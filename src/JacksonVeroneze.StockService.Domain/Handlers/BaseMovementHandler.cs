using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Handlers
{
    public abstract class BaseMovementHandler
    {
        protected readonly IMovementRepository _movementRepository;
        protected readonly IMovementService _movementService;

        protected BaseMovementHandler(
            IMovementRepository movementRepository,
            IMovementService movementService)
        {
            _movementRepository = movementRepository;
            _movementService = movementService;
        }

        protected async Task<Movement> SearchOrCreateMovement(Product product)
        {
            Movement movement = await _movementRepository
                .FindAsync(new MovementFilter {ProductId = product.Id});

            if (movement is not null)
                return movement;

            return await CreateMovement(product);
        }

        private async Task<Movement> CreateMovement(Product product)
        {
            Movement movement = new(product);

            await _movementRepository.AddAsync(movement);

            await _movementRepository.UnitOfWork.CommitAsync();

            return movement;
        }
    }
}
