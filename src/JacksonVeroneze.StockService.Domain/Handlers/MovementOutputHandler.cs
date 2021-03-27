using System.Threading;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Output;
using JacksonVeroneze.StockService.Domain.Events.Product;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using JacksonVeroneze.StockService.Infra.Bus;
using MediatR;

namespace JacksonVeroneze.StockService.Domain.Handlers
{
    public class MovementOutputHandler : BaseMovementHandler, INotificationHandler<OutputClosedEvent>
    {
        private readonly IOutputRepository _outputRepository;
        private readonly IBus _bus;

        public MovementOutputHandler(IOutputRepository outputRepository,
            IMovementRepository movementRepository,
            IMovementService movementService,
            IBus bus) : base(movementRepository, movementService)
        {
            _outputRepository = outputRepository;
            _bus = bus;
        }

        public async Task Handle(OutputClosedEvent notification, CancellationToken cancellationToken)
        {
            Output output = await _outputRepository.FindAsync(notification.AggregateId);

            foreach (OutputItem outputItem in output.Items)
            {
                Movement movement = await SearchOrCreateMovement(outputItem.Product);

                int? lastAmmount = movement.FindLastAmmount();

                int newAmmount = (lastAmmount ?? 0) - outputItem.Amount;

                if (newAmmount < 10)
                    await _bus.PublishDomainEvent(new ProductLowStockEvent(outputItem.Product.Id));

                MovementItem movementItem = new(newAmmount, movement, outputItem);

                await _movementService.AddItemAsync(movement, movementItem);
            }
        }
    }
}
