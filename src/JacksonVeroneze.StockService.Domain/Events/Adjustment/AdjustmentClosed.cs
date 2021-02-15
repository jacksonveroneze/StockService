using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Adjustment
{
    public class AdjustmentClosed : DomainEvent
    {
        public AdjustmentClosed(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
