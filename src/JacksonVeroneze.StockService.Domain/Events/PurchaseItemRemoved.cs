using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events
{
    public class PurchaseItemRemoved : DomainEvent
    {
        public PurchaseItemRemoved(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
