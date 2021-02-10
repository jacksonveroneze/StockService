using System;

namespace JacksonVeroneze.StockService.Application.DTO.PurchaseItem
{
    public class PurchaseAddUpdateItem
    {
        public Guid ProductId { get; set; }

        public int Amount { get; set; }

        public decimal Value { get; set; }
    }
}
