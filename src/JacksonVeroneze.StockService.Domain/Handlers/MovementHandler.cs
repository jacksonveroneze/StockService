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
                Movement movement = await _movementRepository.FindByProductIdAsync(purchaseItem.Product.Id);

                if (movement is null)
                    await _movementRepository.AddAsync(new Movement(purchaseItem.Product));

                await _movementService.AddItemAsync(movement, new MovementItem(purchaseItem.Amount, movement));
            }
        }
    }
}
