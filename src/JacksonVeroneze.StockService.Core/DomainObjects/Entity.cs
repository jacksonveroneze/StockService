using System;
using System.Collections.Generic;
using JacksonVeroneze.StockService.Core.Messages;

namespace JacksonVeroneze.StockService.Core.DomainObjects
{
    public class Entity
    {
        public Guid Id { get; } = Guid.NewGuid();

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = null;

        public DateTime? DeletedAt { get; set; } = null;

        public int Version { get; set; } = 1;

        protected bool ValidState = false;

        public List<Event> _notifications = new List<Event>();

        public IReadOnlyCollection<Event> Notifications => _notifications?.AsReadOnly();

        protected Entity()
        {
        }

        public void AddEvent(Event evento)
            => _notifications.Add(evento);

        public void RemoveEvent(Event evento)
            => _notifications.Remove(evento);

        public void ClearEvents()
            => _notifications.Clear();

        public override bool Equals(object obj)
        {
            Entity compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
            => !(a == b);

        public override int GetHashCode()
            => (GetType().GetHashCode() * 907) + Id.GetHashCode();

        public virtual bool IsValid() => ValidState;

        public void IncrementVersion() => Version++;

        public override string ToString()
            => $"{GetType().Name}: Id: {Id}, CreatedAt: {CreatedAt}, " +
               $"UpdatedAt: {UpdatedAt}, DeletedAt: {DeletedAt}, Version: {Version}";
    }
}
