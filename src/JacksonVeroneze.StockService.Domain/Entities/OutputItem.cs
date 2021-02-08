using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class OutputItem : Entity
    {
        public int Amount { get; private set; }

        public decimal Value { get; private set; }

        public Output Output { get; private set; }

        public Product Product { get; private set; }

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

        public void UpdateItemFromOtherItem(OutputItem item)
        {
            Amount += item.Amount;
            Value += item.Value;
        }

        public void Update(int amount, decimal value)
        {
            Amount = amount;
            Value = value;
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
