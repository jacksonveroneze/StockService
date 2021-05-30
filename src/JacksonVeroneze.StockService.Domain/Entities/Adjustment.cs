using System;
using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.Exceptions;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class Adjustment : EntityRoot, IAggregateRoot
    {
        public string Description { get; private set; }

        public DateTime Date { get; private set; }

        public AdjustmentState State { get; private set; } = AdjustmentState.Open;

        public decimal TotalValue { get; private set; }

        private readonly List<AdjustmentItem> _items = new();
        public virtual IReadOnlyCollection<AdjustmentItem> Items => _items;

        public bool HasItems => Items.Any();

        protected Adjustment()
        {
        }

        public Adjustment(string description, DateTime date)
        {
            Description = description;
            Date = date;

            Validate();
        }

        public void Close()
        {
            ValidateIsOpenState();

            State = AdjustmentState.Closed;
        }

        public void Update(string description, DateTime date)
        {
            Description = description;
            Date = date;

            ValidateIsOpenState();
            Validate();
        }

        public void AddItem(AdjustmentItem item)
        {
            ValidateIsOpenState();
            ValidateIfExistsDuplicatedItem(item);
            ValidateIfExistsItemByProduct(item);

            _items.Add(item);
        }

        public void UpdateItem(AdjustmentItem item)
        {
            ValidateIsOpenState();
            ValidateIfItemNotExist(item);
            ValidateIfExistsItemByProduct(item);

            AdjustmentItem purchaseItem = FindItem(item.Id);

            _items.Remove(purchaseItem);

            _items.Add(item);
        }

        public void RemoveItem(AdjustmentItem item)
        {
            ValidateIsOpenState();

            ValidateIfItemNotExist(item);

            _items.Remove(item);
        }

        public AdjustmentItem FindItem(Guid id)
            => Items.FirstOrDefault(x => x.Id == id);

        private void ValidateIfItemNotExist(AdjustmentItem item)
        {
            if (CheckIfExistsItemById(item.Id) is false)
                throw ExceptionsFactory.FactoryNotFoundException<Adjustment>(item.Id);
        }

        private void ValidateIfExistsDuplicatedItem(AdjustmentItem item)
        {
            AdjustmentItem existItem = FindItem(item.Id);

            if (existItem != null)
                throw ExceptionsFactory.FactoryDomainException(Messages.ItemFound);
        }

        private void ValidateIfExistsItemByProduct(AdjustmentItem item)
        {
            AdjustmentItem purchaseItem = Items.FirstOrDefault(x => x.Product.Id == item.Product.Id && x.Id != item.Id);

            if (purchaseItem != null)
                throw ExceptionsFactory.FactoryDomainException(Messages.ProductFound);
        }

        private void ValidateIsOpenState()
        {
            if (State == AdjustmentState.Closed)
                throw ExceptionsFactory.FactoryDomainException(Messages.RegisterClosedNotMoviment);
        }

        private bool CheckIfExistsItemById(Guid id)
            => Items.Any(x => x.Id == id);

        private void Validate()
        {
            Guards.ValidarSeVazio(Description, "A descrição não pode estar vazia");
            Guards.ValidarTamanho(Description, 1, 100, "A descrição deve ter entre 1 e 100 caracteres");
            Guards.ValidarSeMaiorQue(Date, DateTime.Now, "A data não pode ser superior a data atual");
        }
    }
}
