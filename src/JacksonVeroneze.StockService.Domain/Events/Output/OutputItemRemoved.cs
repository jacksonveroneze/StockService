using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Output
{
    public class OutputItemRemoved : DomainEvent
    {
        public OutputItemRemoved(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
