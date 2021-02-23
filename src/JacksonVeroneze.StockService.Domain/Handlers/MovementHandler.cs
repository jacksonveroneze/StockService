using System.Threading;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Purchase;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using MediatR;

namespace JacksonVeroneze.StockService.Domain.Handlers
{
    public class MovementHandler :
        INotificationHandler<PurchaseClosedEvent>
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMovementRepository _movementRepository;
        private readonly IMovementService _movementService;

        public MovementHandler(IPurchaseRepository purchaseRepository,
            IMovementRepository movementRepository,
            IMovementService movementService)
        {
            _purchaseRepository = purchaseRepository;
            _movementRepository = movementRepository;
            _movementService = movementService;
        }

        public async Task Handle(PurchaseClosedEvent notification, CancellationToken cancellationToken)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(notification.AggregateId);

            foreach (PurchaseItem purchaseItem in purchase.Items)
            {
                Movement movement = await SearchMovement(purchaseItem.Product);

                MovementItem movementItem = FactoryMovementItem(purchaseItem.Amount, movement);

                await _movementService.AddItemAsync(movement, movementItem);
            }
        }

        private async Task<Movement> SearchMovement(Product product)
        {
            Movement movement = await _movementRepository.FindByProductIdAsync(product.Id);

            if (movement != null)
                return movement;

            return await CreateMovement(product);
        }

        private async Task<Movement> CreateMovement(Product product)
        {
            Movement movement = new Movement(product);

            await _movementRepository.AddAsync(movement);

            await _movementRepository.UnitOfWork.CommitAsync();

            return movement;
        }

        private MovementItem FactoryMovementItem(int amount, Movement movement)
            => new MovementItem(amount, movement);
    }
}
