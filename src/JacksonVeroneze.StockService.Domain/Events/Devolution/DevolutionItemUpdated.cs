using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Devolution
{
    public class DevolutionItemUpdated : DomainEvent
    {
        public DevolutionItemUpdated(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
