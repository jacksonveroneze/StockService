using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly List<OutputItem> _items = new();
        public virtual IReadOnlyCollection<OutputItem> Items => _items;

        public bool HasItems => Items.Any();

        protected Output()
        {
        }

        public Output(string description, DateTime date)
        {
            Description = description;
            Date = date;

            Validate();
        }

        public void Close()
        {
            ValidateIsOpenState();

            State = OutputState.Closed;
        }

        public void Update(string description, DateTime date)
        {
            Description = description;
            Date = date;

            ValidateIsOpenState();
            Validate();
        }

        public void AddItem(OutputItem item)
        {
            ValidateIsOpenState();
            ValidateIfExistsDuplicatedItem(item);
            ValidateIfExistsItemByProduct(item);

            _items.Add(item);

            CalculateTotalValue();
        }

        public void UpdateItem(OutputItem item)
        {
            ValidateIsOpenState();
            ValidateIfItemNotExist(item);
            ValidateIfExistsItemByProduct(item);

            OutputItem outputItem = FindItem(item.Id);

            _items.Remove(outputItem);

            _items.Add(item);

            CalculateTotalValue();
        }

        public void RemoveItem(OutputItem item)
        {
            ValidateIsOpenState();

            ValidateIfItemNotExist(item);

            _items.Remove(item);

            CalculateTotalValue();
        }

        public OutputItem FindItem(Guid id)
            => Items.FirstOrDefault(x => x.Id == id);

        private void CalculateTotalValue()
            => TotalValue = Items.Sum(x => x.CalculteValue());

        private void ValidateIfItemNotExist(OutputItem item)
        {
            if (CheckIfExistsItemById(item.Id) is false)
                throw ExceptionsFactory.FactoryNotFoundException<Output>(item.Id);
        }

        private void ValidateIfExistsDuplicatedItem(OutputItem item)
        {
            OutputItem existItem = FindItem(item.Id);

            if (existItem != null)
                throw ExceptionsFactory.FactoryDomainException(Messages.ItemFound);
        }

        private void ValidateIfExistsItemByProduct(OutputItem item)
        {
            OutputItem outputItem = Items.FirstOrDefault(x => x.Product.Id == item.Product.Id && x.Id != item.Id);

            if (outputItem != null)
                throw ExceptionsFactory.FactoryDomainException(Messages.ProductFound);
        }

        private void ValidateIsOpenState()
        {
            if (State == OutputState.Closed)
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
