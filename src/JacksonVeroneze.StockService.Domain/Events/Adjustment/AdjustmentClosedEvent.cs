using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Adjustment
{
    public class AdjustmentClosedEvent : DomainEvent
    {
        public AdjustmentClosedEvent(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
