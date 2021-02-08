using System;
using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class Output : Entity, IAggregateRoot
    {
        public string Description { get; private set; }

        public DateTime Date { get; private set; }

        public decimal TotalValue { get; private set; }

        public PurchaseStateEnum State { get; private set; } = PurchaseStateEnum.Open;

        private readonly List<OutputItem> _items = new List<OutputItem>();
        public IReadOnlyCollection<OutputItem> Items => _items;

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
            ValidateOpenState();

            if (ExistsItem(item))
            {
                OutputItem currentItem = _items.First(x => x.Id == item.Id);

                currentItem.UpdateItemFromOtherItem(item);

                RemoveItem(item);

                item = currentItem;
            }

            _items.Add(item);

            CalculateTotalValue();
        }

        public void RemoveItem(OutputItem item)
        {
            ValidateOpenState();

            if (ExistsItem(item) is false)
                throw new DomainException("Este item não encontra-se como filho do registro atual.");

            _items.Remove(item);

            CalculateTotalValue();
        }

        private void CalculateTotalValue()
            => TotalValue = _items.Sum(x => x.CalculteValue());

        private bool ExistsItem(OutputItem item)
            => _items.Any(x => x.Id == item.Id);

        public void Finish() => State = PurchaseStateEnum.Closed;

        private void ValidateOpenState()
        {
            if (State == PurchaseStateEnum.Closed)
                throw new DomainException("Este registro já está fechado, não pode ser movimentado.");
        }

        private void Validate()
        {
            Validacoes.ValidarSeVazio(Description, "A descrição não pode estar vazia");
            Validacoes.ValidarTamanho(Description, 1, 100, "A descrição não pode estar vazia");
            Validacoes.ValidarSeNulo(Date, "A data não pode estar vazia");
        }
    }
}
