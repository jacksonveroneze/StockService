using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Output
{
    public class OutputClosedEvent : DomainEvent
    {
        public OutputClosedEvent(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
