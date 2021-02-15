using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Adjustment
{
    public class AdjustmentItemUpdated : DomainEvent
    {
        public AdjustmentItemUpdated(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
