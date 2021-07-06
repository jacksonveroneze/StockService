using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Output
{
    public class OutputUndoItemEvent : DomainEvent
    {
        public Guid MovementItemId { get; }

        public int Ammount { get; }

        public OutputUndoItemEvent(Guid aggregateId, Guid movementItemId, int ammount) : base(aggregateId)
        {
            MovementItemId = movementItemId;
            Ammount = ammount;
        }
    }
}
