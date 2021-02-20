using System;
using MediatR;

namespace JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents
{
    public abstract class DomainEvent : Message, INotification, IRequest
    {
        public DateTime Timestamp { get; } = DateTime.Now;

        protected DomainEvent(Guid aggregateId)
            => AggregateId = aggregateId;
    }
}
