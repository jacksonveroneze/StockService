using System.Threading;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Output;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using MediatR;

namespace JacksonVeroneze.StockService.Domain.Handlers
{
    public class MovementOutputHandler : BaseMovementHandler, INotificationHandler<OutputClosedEvent>
    {
        private readonly IOutputRepository _outputRepository;

        public MovementOutputHandler(IOutputRepository outputRepository,
            IMovementRepository movementRepository,
            IMovementService movementService) : base(movementRepository, movementService)
        {
            _outputRepository = outputRepository;
        }

        public async Task Handle(OutputClosedEvent notification, CancellationToken cancellationToken)
        {
            Output output = await _outputRepository.FindAsync(notification.AggregateId);

            foreach (OutputItem outputItem in output.Items)
            {
                Movement movement = await SearchOrCreateMovement(outputItem.Product);

                int? lastAmmount = movement.FindLastAmmount();

                MovementItem movementItem = new((lastAmmount ?? 0) - outputItem.Amount, movement, outputItem);

                await _movementService.AddItemAsync(movement, movementItem);
            }
        }
    }
}
