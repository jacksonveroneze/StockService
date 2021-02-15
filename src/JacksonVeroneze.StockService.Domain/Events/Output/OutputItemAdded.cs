using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Output
{
    public class OutputItemAdded : DomainEvent
    {
        public OutputItemAdded(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
