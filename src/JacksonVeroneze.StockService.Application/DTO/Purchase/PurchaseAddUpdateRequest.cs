using System;

namespace JacksonVeroneze.StockService.Application.DTO.Purchase
{
    public class PurchaseAddUpdateRequest
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}
