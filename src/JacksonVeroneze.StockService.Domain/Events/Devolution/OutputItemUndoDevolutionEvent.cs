using System;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Domain.Events.Devolution
{
    public class OutputItemUndoDevolutionEvent : DomainEvent
    {
        public string Description { get; }

        public int Ammount { get; }

        public Guid ProductId { get; }

        public OutputItemUndoDevolutionEvent(Guid aggregateId, string description, int ammount, Guid productId) : base(aggregateId)
        {
            Description = description;
            Ammount = ammount;
            ProductId = productId;
        }
    }
}
