using System.Collections.Generic;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class AdjustmentItem : Entity
    {
        public int Amount { get; private set; }

        public decimal Value { get; private set; }

        public virtual Adjustment Adjustment { get; private set; }

        public virtual Product Product { get; private set; }

        private readonly List<MovementItem> _movementItems = new();

        public virtual IReadOnlyCollection<MovementItem> MovementItems => _movementItems;

        protected AdjustmentItem()
        {
        }

        public AdjustmentItem(int amount, decimal value, Adjustment adjustment, Product product)
        {
            Amount = amount;
            Value = value;
            Adjustment = adjustment;
            Product = product;

            Validate();
        }

        public decimal CalculteValue() => Value * Amount;

        public void Update(int amount, decimal value, Product product)
        {
            Amount = amount;
            Value = value;
            Product = product;

            Validate();
        }

        private void Validate()
        {
            Validacoes.ValidarSeMenorQue(Amount, 1, "A quantidade deve ser maior que zero");
            Validacoes.ValidarSeMenorQue(Value, 1, "O Valor deve ser maior que zero");
        }
    }
}
