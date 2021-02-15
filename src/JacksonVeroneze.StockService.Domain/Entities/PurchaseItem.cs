using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class PurchaseItem : Entity
    {
        public int Amount { get; private set; }

        public decimal Value { get; private set; }

        public Purchase Purchase { get; private set; }

        public Product Product { get; private set; }

        protected PurchaseItem()
        {
        }

        public PurchaseItem(int amount, decimal value, Purchase purchase, Product product)
        {
            Amount = amount;
            Value = value;
            Purchase = purchase;
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
