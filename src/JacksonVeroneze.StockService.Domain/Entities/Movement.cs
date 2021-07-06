using System;
using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.Exceptions;

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

        public void UpdateItem(MovementItem item)
        {
            MovementItem putputItem = FindItem(item.Id);

            if (CheckIfExistsItemById(item.Id) is false)
                throw ExceptionsFactory.FactoryNotFoundException<MovementItem>(item.Id);

            putputItem.UpdateAmmount(item.Amount);
        }

        public void RemoveItem(MovementItem item)
        {
            ValidateIfItemNotExist(item);

            _items.Remove(item);
        }

        public int? FindLastAmmount()
            => Items.OrderByDescending(x => x.CreatedAt).FirstOrDefault()?.Amount;

        private void ValidateIfItemNotExist(MovementItem item)
        {
            if (CheckIfExistsItemById(item.Id) is false)
                throw ExceptionsFactory.FactoryNotFoundException<MovementItem>(item.Id);
        }

        public MovementItem FindItem(Guid id)
            => Items.FirstOrDefault(x => x.Id == id);

        private bool CheckIfExistsItemById(Guid id)
            => Items.Any(x => x.Id == id);

        private void Validate()
        {
        }
    }
}
