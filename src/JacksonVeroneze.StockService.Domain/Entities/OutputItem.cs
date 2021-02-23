using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class OutputItem : Entity
    {
        public int Amount { get; private set; }

        public decimal Value { get; private set; }

        public virtual Output Output { get; private set; }

        public virtual Product Product { get; private set; }

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
            Validacoes.ValidarSeMenorQue(Amount, 1, "A quantidade deve ser maior que zero");
            Validacoes.ValidarSeMenorQue(Value, 1, "O Valor deve ser maior que zero");
        }
    }
}
