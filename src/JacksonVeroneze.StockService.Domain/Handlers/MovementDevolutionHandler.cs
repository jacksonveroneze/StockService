using System.Threading;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Devolution;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using JacksonVeroneze.StockService.Infra.Bus;
using MediatR;

namespace JacksonVeroneze.StockService.Domain.Handlers
{
    public class MovementDevolutionHandler : BaseMovementHandler, INotificationHandler<DevolutionClosedEvent>
    {
        private readonly IDevolutionRepository _devolutionRepository;

        private readonly IBus _bus;

        public MovementDevolutionHandler(IDevolutionRepository devolutionRepository,
            IMovementRepository movementRepository,
            IMovementService movementService,
            IBus bus) : base(movementRepository, movementService)
        {
            _devolutionRepository = devolutionRepository;
            _bus = bus;
        }

        public async Task Handle(DevolutionClosedEvent notification, CancellationToken cancellationToken)
        {
            Devolution devolution = await _devolutionRepository.FindAsync(notification.AggregateId);

            foreach (DevolutionItem devolutionItem in devolution.Items)
            {
                Movement movement = await SearchOrCreateMovement(devolutionItem.Product);

                int? lastAmmount = movement.FindLastAmmount();

                int newAmmount = (lastAmmount ?? 0) + devolutionItem.Amount;

                MovementItem movementItem = new(newAmmount, movement, devolutionItem);

                await _movementService.AddItemAsync(movement, movementItem);
            }
        }

    }
}
