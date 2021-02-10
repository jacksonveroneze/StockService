using System;

namespace JacksonVeroneze.StockService.Application.DTO.PurchaseItem
{
    public class PurchaseRemoveItem
    {
        public Guid PurchasId { get; set; }

        public Guid PurchasItemId { get; set; }
    }
}
