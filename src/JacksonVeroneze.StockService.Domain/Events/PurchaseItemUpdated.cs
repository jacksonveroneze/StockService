using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events
{
    public class PurchaseItemUpdated : DomainEvent
    {
        public PurchaseItemUpdated(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
