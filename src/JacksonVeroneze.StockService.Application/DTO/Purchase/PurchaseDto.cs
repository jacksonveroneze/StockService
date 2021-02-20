using System;
using JacksonVeroneze.StockService.Domain;

namespace JacksonVeroneze.StockService.Application.DTO.Purchase
{
    public class PurchaseDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalValue { get; set; }

        public PurchaseStateEnum State { get; set; }
    }
}
