using System;
using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.Exceptions;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class Devolution : EntityRoot, IAggregateRoot
    {
        public string Description { get; private set; }

        public DateTime Date { get; private set; }

        public DevolutionState State { get; private set; } = DevolutionState.Open;

        public decimal TotalValue { get; private set; }

        private readonly List<DevolutionItem> _items = new();
        public virtual IReadOnlyCollection<DevolutionItem> Items => _items;

        public bool HasItems => Items.Any();

        protected Devolution()
        {
        }

        public Devolution(string description, DateTime date)
        {
            Description = description;
            Date = date;

            Validate();
        }

        public void Open()
        {
            State = DevolutionState.Open;
        }

        public void Close()
        {
            ValidateIsOpenState();

            State = DevolutionState.Closed;
        }

        public void Update(string description, DateTime date)
        {
            ValidateIsOpenState();

            Description = description;
            Date = date;

            Validate();
        }

        public void AddItem(DevolutionItem item)
        {
            ValidateIsOpenState();
            ValidateIfExistsDuplicatedItem(item);
            ValidateIfExistsItemByProduct(item);

            _items.Add(item);
        }

        public void UpdateItem(DevolutionItem item)
        {
            ValidateIsOpenState();
            ValidateIfItemNotExist(item);
            ValidateIfExistsItemByProduct(item);

            DevolutionItem putputItem = FindItem(item.Id);

            _items.Remove(putputItem);

            _items.Add(item);
        }

        public void RemoveItem(DevolutionItem item)
        {
            ValidateIsOpenState();

            ValidateIfItemNotExist(item);

            _items.Remove(item);
        }

        public DevolutionItem FindItem(Guid id)
            => Items.FirstOrDefault(x => x.Id == id);

        private void ValidateIfItemNotExist(DevolutionItem item)
        {
            if (CheckIfExistsItemById(item.Id) is false)
                throw ExceptionsFactory.FactoryNotFoundException<Devolution>(item.Id);
        }

        private void ValidateIfExistsDuplicatedItem(DevolutionItem item)
        {
            DevolutionItem existItem = FindItem(item.Id);

            if (existItem != null)
                throw ExceptionsFactory.FactoryDomainException(Messages.ItemFound);
        }

        private void ValidateIfExistsItemByProduct(DevolutionItem item)
        {
            DevolutionItem putputItem = Items.FirstOrDefault(x => x.Product.Id == item.Product.Id && x.Id != item.Id);

            if (putputItem != null)
                throw ExceptionsFactory.FactoryDomainException(Messages.ProductFound);
        }

        private void ValidateIsOpenState()
        {
            if (State == DevolutionState.Closed)
                throw ExceptionsFactory.FactoryDomainException(Messages.RegisterClosedNotMoviment);
        }

        public bool CheckIfExistsItemById(Guid id)
            => Items.Any(x => x.Id == id);

        private void Validate()
        {
            Guards.ValidarSeVazio(Description, "A descrição não pode estar vazia");
            Guards.ValidarTamanho(Description, 1, 200, "A descrição deve ter entre 1 e 100 caracteres");
            Guards.ValidarSeMaiorQue(Date, DateTime.Now, "A data não pode ser superior a data atual");
        }
    }
}
