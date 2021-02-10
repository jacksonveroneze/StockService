using System;
using JacksonVeroneze.StockService.Domain;

namespace JacksonVeroneze.StockService.Application.DTO.Purchase
{
    public class PurchaseResponse
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalValue { get; private set; }

        public PurchaseStateEnum State { get; set; }
    }
}
