using System.Collections.Generic;
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
    public class MovementOutputHandler : BaseMovementHandler, INotificationHandler<OutputClosedEvent>,
        INotificationHandler<OutputUndoItemEvent>
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

        public async Task Handle(OutputUndoItemEvent notification, CancellationToken cancellationToken)
        {
            Movement movement = await _movementRepository.FindAsync(notification.AggregateId);

            MovementItem movementItemToRemove = movement.FindItem(notification.MovementItemId);

            MovementItem movementItemFirstAjustment =
                await _movementRepository.FindFirstAjustment(movement.Id, movementItemToRemove.CreatedAt);

            IList<MovementItem> movementsToRecalc =
                await _movementRepository.FindItensToRecalcOnUndoOutputItemAsync(
                    movement.Product.Id,
                    movementItemToRemove.CreatedAt, movementItemFirstAjustment?.CreatedAt);

            foreach (MovementItem movementItemIt in movementsToRecalc)
            {
                movementItemIt.UpdateAmmount(movementItemIt.Amount + notification.Ammount);

                movement.UpdateItem(movementItemIt);
            }

            movement.RemoveItem(movementItemToRemove);

            _movementRepository.Update(movement);

            await _movementRepository.UnitOfWork.CommitAsync();
        }
    }
}
