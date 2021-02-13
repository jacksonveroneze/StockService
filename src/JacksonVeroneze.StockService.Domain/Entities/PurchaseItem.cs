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

        public void Update(int amount, decimal value)
        {
            Amount = amount;
            Value = value;

            Validate();
        }

        private void Validate()
        {
            Validacoes.ValidarSeNulo(Amount, "A quantidade não pode estar vazia");
            Validacoes.ValidarSeMenorQue(Amount, 1, "A quantidade deve ser maior que zero");
            Validacoes.ValidarSeNulo(Value, "O Valor não pode estar vazio");
            Validacoes.ValidarSeMenorQue(Value, 1, "O Valor deve ser maior que zero");
        }
    }
}
