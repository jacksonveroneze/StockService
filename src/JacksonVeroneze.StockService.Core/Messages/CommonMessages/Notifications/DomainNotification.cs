using System;
using MediatR;

namespace JacksonVeroneze.StockService.Core.Messages.CommonMessages.Notifications
{
    public class DomainNotification : Message, INotification
    {
        public DateTime Timestamp { get; private set; } = DateTime.Now;

        public Guid DomainNotificationId { get; private set; } = Guid.NewGuid();

        public int Version { get; private set; } = 1;

        public string Key { get; private set; }

        public string Value { get; private set; }

        public DomainNotification(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
