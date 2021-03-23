using System.Collections.Generic;
using System.Linq;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class Product : EntityRoot, IAggregateRoot
    {
        public string Description { get; private set; }

        public bool IsActive { get; private set; } = true;

        private readonly List<PurchaseItem> _itemsPurchase = new();
        private readonly List<AdjustmentItem> _itemsAdjustment = new();
        private readonly List<OutputItem> _itemsOutput = new();
        private readonly List<Movement> _itemsMovement = new();

        public virtual IReadOnlyCollection<PurchaseItem> ItemsPurchase => _itemsPurchase;
        public virtual IReadOnlyCollection<AdjustmentItem> ItemsAdjustment => _itemsAdjustment;
        public virtual IReadOnlyCollection<OutputItem> ItemsOutput => _itemsOutput;
        public virtual IReadOnlyCollection<Movement> ItemsMovement => _itemsMovement;

        public bool HasItems =>
            ItemsPurchase.Any() || ItemsAdjustment.Any() || ItemsOutput.Any() || ItemsMovement.Any();

        protected Product()
        {
        }

        public Product(string description)
        {
            Description = description;

            Validate();
        }

        public void Update(string description, bool isActive)
        {
            Description = description;
            IsActive = isActive;

            Validate();
        }

        public void Activate() => IsActive = true;

        public void Inactivate() => IsActive = false;

        private void Validate()
        {
            Validacoes.ValidarSeVazio(Description, "A descrição não pode estar vazia");
            Validacoes.ValidarTamanho(Description, 1, 100, "A descrição não pode estar vazia");
        }
    }
}
