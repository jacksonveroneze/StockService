using System.Threading;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Adjustment;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using MediatR;

namespace JacksonVeroneze.StockService.Domain.Handlers
{
    public class MovementAdjustmentHandler : BaseMovementHandler, INotificationHandler<AdjustmentClosedEvent>
    {
        private readonly IAdjustmentRepository _adjustmentRepository;

        public MovementAdjustmentHandler(IAdjustmentRepository adjustmentRepository,
            IMovementRepository movementRepository,
            IMovementService movementService) : base(movementRepository, movementService)
        {
            _adjustmentRepository = adjustmentRepository;
        }

        public async Task Handle(AdjustmentClosedEvent notification, CancellationToken cancellationToken)
        {
            Adjustment adjustment = await _adjustmentRepository.FindAsync(notification.AggregateId);

            foreach (AdjustmentItem adjustmentItem in adjustment.Items)
            {
                Movement movement = await SearchOrCreateMovement(adjustmentItem.Product);

                MovementItem movementItem = new(adjustmentItem.Amount, movement, adjustmentItem);

                await _movementService.AddItemAsync(movement, movementItem);
            }
        }
    }
}
