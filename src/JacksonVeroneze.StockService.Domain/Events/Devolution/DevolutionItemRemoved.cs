using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Devolution
{
    public class DevolutionItemRemoved : DomainEvent
    {
        public DevolutionItemRemoved(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
