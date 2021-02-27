using System;

namespace JacksonVeroneze.StockService.Core.Messages
{
    public abstract class Message
    {
        public string MessageType { get; private set; }

        public Guid AggregateId { get; protected set; }

        protected Message()
            => MessageType = GetType().Name;
    }
}
