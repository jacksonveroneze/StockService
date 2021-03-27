using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Product
{
    public class ProductAddedEvent : DomainEvent
    {
        public ProductAddedEvent(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
