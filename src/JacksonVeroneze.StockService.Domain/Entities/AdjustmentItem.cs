using System.Collections.Generic;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class AdjustmentItem : Entity
    {
        public int Amount { get; private set; }

        public virtual Adjustment Adjustment { get; private set; }

        public virtual Product Product { get; private set; }

        private readonly List<MovementItem> _movementItems = new();

        public virtual IReadOnlyCollection<MovementItem> MovementItems => _movementItems;

        protected AdjustmentItem()
        {
        }

        public AdjustmentItem(int amount, Adjustment adjustment, Product product)
        {
            Amount = amount;
            Adjustment = adjustment;
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
