using System.Collections.Generic;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class MovementItem : Entity
    {
        public int Amount { get; private set; }

        public virtual Movement Movement { get; private set; }

        private readonly List<AdjustmentItem> _adjustmentItems = new();
        private readonly List<OutputItem> _outputItems = new();
        private readonly List<PurchaseItem> _purchaseItems = new();

        public virtual IReadOnlyCollection<AdjustmentItem> AdjustmentItems => _adjustmentItems;
        public virtual IReadOnlyCollection<OutputItem> OutputItems => _outputItems;
        public virtual IReadOnlyCollection<PurchaseItem> PurchaseItems => _purchaseItems;

        public MovementItem()
        {
        }

        public MovementItem(int amount, Movement movement)
        {
            Amount = amount;
            Movement = movement;

            Validate();
        }

        private void Validate()
        {
            Guards.ValidarSeMenorQue(Amount, 1, "A quantidade deve ser maior que zero");
        }
    }
}
