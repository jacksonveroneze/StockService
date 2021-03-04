using System;
using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.Exceptions;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class Output : Entity, IAggregateRoot
    {
        public string Description { get; private set; }

        public DateTime Date { get; private set; }

        public OutputState State { get; private set; } = OutputState.Open;

        public decimal TotalValue { get; private set; }

        private readonly List<OutputItem> _items = new List<OutputItem>();
        public virtual IReadOnlyCollection<OutputItem> Items => _items;

        protected Output()
        {
        }

        public Output(string description, DateTime date)
        {
            Description = description;
            Date = date;

            Validate();
        }

        public void AddItem(OutputItem item)
        {
            ValidateDuplicatedItem(item);
            ValidateDuplicatedProduct(item);

            _items.Add(item);

            CalculateTotalValue();
        }

        public void UpdateItem(OutputItem item)
        {
            ValidateDuplicatedItem(item);
            ValidateDuplicatedProduct(item);

            OutputItem existItem = FindItemById(item.Id);

            existItem.Update(item.Amount, item.Value, item.Product);

            CalculateTotalValue();
        }

        public void RemoveItem(OutputItem item)
        {
            ValidateOpenState();

            ValidateIfNotExistsItem(item);

            _items.Remove(item);

            CalculateTotalValue();
        }

        public void Close()
        {
            if (State == OutputState.Closed)
                throw ExceptionsFactory.FactoryDomainException(Messages.RegisterClosed);

            State = OutputState.Closed;
        }

        public void Update(string description, DateTime date)
        {
            Description = description;
            Date = date;

            Validate();
        }

        private void CalculateTotalValue()
            => TotalValue = Items.Sum(x => x.CalculteValue());

        public OutputItem FindItemById(Guid id)
            => Items.FirstOrDefault(x => x.Id == id);

        private void ValidateIfNotExistsItem(OutputItem item)
        {
            if (ExistsItem(item) is false)
                throw ExceptionsFactory.FactoryNotFoundException<Output>(item.Id);
        }

        private void ValidateDuplicatedItem(OutputItem item)
        {
            if (ExistsItem(item) is true)
                throw ExceptionsFactory.FactoryDomainException(Messages.ItemFound);
        }

        private void ValidateDuplicatedProduct(OutputItem item)
        {
            if (ExistsProduct(item.Product) is true)
                throw ExceptionsFactory.FactoryDomainException(Messages.ProductFound);
        }

        private void ValidateOpenState()
        {
            if (State == OutputState.Closed)
                throw ExceptionsFactory.FactoryDomainException(Messages.RegisterClosedNotMoviment);
        }

        private bool ExistsProduct(Product product)
            => Items.Any(x => x.Product.Id == product.Id);

        private bool ExistsItem(OutputItem item)
            => Items.Any(x => x.Id == item.Id);

        private void Validate()
        {
            Validacoes.ValidarSeVazio(Description, "A descrição não pode estar vazia");
            Validacoes.ValidarTamanho(Description, 1, 100, "A descrição deve ter entre 1 e 100 caracteres");
            Validacoes.ValidarSeMaiorQue(Date, DateTime.Now, "A data não pode ser superior a data atual");
        }
    }
}
