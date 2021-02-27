using System;
using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class Adjustment : Entity, IAggregateRoot
    {
        public string Description { get; private set; }

        public DateTime Date { get; private set; }

        public AdjustmentState State { get; private set; } = AdjustmentState.Open;

        public decimal TotalValue { get; private set; }

        private readonly List<AdjustmentItem> _items = new();
        public virtual IReadOnlyCollection<AdjustmentItem> Items => _items;

        protected Adjustment()
        {
        }

        public Adjustment(string description, DateTime date)
        {
            Description = description;
            Date = date;

            Validate();
        }

        public void AddItem(AdjustmentItem item)
        {
            ValidateDuplicatedItem(item);
            ValidateDuplicatedProduct(item);

            _items.Add(item);

            CalculateTotalValue();
        }

        public void UpdateItem(AdjustmentItem item)
        {
            ValidateDuplicatedItem(item);
            ValidateDuplicatedProduct(item);

            AdjustmentItem existItem = FindItemById(item.Id);

            existItem.Update(item.Amount, item.Value, item.Product);

            CalculateTotalValue();
        }

        public void RemoveItem(AdjustmentItem item)
        {
            ValidateOpenState();

            ValidateIfNotExistsItem(item);

            _items.Remove(item);

            CalculateTotalValue();
        }

        public void Close()
        {
            if (State == AdjustmentState.Closed)
                throw ExceptionsFactory.FactoryDomainException(Messages.RegisterClosed);

            State = AdjustmentState.Closed;
        }

        public void Update(string description, DateTime date)
        {
            Description = description;
            Date = date;

            Validate();
        }

        private void CalculateTotalValue()
            => TotalValue = Items.Sum(x => x.CalculteValue());

        public AdjustmentItem FindItemById(Guid id)
            => Items.FirstOrDefault(x => x.Id == id);

        private void ValidateIfNotExistsItem(AdjustmentItem item)
        {
            if (ExistsItem(item) is false)
                throw ExceptionsFactory.FactoryNotFoundException<Adjustment>(item.Id);
        }

        private void ValidateDuplicatedItem(AdjustmentItem item)
        {
            if (ExistsItem(item) is true)
                throw ExceptionsFactory.FactoryDomainException(Messages.ItemFound);
        }

        private void ValidateDuplicatedProduct(AdjustmentItem item)
        {
            if (ExistsProduct(item.Product) is true)
                throw ExceptionsFactory.FactoryDomainException(Messages.ProductFound);
        }

        private void ValidateOpenState()
        {
            if (State == AdjustmentState.Closed)
                throw ExceptionsFactory.FactoryDomainException(Messages.RegisterClosedNotMoviment);
        }

        private bool ExistsProduct(Product product)
            => Items.Any(x => x.Product.Id == product.Id);

        private bool ExistsItem(AdjustmentItem item)
            => Items.Any(x => x.Id == item.Id);

        private void Validate()
        {
            Validacoes.ValidarSeVazio(Description, "A descrição não pode estar vazia");
            Validacoes.ValidarTamanho(Description, 1, 100, "A descrição deve ter entre 1 e 100 caracteres");
            Validacoes.ValidarSeMaiorQue(Date, DateTime.Now, "A data não pode ser superior a data atual");
        }
    }
}
