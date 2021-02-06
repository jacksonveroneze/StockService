using System.Collections.Generic;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class Product : Entity, IAggregateRoot
    {
        public string Description { get; private set; }

        public bool IsActive { get; private set; } = true;

        private readonly List<Purchase> _items = new List<Purchase>();
        public IReadOnlyCollection<Purchase> Items => _items;

        protected Product()
        {
        }

        public Product(string description)
        {
            Description = description;

            Validate();
        }

        public void UpdateDescription(string description)
        {
            Description = description;

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
