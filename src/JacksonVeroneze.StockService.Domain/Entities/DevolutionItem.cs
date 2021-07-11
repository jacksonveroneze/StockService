using System.Collections.Generic;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class DevolutionItem : Entity
    {
        public int Amount { get; private set; }

        public virtual Devolution Devolution { get; private set; }

        public virtual Product Product { get; private set; }

        private readonly List<MovementItem> _movementItems = new();

        public virtual IReadOnlyCollection<MovementItem> MovementItems => _movementItems;

        protected DevolutionItem()
        {
        }

        public DevolutionItem(int amount, Devolution devolution, Product product)
        {
            Amount = amount;
            Devolution = devolution;
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
