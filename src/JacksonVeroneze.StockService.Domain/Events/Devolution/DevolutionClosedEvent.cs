using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Devolution
{
    public class DevolutionClosedEvent : DomainEvent
    {
        public DevolutionClosedEvent(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
