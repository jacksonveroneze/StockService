using System.Threading;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Adjustment;
using JacksonVeroneze.StockService.Domain.Events.Output;
using JacksonVeroneze.StockService.Domain.Events.Purchase;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using MediatR;

namespace JacksonVeroneze.StockService.Domain.Handlers
{
    public class MovementHandler :
        INotificationHandler<AdjustmentClosedEvent>,
        INotificationHandler<OutputClosedEvent>,
        INotificationHandler<PurchaseClosedEvent>
    {
        private readonly IAdjustmentRepository _adjustmentRepository;
        private readonly IOutputRepository _outputRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMovementRepository _movementRepository;
        private readonly IMovementService _movementService;

        public MovementHandler(IAdjustmentRepository adjustmentRepository,
            IOutputRepository outputRepository,
            IPurchaseRepository purchaseRepository,
            IMovementRepository movementRepository,
            IMovementService movementService)
        {
            _adjustmentRepository = adjustmentRepository;
            _outputRepository = outputRepository;
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

        public async Task Handle(AdjustmentClosedEvent notification, CancellationToken cancellationToken)
        {
            Adjustment adjustment = await _adjustmentRepository.FindAsync(notification.AggregateId);

            foreach (AdjustmentItem purchaseItem in adjustment.Items)
            {
                Movement movement = await SearchMovement(purchaseItem.Product);

                MovementItem movementItem = FactoryMovementItem(purchaseItem.Amount, movement);

                await _movementService.AddItemAsync(movement, movementItem);
            }
        }

        public async Task Handle(OutputClosedEvent notification, CancellationToken cancellationToken)
        {
            Output output = await _outputRepository.FindAsync(notification.AggregateId);

            foreach (OutputItem purchaseItem in output.Items)
            {
                Movement movement = await SearchMovement(purchaseItem.Product);

                MovementItem movementItem = FactoryMovementItem(purchaseItem.Amount, movement);

                await _movementService.AddItemAsync(movement, movementItem);
            }
        }

        private async Task<Movement> SearchMovement(Product product)
        {
            Movement movement = await _movementRepository
                .FindAsync(new MovementFilter() {productId = product.Id});

            if (movement != null)
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

        private MovementItem FactoryMovementItem(int amount, Movement movement)
            => new MovementItem(amount, movement);
    }
}
