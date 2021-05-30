using System.Collections.Generic;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class OutputItem : Entity
    {
        public int Amount { get; private set; }

        public virtual Output Output { get; private set; }

        public virtual Product Product { get; private set; }

        private readonly List<MovementItem> _movementItems = new();

        public virtual IReadOnlyCollection<MovementItem> MovementItems => _movementItems;

        protected OutputItem()
        {
        }

        public OutputItem(int amount, Output output, Product product)
        {
            Amount = amount;
            Output = output;
            Product = product;

            Validate();
        }

        public void Update(int amount, Product product)
        {
            Amount = amount;
            Product = product;

            Validate();
        }

        private void Validate()
        {
            Guards.ValidarSeMenorQue(Amount, 1, "A quantidade deve ser maior que zero");
        }
    }
}
