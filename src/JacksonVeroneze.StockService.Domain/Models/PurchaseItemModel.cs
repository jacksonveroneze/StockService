using System;

namespace JacksonVeroneze.StockService.Domain.Models
{
    public class PurchaseItemModel
    {
        public Guid Id { get; set; }

        public int Amount { get; set; }

        public decimal Value { get; set; }

        public Guid PurchaseId { get; set; }

        public Guid ProductId { get; set; }

        public string ProductDescription { get; set; }
    }
}
