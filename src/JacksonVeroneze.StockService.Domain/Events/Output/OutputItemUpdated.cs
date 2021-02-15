using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Output
{
    public class OutputItemUpdated : DomainEvent
    {
        public OutputItemUpdated(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
