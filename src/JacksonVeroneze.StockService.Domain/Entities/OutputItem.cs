using System.Collections.Generic;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class OutputItem : Entity
    {
        public int Amount { get; private set; }

        public decimal Value { get; private set; }

        public virtual Output Output { get; private set; }

        public virtual Product Product { get; private set; }

        private readonly List<MovementItem> _movementItems = new();

        public virtual IReadOnlyCollection<MovementItem> MovementItems => _movementItems;

        protected OutputItem()
        {
        }

        public OutputItem(int amount, decimal value, Output output, Product product)
        {
            Amount = amount;
            Value = value;
            Output = output;
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
            Guards.ValidarSeMenorQue(Amount, 1, "A quantidade deve ser maior que zero");
            Guards.ValidarSeMenorQue(Value, 1, "O Valor deve ser maior que zero");
        }
    }
}
