using System;

namespace JacksonVeroneze.StockService.Application.DTO.PurchaseItem
{
    public class AddOrUpdatePurchaseItemDto
    {
        public Guid PurchaseId { get; set; }

        public Guid ProductId { get; set; }

        public int Amount { get; set; }

        public decimal Value { get; set; }
    }
}
