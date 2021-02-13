using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Purchase
{
    public class PurchaseClosed : DomainEvent
    {
        public PurchaseClosed(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
