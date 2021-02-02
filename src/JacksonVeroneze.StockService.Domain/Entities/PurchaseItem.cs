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

        public override bool IsValid() => true;
    }
}
