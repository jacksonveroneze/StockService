using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events
{
    public class PurchaseClosed : DomainEvent
    {
        public Guid PurchaseItemId { get; protected set; }

        public PurchaseClosed(Guid aggregateId, Guid purchaseItemId) : base(aggregateId)
            => PurchaseItemId = purchaseItemId;
    }
}
