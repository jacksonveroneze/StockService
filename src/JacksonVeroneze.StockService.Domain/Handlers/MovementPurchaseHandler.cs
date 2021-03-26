using System.Threading;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Purchase;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using MediatR;

namespace JacksonVeroneze.StockService.Domain.Handlers
{
    public class MovementPurchaseHandler : BaseMovementHandler, INotificationHandler<PurchaseClosedEvent>
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public MovementPurchaseHandler(IPurchaseRepository purchaseRepository,
            IMovementRepository movementRepository,
            IMovementService movementService) : base(movementRepository, movementService)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task Handle(PurchaseClosedEvent notification, CancellationToken cancellationToken)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(notification.AggregateId);

            foreach (PurchaseItem purchaseItem in purchase.Items)
            {
                Movement movement = await SearchOrCreateMovement(purchaseItem.Product);

                int? lastAmmount = movement.FindLastAmmount();

                MovementItem movementItem = new((lastAmmount ?? 0) + purchaseItem.Amount, movement, purchaseItem);

                await _movementService.AddItemAsync(movement, movementItem);
            }
        }
    }
}
