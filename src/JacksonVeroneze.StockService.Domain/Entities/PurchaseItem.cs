using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class PurchaseItem : Entity
    {
        public int Amount { get; set; }

        public decimal Value { get; set; }

        public Purchase Purchase { get; set; }

        protected PurchaseItem()
        {
        }

        public PurchaseItem(int amount, decimal value, Purchase purchase)
        {
            Amount = amount;
            Value = value;
            Purchase = purchase;
        }

        public decimal CalculteValue() => Value * Amount;

        public void UpdateItemFromOtherItem(PurchaseItem item)
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
