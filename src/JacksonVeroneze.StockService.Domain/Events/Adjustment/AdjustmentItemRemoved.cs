using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Adjustment
{
    public class AdjustmentItemRemoved : DomainEvent
    {
        public AdjustmentItemRemoved(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
