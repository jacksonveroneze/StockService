using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Devolution
{
    public class DevolutionItemAdded : DomainEvent
    {
        public DevolutionItemAdded(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
