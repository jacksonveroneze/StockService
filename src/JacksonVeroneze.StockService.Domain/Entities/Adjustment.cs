using System;
using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class Adjustment : Entity, IAggregateRoot
    {
        public string Description { get; private set; }

        public DateTime Date { get; private set; }

        public AdjustmentStateEnum State { get; private set; } = AdjustmentStateEnum.Open;

        public decimal TotalValue { get; private set; }

        private readonly List<AdjustmentItem> _items = new List<AdjustmentItem>();
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
            ValidateOpenState();

            if (ExistsItem(item))
                throw new DomainException(Messages.ItemFound);

            _items.Add(item);

            CalculateTotalValue();
        }

        public void UpdateItem(AdjustmentItem item)
        {
            ValidateExistsItem(item);

            AdjustmentItem existItem = FindItemById(item.Id);

            existItem.Update(item.Amount, item.Value, item.Product);

            CalculateTotalValue();
        }

        public void RemoveItem(AdjustmentItem item)
        {
            ValidateOpenState();

            ValidateExistsItem(item);

            _items.Remove(item);

            CalculateTotalValue();
        }

        public void Close()
        {
            if (State == AdjustmentStateEnum.Closed)
                throw new DomainException(Messages.RegisterClosed);

            State = AdjustmentStateEnum.Closed;
        }

        public AdjustmentItem FindItemById(Guid id)
            => _items.FirstOrDefault(x => x.Id == id);

        public void ValidateExistsItem(AdjustmentItem item)
        {
            if (ExistsItem(item) is false)
                throw new DomainException(Messages.ItemNotFound);
        }

        private void ValidateOpenState()
        {
            if (State == AdjustmentStateEnum.Closed)
                throw new DomainException(Messages.RegisterClosedNotMoviment);
        }

        private void CalculateTotalValue()
            => TotalValue = _items.Sum(x => x.CalculteValue());

        private bool ExistsItem(AdjustmentItem item)
            => _items.Any(x => x.Id == item.Id);

        private void Validate()
        {
            Validacoes.ValidarSeVazio(Description, "A descrição não pode estar vazia");
            Validacoes.ValidarTamanho(Description, 1, 100, "A descrição deve ter entre 1 e 100 caracteres");
            Validacoes.ValidarSeMaiorQue(Date, DateTime.Now, "A data não pode ser superior a data atual");
        }
    }
}
