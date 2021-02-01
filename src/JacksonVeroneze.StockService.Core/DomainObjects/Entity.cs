using System;
using System.Collections.Generic;
using JacksonVeroneze.StockService.Core.Messages;

namespace JacksonVeroneze.StockService.Core.DomainObjects
{
    public class Entity
    {
        public Guid Id { get; set; }

        public List<Event> _notifications = new List<Event>();

        public IReadOnlyCollection<Event> Notificacoes => _notifications?.AsReadOnly();

        protected Entity()
            => Id = Guid.NewGuid();

        public void AdicionarEvento(Event evento)
            => _notifications.Add(evento);

        public void RemoverEvento(Event evento)
            => _notifications.Remove(evento);

        public void LimparEventos()
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

        public virtual bool EhValido()
            => throw new NotImplementedException();

        public override string ToString()
            => $"{GetType().Name} [Id={Id}]";
    }
}
