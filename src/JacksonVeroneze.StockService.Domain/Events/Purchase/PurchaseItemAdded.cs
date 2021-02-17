using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Purchase
{
    public class PurchaseItemAdded : DomainEvent
    {
        public PurchaseItemAdded(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
