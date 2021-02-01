using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Domain.Entities
{
    public class PurchaseItem : Entity
    {
        public int Amount { get; set; }

        public decimal Value { get; set; }

        public Purchase Purchase { get; set; }

        public PurchaseItem()
        {

        }

        public PurchaseItem(int amount, decimal value, Purchase purchase)
        {
            Amount = amount;
            Value = value;
            Purchase = purchase;
        }

        public decimal CalculteValue() => Value * Amount;

        public void UpdateItemFromOtherPurchaseItem(PurchaseItem item)
        {
            Amount += item.Amount;
            Value += item.Value;
        }
    }
}
