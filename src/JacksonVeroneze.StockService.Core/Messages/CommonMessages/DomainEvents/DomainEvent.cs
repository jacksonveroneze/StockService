using System;
using MediatR;

namespace JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents
{
    public abstract class DomainEvent : Event, IRequest
    {
        protected DomainEvent(Guid aggregateId)
            => AggregateId = aggregateId;
    }
}
