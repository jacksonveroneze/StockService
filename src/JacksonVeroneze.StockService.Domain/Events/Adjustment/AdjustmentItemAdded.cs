using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Adjustment
{
    public class AdjustmentItemAdded : DomainEvent
    {
        public AdjustmentItemAdded(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
