using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class Movement : EntityRoot, IAggregateRoot
    {
        public virtual Product Product { get; private set; }

        private readonly List<MovementItem> _items = new();

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

        public int? FindLastAmmount()
            => Items.OrderByDescending(x => x.CreatedAt).FirstOrDefault()?.Amount;

        private void Validate()
        {
        }
    }
}
