using System;
using JacksonVeroneze.StockService.Domain.Enums;

namespace JacksonVeroneze.StockService.Application.DTO.Purchase
{
    public class PurchaseDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public PurchaseState State { get; set; }
    }
}
