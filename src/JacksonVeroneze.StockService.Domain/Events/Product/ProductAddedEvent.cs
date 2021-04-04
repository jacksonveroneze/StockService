using System;
using JacksonVeroneze.StockService.Core.Messages;

namespace JacksonVeroneze.StockService.Domain.Events.Product
{
    public class ProductAddedEvent : Event
    {
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public ProductAddedEvent(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
