using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class AdjustmentItem : Entity
    {
        public int Amount { get; private set; }

        public decimal Value { get; private set; }

        public Adjustment Adjustment { get; private set; }

        public Product Product { get; private set; }

        protected AdjustmentItem()
        {
        }

        public AdjustmentItem(int amount, decimal value, Adjustment adjustment, Product product)
        {
            Amount = amount;
            Value = value;
            Adjustment = adjustment;
            Product = product;

            Validate();
        }

        public decimal CalculteValue() => Value * Amount;

        public void UpdateItemFromOtherItem(AdjustmentItem item)
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
