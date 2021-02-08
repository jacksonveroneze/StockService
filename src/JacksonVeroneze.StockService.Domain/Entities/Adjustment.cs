using System;
using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class Adjustment : Entity, IAggregateRoot
    {
        public string Description { get; private set; }

        public DateTime Date { get; private set; }

        public decimal TotalValue { get; private set; }

        public PurchaseStateEnum State { get; private set; } = PurchaseStateEnum.Open;

        private readonly List<AdjustmentItem> _items = new List<AdjustmentItem>();
        public IReadOnlyCollection<AdjustmentItem> Items => _items;

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
            ValidateOpenState();

            if (ExistsItem(item))
            {
                AdjustmentItem currentItem = _items.First(x => x.Id == item.Id);

                currentItem.UpdateItemFromOtherItem(item);

                RemoveItem(item);

                item = currentItem;
            }

            _items.Add(item);

            CalculateTotalValue();
        }

        public void RemoveItem(AdjustmentItem item)
        {
            ValidateOpenState();

            if (ExistsItem(item) is false)
                throw new DomainException("Este item não encontra-se como filho do registro atual.");

            _items.Remove(item);

            CalculateTotalValue();
        }

        private void CalculateTotalValue()
            => TotalValue = _items.Sum(x => x.CalculteValue());

        private bool ExistsItem(AdjustmentItem item)
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
