using System.Collections.Generic;
using JacksonVeroneze.StockService.Core.Messages;

namespace JacksonVeroneze.StockService.Core.DomainObjects
{
    public abstract class EntityRoot : Entity
    {
        private readonly List<Event> _notifications = new();

        public IReadOnlyCollection<Event> Notifications => _notifications?.AsReadOnly();

        protected EntityRoot()
        {
        }

        public void AddEvent(Event evento)
            => _notifications.Add(evento);

        public void RemoveEvent(Event evento)
            => _notifications.Remove(evento);

        public void ClearEvents()
            => _notifications.Clear();

        public override string ToString()
            => $"{base.ToString()} - {GetType().Name}: Id: {Id}, Notifications: {Notifications.Count}";
    }
}
