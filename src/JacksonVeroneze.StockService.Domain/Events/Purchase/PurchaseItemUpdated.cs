using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Purchase
{
    public class PurchaseItemUpdated : DomainEvent
    {
        public PurchaseItemUpdated(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
