using System.Collections.Generic;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class Movement : Entity, IAggregateRoot
    {
        public virtual Product Product { get; private set; }

        private readonly List<MovementItem> _items = new List<MovementItem>();

        public virtual IReadOnlyCollection<MovementItem> Items => _items;

        protected Movement()
        {
        }

        public Movement(Product product)
        {
            Product = product;

            Validate();
        }

        public void AddItem(MovementItem item)
            => _items.Add(item);

        private void Validate()
        {
        }
    }
}
