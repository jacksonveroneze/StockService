using System;
using MediatR;

namespace JacksonVeroneze.StockService.Core.Messages
{
    public abstract class Event : Message, INotification
    {
        public DateTime Timestamp { get; } = DateTime.Now;
    }
}
