using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Product
{
    public class ProductLowStockEvent : DomainEvent
    {
        public ProductLowStockEvent(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
