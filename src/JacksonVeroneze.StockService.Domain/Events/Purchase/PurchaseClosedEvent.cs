using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Purchase
{
    public class PurchaseClosedEvent : DomainEvent
    {
        public PurchaseClosedEvent(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
