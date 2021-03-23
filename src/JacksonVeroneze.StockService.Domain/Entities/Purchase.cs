using System;
using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.Exceptions;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class Purchase : EntityRoot, IAggregateRoot
    {
        public string Description { get; private set; }

        public DateTime Date { get; private set; }

        public PurchaseState State { get; private set; } = PurchaseState.Open;

        public decimal TotalValue { get; private set; }

        private readonly List<PurchaseItem> _items = new();
        public virtual IReadOnlyCollection<PurchaseItem> Items => _items;

        public bool HasItems => Items.Any();

        protected Purchase()
        {
        }

        public Purchase(string description, DateTime date)
        {
            Description = description;
            Date = date;

            Validate();
        }

        public void Close()
        {
            ValidateIsOpenState();

            State = PurchaseState.Closed;
        }

        public void Update(string description, DateTime date)
        {
            Description = description;
            Date = date;

            ValidateIsOpenState();
            Validate();
        }

        public void AddItem(PurchaseItem item)
        {
            ValidateIsOpenState();
            ValidateIfExistsDuplicatedItem(item);
            ValidateIfExistsItemByProduct(item);

            _items.Add(item);

            CalculateTotalValue();
        }

        public void UpdateItem(PurchaseItem item)
        {
            ValidateIsOpenState();
            ValidateIfItemNotExist(item);
            ValidateIfExistsItemByProduct(item);

            PurchaseItem purchaseItem = FindItem(item.Id);

            _items.Remove(purchaseItem);

            _items.Add(item);

            CalculateTotalValue();
        }

        public void RemoveItem(PurchaseItem item)
        {
            ValidateIsOpenState();

            ValidateIfItemNotExist(item);

            _items.Remove(item);

            CalculateTotalValue();
        }

        public PurchaseItem FindItem(Guid id)
            => Items.FirstOrDefault(x => x.Id == id);

        private void CalculateTotalValue()
            => TotalValue = Items.Sum(x => x.CalculteValue());

        private void ValidateIfItemNotExist(PurchaseItem item)
        {
            if (CheckIfExistsItemById(item.Id) is false)
                throw ExceptionsFactory.FactoryNotFoundException<Purchase>(item.Id);
        }

        private void ValidateIfExistsDuplicatedItem(PurchaseItem item)
        {
            PurchaseItem existItem = FindItem(item.Id);

            if (existItem != null)
                throw ExceptionsFactory.FactoryDomainException(Messages.ItemFound);
        }

        private void ValidateIfExistsItemByProduct(PurchaseItem item)
        {
            PurchaseItem purchaseItem = Items.FirstOrDefault(x => x.Product.Id == item.Product.Id && x.Id != item.Id);

            if (purchaseItem != null)
                throw ExceptionsFactory.FactoryDomainException(Messages.ProductFound);
        }

        private void ValidateIsOpenState()
        {
            if (State == PurchaseState.Closed)
                throw ExceptionsFactory.FactoryDomainException(Messages.RegisterClosedNotMoviment);
        }

        private bool CheckIfExistsItemById(Guid id)
            => Items.Any(x => x.Id == id);

        private void Validate()
        {
            Validacoes.ValidarSeVazio(Description, "A descrição não pode estar vazia");
            Validacoes.ValidarTamanho(Description, 1, 100, "A descrição deve ter entre 1 e 100 caracteres");
            Validacoes.ValidarSeMaiorQue(Date, DateTime.Now, "A data não pode ser superior a data atual");
        }
    }
}
